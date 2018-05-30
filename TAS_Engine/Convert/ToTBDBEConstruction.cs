using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using BHEE = BH.oM.Environmental.Elements;
using BH.Engine.Geometry;
using TBD;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {

        public static TBD.Construction ToTBDBEConstruction(BHEE.BuildingElement buildingElement, TBD.Building building)
        {
            TBD.Construction aConstruction = null;
            //TBD.Building aBuilding = new TBD.Building();  // HOW TO GET OUR CURRENT BUILDING???
            if (buildingElement != null)
                aConstruction = building.GetConstructionByName(ToTBDSurfaceType(buildingElement.ToTBDBEType()));
            return aConstruction;

        }

    }

    ///***************************************************/


    ///***************************************************/
}
