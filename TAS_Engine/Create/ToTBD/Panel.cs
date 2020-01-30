/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.Geometry;
using BH.oM.Reflection.Attributes;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Fragments;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Create
    {
        public static Panel Panel(ICurve curve, double height)
        {
            if (curve == null) return null;
            Vector aVector = Geometry.Create.Vector(0, 0, height);
            Point aPoint_Min_1 = Geometry.Query.StartPoint(curve as dynamic);
            Point aPoint_Min_2 = Geometry.Query.EndPoint(curve as dynamic);

            Point aPoint_Max_1 = aPoint_Min_1 + aVector;
            Point aPoint_Max_2 = aPoint_Min_2 + aVector;

            Plane aPlane = Geometry.Create.Plane(aPoint_Max_1, Geometry.Create.Vector(0, 0, 1));
            ICurve aCurve = Geometry.Modify.Project(curve as dynamic, aPlane);
            Panel aBuildingElement = Environment.Create.Panel(externalEdges: aCurve.ToEdges());
            return aBuildingElement;
        }

        public static Panel BuildingElement(int elementID)
        {
            Panel aBuildingElement = new Panel()
            {
            };

            aBuildingElement.CustomData.Add(Convert.TBDAdapterID, elementID);
            return aBuildingElement;
        }
    }
}

