using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Adapters;

namespace StreamInsights
{
    public class SensorOutputAdapterFactory : ITypedOutputAdapterFactory<SensorOutputConfig>
    {
        public OutputAdapterBase Create<TPayload>(SensorOutputConfig config, EventShape eventShape)
        {
            // Only support the point event model
            if (eventShape == EventShape.Point)
                return new SensorOutputAdapter(config);
            else
                return default(OutputAdapterBase);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
