using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Adapters;
using Microsoft.ComplexEventProcessing.ManagementService;
using Microsoft.ComplexEventProcessing.Linq;

using System.Threading;

namespace StreamInsights
{
    class Program
    {
        public const int Timeout = 10000;
        public const int Interval = 2000;
        public const int NumberOfReading = 100;

        static void Main(string[] args)
        {


            var server = Server.Create("default");
            var application = server.CreateApplication("CellData");         

            // Configuration for input 1
            var input1Config = new SensorInputConfig
            {
                Timeout = Program.Timeout,
                Interval = Program.Interval,
                NumberOfReadings = Program.NumberOfReading
            };


            //string streamName = "{ \"sensor\":\"compass\",\"values\":{ \"x\":82.26953,\"y\":-17.796535,\"z\":-8.009412},\"type\":\"sensor\"}";

            // Instantiate input adapters
            var input1Stream = CepStream<SensorData>.Create("input1Stream", typeof(SensorInputAdapterFactory), input1Config, EventShape.Point);    

            // Configure output adapter
            var outputConfig = new SensorOutputConfig { AdapterStopSignal = "StopData" };


            // filtering logic for the json data
            // Join input adapters with a simple LINQ query
            var combinedInputStream = (from e in input1Stream
                                           //where (e.sensor == "compass" && Convert.ToDouble(e.values.x) > 149)
                                           //select e);
                                       where (e.sensor == "compass")
                                       select new SensorData
                                       {
                                           sensor = e.sensor,
                                           values = e.values,
                                           type = e.type,
                                           direction = Direction.GetDirection(e.values.x)
                                       });
       

            // Connect input adapters with output adapter
            var query = combinedInputStream.ToQuery(application, "CellInformation", "...", typeof(SensorOutputAdapterFactory), outputConfig, EventShape.Point, StreamEventOrder.ChainOrdered);


            // Instantiate semaphor for stop signal
            var adapterStopSignal = new EventWaitHandle(false,
                EventResetMode.ManualReset, outputConfig.AdapterStopSignal);

            // Run the query
            query.Start();
            adapterStopSignal.WaitOne();
            query.Stop();

            application.Delete();
            server.Dispose();

        }
    }
}
