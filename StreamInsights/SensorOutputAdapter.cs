using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing.Adapters;
using Microsoft.ComplexEventProcessing.ManagementService;

using System.Threading;
using Microsoft.ComplexEventProcessing;

namespace StreamInsights
{
    public class SensorOutputAdapter : TypedPointOutputAdapter<SensorData>
    {
        private EventWaitHandle _adapterStopSignal;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Adapter configuration</param>
        public SensorOutputAdapter(SensorOutputConfig config)
        {
            _adapterStopSignal = EventWaitHandle.OpenExisting(config.AdapterStopSignal);
        }

        public override void Resume()
        {
            ConsumeEvents();
        }

        public override void Start()
        {
            ConsumeEvents();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Main loop
        /// </summary>
        private void ConsumeEvents()
        {
            PointEvent<SensorData> currEvent;
            DequeueOperationResult result;

            try
            {
                // Run until stop state
                while (AdapterState != AdapterState.Stopping)
                {
                    result = Dequeue(out currEvent);

                    // Take a break if queue is empty
                    if (result == DequeueOperationResult.Empty)
                    {
                        PrepareToResume();
                        Ready();
                        return;
                    }
                    else
                    {
                        //PrintEvent(currEvent);

                        // Write to console
                        if (currEvent.EventKind == EventKind.Insert)
                        {
                            //Console.WriteLine("Received sensor data: " +
                            //    currEvent.StartTime + " " +
                            //    currEvent.Payload.sensor + " " +
                            //    currEvent.Payload.values.x);
                            Console.WriteLine("Received sensor data: " +
                               currEvent.StartTime + " - Device is pointing towards " +
                               currEvent.Payload.direction + " direction at " + currEvent.Payload.values.x + " deg");
                        }

                        ReleaseEvent(ref currEvent);
                    }
                }
                result = Dequeue(out currEvent);
                PrepareToStop(currEvent, result);
                Stopped();
            }
            catch (AdapterException e)
            {
                Console.WriteLine("AdvantIQ.StockTickerTypedPointOutput.ConsumeEvents - " + e.Message + e.StackTrace);
            }

            _adapterStopSignal.Set();
        }

        private void PrepareToResume()
        {
        }

        private void PrepareToStop(PointEvent<SensorData> currEvent, DequeueOperationResult result)
        {
            if (result == DequeueOperationResult.Success)
            {
                ReleaseEvent(ref currEvent);
            }
        }

        /// <summary>
        /// Used for debugging purposes
        /// </summary>
        /// <param name="evt"></param>
        private void PrintEvent(PointEvent<SensorData> evt)
        {
            if (evt.EventKind == EventKind.Cti)
            {
                //Console.WriteLine("Output: CTI " + evt.StartTime);
            }
            else
            {
                Console.WriteLine("Output: " + evt.EventKind + " " +
                    evt.StartTime + " " + evt.Payload.sensor + " " +
                    evt.Payload.values.x);
            }
        }
    }
}
