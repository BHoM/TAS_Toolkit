using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;
using BHEI = BH.oM.Environmental.Interface;
using BH.Engine.Environment;
using BH.Engine.Geometry;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static float GetFloorArea(this BHEE.Space bHoMSpace)
        {
            float floorArea;
            List<BHEE.BuildingElement> bHoMBuildingElement = bHoMSpace.BuildingElements;
            List<double> areaSum = new List<double>();
            foreach (BHEE.BuildingElement element in bHoMBuildingElement)
            {
                
                if (GetInclination(element.BuildingElementGeometry, bHoMSpace) == 180) // if floor
                {
                    floorArea = (float)element.BuildingElementGeometry.ICurve().IArea();
                    areaSum.Add(floorArea); //if we have many floor surfaces in the same space we ned to calculate the sum
                }
            }
            return (float)areaSum.Sum();
        }

        /***************************************************/
    }
}
