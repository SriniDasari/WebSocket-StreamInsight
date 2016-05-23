using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamInsights
{
   public class SensorData
    {
        public string sensor { get; set; }
        public Values values { get; set; }
        public string type { get; set; }
        public string direction { get; set; }
    }

    public class Values
    {
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
    }
}
