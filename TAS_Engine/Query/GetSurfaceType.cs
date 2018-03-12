//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BHG = BH.oM.Geometry;
//using BHE = BH.oM.Environmental;
//using BHEE = BH.oM.Environmental.Elements;
//using BHEI = BH.oM.Environmental.Interface;
//using BH.Engine.Environment;

//namespace BH.Engine.TAS
//{
//    public static partial class Query
//    {
//        /***************************************************/

//        public static TBD.SurfaceType GetSurfaceType(BHE.Elements.BuildingElement bHoMBuildingElement)
//        {

//            if (BH.Engine.Environment.Query.AdjacentSpaces(bHoMBuildingElement, ))
//            {
//                if (true) //under ground
//                    return TBD.SurfaceType.tbdGround;
//                else
//                    return TBD.SurfaceType.tbdExposed;
//            }
//            else if (GetAdjacentSpace(bHoMBuildingElementPanel, bHoMBuilding) >= 1)
//                return TBD.SurfaceType.tbdLink;

//            else
//            {
//                return TBD.SurfaceType.tbdNullLink; //Adiabatic
//            }

//        }

//        /***************************************************/
//    }
//}
