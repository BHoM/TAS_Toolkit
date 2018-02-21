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

        public static float GetOrientation(BHEI.IBuildingElementGeometry bHoMPanel)
        {
            List<BHG.Point> controlpoints = BH.Engine.Geometry.Query.IControlPoints(bHoMPanel.ICurve());
            BHG.Vector normal = Geometry.Compute.FitPlane(controlpoints).Normal;

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
