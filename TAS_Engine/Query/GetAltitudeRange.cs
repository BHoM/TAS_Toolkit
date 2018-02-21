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

        public static float GetAltitudeRange(BHEI.IBuildingElementGeometry bHoMBuildingElementPanel)
        {
            BHG.BoundingBox panelBoundingBox = BH.Engine.Geometry.Query.IBounds(bHoMBuildingElementPanel.ICurve());
            float altitudeRange = (float)panelBoundingBox.Max.Z - (float)panelBoundingBox.Min.Z;

            return altitudeRange;

        }

        /***************************************************/
    }
}
