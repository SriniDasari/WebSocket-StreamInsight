using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamInsights
{
    public static class Direction
    {
        public static string GetDirection(string coordinates)
        {
            double values = Convert.ToDouble(coordinates);
            string dir = "";
            if ((values >= 338 && values < 360) || (values >= 0 && values < 23))
                dir = "North";
            else if (values >= 23 && values < 68)
                dir = "North-East";
            else if (values >= 68 && values < 113)
                dir = "East";
            else if (values >= 113 && values < 158)
                dir = "South-East";
            else if (values >= 158 && values < 203)
                dir = "South";
            else if (values >= 203 && values < 248)
                dir = "South-West";
            else if (values >= 248 && values < 293)
                dir = "West";
            else if (values >= 293 && values < 338)
                dir = "North-West";

            return dir;
        }
    }
}
