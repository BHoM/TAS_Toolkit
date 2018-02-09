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

        public static float GetAltitude(BHEE.BuildingElementPanel bHoMBuildingElementPanel)
        {
            BHG.BoundingBox panelBoundingBox = BH.Engine.Geometry.Query.Bounds(bHoMBuildingElementPanel.PolyCurve);
            float altitude = (float)panelBoundingBox.Min.Z;

            return altitude;

        }

        /***************************************************/
    }
}
