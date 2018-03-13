using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;
using BHEI = BH.oM.Environmental.Interface;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static float GetVolume(BHEE.Space bHoMSpace)
        {
            //This does only work for a space where all of the building element panels have the same height. Will change this later

            List<BHEE.BuildingElement> bHoMBuildingElement = bHoMSpace.BuildingElements;
            List<BHEE.BuildingElementPanel> verticalPanels = new List<BHEE.BuildingElementPanel>();

            float roomheight = 0;
            foreach (BHEE.BuildingElement element in bHoMBuildingElement)
            {
                if (GetInclination(element.BuildingElementGeometry, bHoMSpace) == 90) // if wall
                {
                    roomheight = GetAltitudeRange(element.BuildingElementGeometry);
                    break;
                }
            }
           
            float volume = GetFloorArea(bHoMSpace) * roomheight;
            return volume;
        }

        /***************************************************/
    }
}
