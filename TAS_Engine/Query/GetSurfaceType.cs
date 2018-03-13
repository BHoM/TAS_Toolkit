using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using BHEE = BH.oM.Environmental.Elements;
using BHEI = BH.oM.Environmental.Interface;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/

        public static TBD.SurfaceType GetSurfaceType(BHE.Elements.BuildingElement bHoMBuildingElement, IEnumerable<BHE.Elements.Space> spaces)
        {

            List<BHE.Elements.Space> adjSpace = BH.Engine.Environment.Query.AdjacentSpaces(bHoMBuildingElement, spaces, "");

            if (adjSpace == null)
                return TBD.SurfaceType.tbdNullLink; //Adiabatic; 



            if (adjSpace.Count <= 1)
            {
                if (bHoMBuildingElement.Level.Elevation < 0) //under ground. is this correct?
                    return TBD.SurfaceType.tbdGround;
                else
                    return TBD.SurfaceType.tbdExposed;
            }

            else if (adjSpace.Count > 1)
                return TBD.SurfaceType.tbdLink; //TODO: if it is linked we want to know to which space. 
            else
                return TBD.SurfaceType.tbdNullLink; //Adiabatic


        }

        /***************************************************/
    }
}
