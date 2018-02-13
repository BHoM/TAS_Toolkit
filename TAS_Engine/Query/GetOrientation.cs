using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static float GetOrientation(BHEE.BuildingElementPanel bHoMPanel)
        {
            List<BHG.Point> controlpoints = BH.Engine.Geometry.Query.ControlPoints(bHoMPanel.PolyCurve);
            BHG.Vector normal = Geometry.Compute.FitPlane(bHoMPanel.PolyCurve).Normal;

            float orientation;

            if (normal.X == 0 && normal.Y == 0 && normal.Z == 1)
                orientation = 270;
            else if (normal.X == 1 && normal.Y == 0 && normal.Z == 0)
                orientation = 90;
            else
                orientation = 0;

            return orientation;
        }

        /***************************************************/
    }
}
