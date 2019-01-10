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

using System.ComponentModel;
using System.Collections.Generic;

using BH.oM.Base;
using BH.oM.Reflection.Attributes;
using BH.oM.Environment.Elements;

namespace BH.Engine.TAS
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Sets Tag for BHoMObject.")]
        [Input("bHoMObject", "BHoMObject")]
        [Input("tag", "tag to be set")]
        [Output("IBHoMObject")]
        public static BuildingElementType FixType(this BuildingElementType bHoMBuildingElementType, TBD.buildingElement tbdBuildingElement, TBD.zoneSurface tbdZoneSurface)
        {
            if (bHoMBuildingElementType == oM.Environment.Elements.BuildingElementType.Undefined)
            {
                if (tbdBuildingElement.name.Contains("-frame"))
                {
                    if (tbdZoneSurface.inclination == 0)
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.RooflightWithFrame;
                    else
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.WindowWithFrame;

                }
                else if (tbdBuildingElement.name.Contains("Floor"))
                {
                    bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Floor;
                }
                else if (tbdBuildingElement.name.Contains("Wall"))
                {
                    bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Wall;
                }

                else if ((tbdBuildingElement.name == "Air") || (tbdBuildingElement.name == "Air-internal"))
                {
                    if (tbdZoneSurface.inclination == 0 || tbdZoneSurface.inclination == 180)
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Floor;
                    else
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Wall;
                }
            }


            return bHoMBuildingElementType;
        }

        /***************************************************/
    }
}
