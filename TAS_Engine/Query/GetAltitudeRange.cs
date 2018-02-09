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

        public static float GetAltitudeRange(BHEE.BuildingElementPanel bHoMBuildingElementPanel)
        {
            BHG.BoundingBox panelBoundingBox = BH.Engine.Geometry.Query.Bounds(bHoMBuildingElementPanel.PolyCurve);
            float altitudeRange = (float)panelBoundingBox.Max.Z - (float)panelBoundingBox.Min.Z;

            return altitudeRange;

        }

        /***************************************************/
    }
}
