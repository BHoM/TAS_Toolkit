using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;
using BHEI = BH.oM.Environmental.Interface;
using BH.Engine.Environment;
using BHE = BH.oM.Environmental;
using BH.Engine.Geometry;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static float GetOrientation(BHEI.IBuildingElementGeometry bHoMPanel, BHE.Elements.Space bHoMSpace)
        {

            float orientation;
            BHE.Elements.BuildingElementPanel panel = bHoMPanel as BHE.Elements.BuildingElementPanel;
            BHG.Polyline pline = new BHG.Polyline { ControlPoints = BH.Engine.Geometry.Query.IControlPoints(panel.PolyCurve) };
        

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);

            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            BHG.Vector xyNormal = BH.Engine.Geometry.Create.Vector(0, 1, 0);
            orientation = (float)(BH.Engine.Geometry.Query.Angle(plane.Normal, xyNormal) * (180 / Math.PI));

            //orientation = (float)(Math.Asin(plane.Normal.Y) * (180 / Math.PI));

            //float orientation;

            //if (normal.X == 0 && normal.Y == 0 && normal.Z == 1)
            //    orientation = 270;
            //else if (normal.X == 1 && normal.Y == 0 && normal.Z == 0)
            //    orientation = 90;
            //else
            //    orientation = 0;

            return orientation;
        }

        /***************************************************/
    }
}
