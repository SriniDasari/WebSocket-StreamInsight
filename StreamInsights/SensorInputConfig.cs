
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamInsights
{
    public class SensorInputConfig
    {
        public int Timeout { get; set; }
        public int Interval { get; set; }
        public int NumberOfReadings { get; set; }
    }
}
