/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;

using BH.Engine.Environment;
using BHE = BH.oM.Environment;
using BH.Engine.Geometry;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Query
    {
        /***************************************************/
        /* //TODO: Fixes needed for the space to have properties.
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
         

            return perimeters.Sum();   
        }*/

        /***************************************************/
    }
}



