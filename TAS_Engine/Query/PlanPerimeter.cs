﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BHEI = BH.oM.Environment.Interface;
using BH.Engine.Environment;
using BHE = BH.oM.Environment;
using BH.Engine.Geometry;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/

        public static double PlanPerimeter(BH.oM.Environment.Elements.Space bHoMSpace)
        {
            List<BH.oM.Environment.Elements.BuildingElement> bHoMBuildingElement = bHoMSpace.BuildingElements;
            List<double> perimeters = new List<double>();
            foreach (BHEE.BuildingElement element in bHoMBuildingElement)
            {
                BHE.Elements.BuildingElement panel = bHoMSpace.BuildingElements[0].BuildingElementGeometry as BHE.Elements.BuildingElement;
                BHG.Polyline pline = new BHG.Polyline { ControlPoints = BH.Engine.Geometry.Query.IControlPoints(panel.PanelCurve) };

                if (BH.Engine.Environment.Query.Inclination(element.BuildingElementGeometry) == 180 || BH.Engine.Environment.Query.Inclination(element.BuildingElementGeometry) == 0)
                {
                    perimeters.Add(pline.Length()); //TODO: join perimeters
                }
            }

            return perimeters.Sum();
        }

        /***************************************************/
    }
}
