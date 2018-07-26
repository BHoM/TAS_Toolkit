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

        public static TBD.buildingElement BuildingElement(TBD.Building building, string name)
        {
            int index = 0;
            TBD.buildingElement buildingElement = null;
            while ((buildingElement = building.GetBuildingElement(index)) != null)
            {
                if (buildingElement.name == name)
                    return buildingElement;
                index++;
            }

            return null;

        }

        /***************************************************/
    }
}
