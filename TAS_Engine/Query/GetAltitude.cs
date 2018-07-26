using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BHEI = BH.oM.Environment.Interface;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/

        public static float GetAltitude(BHEI.IBuildingElementGeometry bHoMBuildingElementPanel)
        {
            BHG.BoundingBox panelBoundingBox = BH.Engine.Geometry.Query.IBounds(bHoMBuildingElementPanel.ICurve());
            float altitude = (float)panelBoundingBox.Min.Z;

            return altitude;

        }

        /***************************************************/
    }
}
