using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BHEI = BH.oM.Environment.Interface;
using BH.Engine.Environment;
using BHE = BH.oM.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/

        public static float GetInclination(BHEI.IBuildingElementGeometry bHoMBuildingElementPanel, BHE.Elements.Space bHoMSpace)
        {
            double inclination = 0;

            BHE.Elements.BuildingElementPanel panel = bHoMBuildingElementPanel as BHE.Elements.BuildingElementPanel;
            BHG.Polyline pline = new BHG.Polyline {ControlPoints = BH.Engine.Geometry.Query.IControlPoints(panel.PolyCurve) };

            List<BHG.Point> pts = BH.Engine.Geometry.Query.DiscontinuityPoints(pline);
            BHG.Plane plane = BH.Engine.Geometry.Create.Plane(pts[0], pts[1], pts[2]);

            BHG.Vector xyNormal = BH.Engine.Geometry.Create.Vector(0, 0, 1);
            inclination = BH.Engine.Geometry.Query.Angle(plane.Normal, xyNormal)* (180 / Math.PI);

            return (float)inclination;
        }

        /***************************************************/
    }
}
