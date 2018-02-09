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

        public static float GetInclination(BHEE.BuildingElementPanel bHoMBuildingElementPanel)
        {

            float inclination;
            BHG.Vector normal = Geometry.Compute.FitPlane(bHoMBuildingElementPanel.PolyCurve).Normal;

            if (Math.Round(normal.X) == 0 && Math.Round(normal.Y) == 0 && Math.Round(normal.Z) == -1)
                inclination = 0; //ceiling
            else if (Math.Round(normal.X) == 0 && Math.Round(normal.Y) == 0 && Math.Round(normal.Z) == 1)
                inclination = 180; //floor
            else
                inclination = 90; //walls

            return inclination;
        }

        /***************************************************/
    }
}
