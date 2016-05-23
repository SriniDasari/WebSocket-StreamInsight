using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Adapters;

namespace StreamInsights
{
    public class SensorInputAdapterFactory: ITypedInputAdapterFactory<SensorInputConfig>
    {
        public InputAdapterBase Create<TPayLoad>(SensorInputConfig config, EventShape eventShape)
        {
            // Only support the point event model
            if (eventShape == EventShape.Point)
                return new SensorInputAdapter(config);
            else
                return default(InputAdapterBase);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
