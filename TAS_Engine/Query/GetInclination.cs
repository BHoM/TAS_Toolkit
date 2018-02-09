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

        public static float GetInclanation(BHEE.BuildingElementPanel bHoMBuildingElementPanel)
        {

            float inclanation; //TAS uses float and therefore we do that as well
            BHG.Vector normal = Geometry.Compute.FitPlane(bHoMBuildingElementPanel.PolyCurve).Normal;

            if (normal.X == 0 && normal.Y == 0 && normal.Z == -1)
                inclanation = 0; //ceiling
            else if (normal.X == 0 && normal.Y == 0 && normal.Z == 1)
                inclanation = 180; //floor
            else
                inclanation = 90; //walls

            return inclanation;
        }

        /***************************************************/
    }
}
