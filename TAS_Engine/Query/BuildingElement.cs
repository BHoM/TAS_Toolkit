/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using BH.oM.Geometry;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/
        [Description("Get TBD Building Element from building by Name")]
        [Input("tbdBuilding", "TBD Building")]
        [Input("name", "string name for building element ")]
        [Output("tbdBuildingElement", "return building element")]
        public static TBD.buildingElement BuildingElement(TBD.Building tbdBuilding, string name)
        {
            int index = 0;
            TBD.buildingElement tbdBuildingElement = null;
            while ((tbdBuildingElement = tbdBuilding.GetBuildingElement(index)) != null)
            {
                if (tbdBuildingElement.name == name)
                    return tbdBuildingElement;
                index++;
            }

            return null;
        }
        
        public static TBD.buildingElement BuildingElement(TBD.Building tbdBuilding, BH.oM.Geometry.ICurve panelCurve)
        {
            /*int index = 0;
            TBD.buildingElement tbdBuildingElement = null;
            while ((tbdBuildingElement = tbdBuilding.GetBuildingElement(index)) != null)
            {
                if (tbdBuildingElement. == panelCurve)
                    return tbdBuildingElement;
                index++;
            }*/

            return null;
        }

        /***************************************************/
    }
}
