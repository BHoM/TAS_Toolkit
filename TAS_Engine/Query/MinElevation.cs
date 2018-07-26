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

        public static float MinElevation(TBD.Perimeter perimeter)
        {
            TBD.Polygon currPolygon = perimeter.GetFace();
            int indexepoints = 0;
            float currenZvaluet = float.MaxValue;
            TBD.TasPoint TasPoint = currPolygon.GetPoint(indexepoints);
            while (TasPoint != null)
            {
                if (TasPoint.z < currenZvaluet)
                    currenZvaluet = TasPoint.z;
                indexepoints++;
                TasPoint = currPolygon.GetPoint(indexepoints);
            }

            return currenZvaluet;
        }

        /***************************************************/
    }
}