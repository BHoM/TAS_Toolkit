﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;
using BHEI = BH.oM.Environmental.Interface;
using BH.Engine.Environment;
using BHE = BH.oM.Environmental;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/

        public static float GetInclination(BHEI.IBuildingElementGeometry bHoMBuildingElementPanel)
        {

            double inclination;
            List<BHG.Point> pts = BH.Engine.Geometry.Query.IControlPoints(bHoMBuildingElementPanel.ICurve());


            BHG.Plane plane = BH.Engine.Geometry.Compute.FitPlane(pts);
               // Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            inclination = Math.Asin(plane.Normal.X)*(180/Math.PI);

            //if (plane.Normal.X == 0 && plane.Normal.Y == 0 && plane.Normal.Z == 1)
            //    inclination = 0; //ceiling
            //else if (plane.Normal.X == 0 && plane.Normal.Y == 0 && plane.Normal.Z == -1)
            //    inclination = 180; //floor
            //else
            //    inclination = 90; //walls

            return (float)inclination;
        }

        /***************************************************/
    }
}
