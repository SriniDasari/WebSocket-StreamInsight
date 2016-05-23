using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing.Adapters;
using Microsoft.ComplexEventProcessing.ManagementService;
using Microsoft.ComplexEventProcessing;
using System.Threading;
using StreamInsights.WebSocketUtility;
using System.Net.WebSockets;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace StreamInsights
{
    public class SensorInputAdapter : TypedPointInputAdapter<SensorData>
    {

        private SensorInputConfig _config;      
        private PointEvent<SensorData> _pendingEvent;
        private string _deviceIpAddress = Convert.ToString(ConfigurationManager.AppSettings["MobileDeviceIpAddress"]);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration for this adapter</param>
        public SensorInputAdapter(SensorInputConfig config)
        {
            _config = config;          
        }

        public override void Resume()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            ProcessEvent().Wait();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private async Task ProcessEvent()
        {
            Console.WriteLine("Input Adapter Call ");

            var currEvent = default(PointEvent<SensorData>);
            var ctr = 0;

            EnqueueCtiEvent(DateTime.Now);

            try
            {
                if (AdapterState.Stopping == AdapterState)
                {
                    // first, check if the adapter is being asked to stop. If yes, then
                    // resolve the event from the last failed Enqueue;
                    // do housekeeping before calling stopped;
                    // declare the adapter as stopped; and then
                    // exit the worker thread
                    if (_pendingEvent != null)
                    {
                        currEvent = _pendingEvent;
                        _pendingEvent = null;
                    }

                    PrepareToStop(currEvent);
                    Stopped();
                    return;
                }

                // Loop until stop signal
                while (AdapterState != AdapterState.Stopping)
                {
                    if (_pendingEvent != null)
                    {
                        currEvent = _pendingEvent;
                        _pendingEvent = null;
                    }
                    else
                    {
                        
                        try
                        {
                            // Read from source                         
                            Thread.Sleep(1000); //wait for a sec, so server starts and ready to accept connection..

                            CustomClientWebSocket webSocket = new CustomClientWebSocket();
                            webSocket.Options.RequestHeaders["Connection"] = "Upgrade";                        
                            await webSocket.ConnectAsync(new Uri("ws://"+ _deviceIpAddress), CancellationToken.None);// 192.168.3.78
                            byte[] buffer = new byte[1024];

                            while (webSocket.State == WebSocketState.Open)
                            {
                                SensorData sensorData = null;
                                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                if (result.MessageType == WebSocketMessageType.Close)
                                {
                                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                                }
                                else
                                {
                                    
                                    string str = Encoding.UTF8.GetString(buffer);                                    
                                    var newString = str.Replace(str.Substring(str.LastIndexOf("\"sensor\"") + 1), "sensor\"}");
                                    //Console.WriteLine(newString.TrimEnd('\0'));
                                    try
                                    {
                                        sensorData = JsonConvert.DeserializeObject<SensorData>(newString);
                                    }
                                    catch (JsonException ex)
                                    {
                                        //Console.WriteLine("Exception Thrown at:  " + newString.TrimEnd('\0') + " Message: " + ex.Message);
                                        continue;
                                    }
                                }


                                // Produce INSERT event
                                currEvent = CreateInsertEvent();
                                currEvent.StartTime = DateTime.Now;
                                currEvent.Payload = sensorData;
                                _pendingEvent = null;
                                //PrintEvent(currEvent);
                                Enqueue(ref currEvent);

                                // Also send an CTI event
                                EnqueueCtiEvent(DateTime.Now);
                            }

                        }
                        catch (Exception ex)
                        {
                            // Error handling should go here
                        }
                     
                    }
                }
                //}

                if (_pendingEvent != null)
                {
                    currEvent = _pendingEvent;
                    _pendingEvent = null;
                }

                PrepareToStop(currEvent);
                Stopped();
            }
            catch (AdapterException e)
            {
                Console.WriteLine("AdvantIQ.StockTickerTypedPointInput.ProduceEvents - " + e.Message + e.StackTrace);
            }

        }

        private void PrepareToStop(PointEvent<SensorData> currEvent)
        {
            //EnqueueCtiEvent(DateTime.Now);
            if (currEvent != null)
            {
                // Do this to avoid memory leaks
                ReleaseEvent(ref currEvent);
            }
        }

        private void PrepareToResume(PointEvent<SensorData> currEvent)
        {
            _pendingEvent = currEvent;
        }

    }
}
