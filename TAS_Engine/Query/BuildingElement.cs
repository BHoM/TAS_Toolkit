using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BHEI = BH.oM.Environment.Interface;
using BH.Engine.Environment;
using BH.oM.Geometry;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static TBD.buildingElement BuildingElement(TBD.Building tbdBuilding, string name)
        {
            int index = 0;
            TBD.buildingElement tbdBuildingElement = null;
            while ((tbdBuildingElement = tbdBuilding.GetBuildingElement(index)) != null)
            {
                if (tbdBuildingElement.name == name)
                    return tbdBuildingElement;
                index++;
            }

            return null;
        }

        public static TBD.buildingElement BuildingElement(TBD.Building tbdBuilding, BH.oM.Geometry.ICurve panelCurve)
        {
            int index = 0;
            TBD.buildingElement tbdBuildingElement = null;
            while ((tbdBuildingElement = tbdBuilding.GetBuildingElement(index)) != null)
            {
                if (tbdBuildingElement. == panelCurve)
                    return tbdBuildingElement;
                index++;
            }

            return null;

        }

        /***************************************************/
    }
}
