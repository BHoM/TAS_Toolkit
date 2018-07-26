using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BHEI = BH.oM.Environment.Interface;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/

        public static System.Drawing.Color GetRGB(uint UInt)
        {
            byte b = (byte)(UInt >> 16);
            byte g = (byte)(UInt >> 8);
            byte r = (byte)(UInt >> 0);

            return System.Drawing.Color.FromArgb(r, b, g);

        }

        /***************************************************/
    }
}
