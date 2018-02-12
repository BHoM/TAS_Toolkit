using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static float GetFloorArea(BHEE.Space bHoMSpace)
        {
            float floorArea;
            List<BHEE.BuildingElementPanel> bHoMPanels = bHoMSpace.BuildingElementPanel;
            List<double> areaSum = new List<double>();
            foreach (BHEE.BuildingElementPanel panel in bHoMPanels)
            {
                if (GetInclination(panel) == 180) // if floor
                {
                    floorArea = (float)Engine.Geometry.Query.Area(panel.PolyCurve);
                    areaSum.Add(floorArea); //if we have many floor surfaces in the same space we ned to calculate the sum
                }
            }
            return (float)areaSum.Sum();
        }

        /***************************************************/
    }
}
