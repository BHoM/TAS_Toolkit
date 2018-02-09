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

        public static double GetVolume(BHEE.Space bHoMSpace)
        {
            //This does only work for a space where all of the building element panels have the same height. We can change this later on.

            List<BHEE.BuildingElementPanel> bHoMPanels = bHoMSpace.BuildingElementPanel;
            List<BHEE.BuildingElementPanel> verticalPanels = new List<BHEE.BuildingElementPanel>();

            foreach (BHEE.BuildingElementPanel panel in bHoMPanels)
            {
                if (GetInclanation(bHoMSpace.BuildingElementPanel[0]) == 90) // if wall
                {
                    verticalPanels.Add(panel);
                }
            }

            double volume = GetFloorArea(bHoMSpace) * GetAltitude(verticalPanels[0]);
            return volume;
        }

        /***************************************************/
    }
}
