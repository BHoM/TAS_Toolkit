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

        public static TBD.buildingElement BuildingElement(TBD.Building building, string name)
        {


            int index = 0;
            TBD.buildingElement BuildingElement = building.GetBuildingElement(index);
            while (BuildingElement != null)
            {
                if (BuildingElement.name == name)
                    return BuildingElement;
                index++;
                BuildingElement = building.GetBuildingElement(index);

            }

            return null;

        }

        /***************************************************/
    }
}
