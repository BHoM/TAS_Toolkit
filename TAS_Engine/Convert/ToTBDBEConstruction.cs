using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environment;
using BHEE = BH.oM.Environment.Elements;
using BH.Engine.Geometry;
using TBD;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {

        public static TBD.Construction ToTBDBEConstruction(BHEE.BuildingElement buildingElement, TBD.Building tbdBuilding)
        {
            TBD.Construction tbdConstruction = null;
            //TBD.Building aBuilding = new TBD.Building();  // HOW TO GET OUR CURRENT BUILDING???
            if (buildingElement != null)
                tbdConstruction = tbdBuilding.GetConstructionByName(ToTBDSurfaceType(buildingElement.ToTBDBEType()));
            return tbdConstruction;

        }

    }

    ///***************************************************/


    ///***************************************************/
}
