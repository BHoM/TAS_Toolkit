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

        public static float MinElevation(TBD.Perimeter tbdPerimeter)
        {
            TBD.Polygon tbdPolygon = tbdPerimeter.GetFace();
            int indexepoints = 0;
            float aZvalue = float.MaxValue;
            TBD.TasPoint TasPoint = tbdPolygon.GetPoint(indexepoints);
            while (TasPoint != null)
            {
                if (TasPoint.z < aZvalue)
                    aZvalue = TasPoint.z;
                indexepoints++;
                TasPoint = tbdPolygon.GetPoint(indexepoints);
            }

            return aZvalue;
        }

        /***************************************************/
    }
}