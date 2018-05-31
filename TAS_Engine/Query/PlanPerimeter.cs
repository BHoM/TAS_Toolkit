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

        public static double PlanPerimeter(BHE.Elements.Space bHoMSpace)
        {
            List<BHEE.BuildingElement> bHoMBuildingElement = bHoMSpace.BuildingElements;
            List<double> perimeters = new List<double>();
            foreach (BHEE.BuildingElement element in bHoMBuildingElement)
            {
                BHE.Elements.BuildingElementPanel panel = bHoMSpace.BuildingElements[0].BuildingElementGeometry as BHE.Elements.BuildingElementPanel;
                BHG.Polyline pline = new BHG.Polyline { ControlPoints = BH.Engine.Geometry.Query.IControlPoints(panel.PolyCurve) };

                if (GetInclination(element.BuildingElementGeometry, bHoMSpace) == 180 || GetInclination(element.BuildingElementGeometry, bHoMSpace) == 0)
                {
                    perimeters.Add(pline.Length()); //TODO: join perimeters
                }
            }

            return perimeters.Sum();
        }

        /***************************************************/
    }
}
