using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;

namespace BH.Engine.TAS.Query
{
    public static partial class Query
    {

        /***************************************************/

        public static double GetFloorArea(BHEE.Space bHoMSpace)
        {
            double floorArea = 0;
            List<BHEE.BuildingElementPanel> bHoMPanels = bHoMSpace.BuildingElementPanel;
            List<double> areaSum = new List<double>();
            foreach (BHEE.BuildingElementPanel panel in bHoMPanels)
            {
                if (GetInclanation(panel) == 180) // if floor
                floorArea = (float)Engine.Geometry.Query.Area(panel.PolyCurve);
                areaSum.Add(floorArea); //if we have many floor surfaces in the same space we ned to calculate the sum
            }
            return areaSum.Sum();
        }

        /***************************************************/
    }
}
