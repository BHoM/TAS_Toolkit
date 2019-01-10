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

using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environment;
using BH.Engine.Geometry;
using TBD;
using System;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        public static int ToTBDBEType(this BHE.Elements.BuildingElement bHoMBuildingElement)
        {
            //Air= 17
            int type = 17;
            if (bHoMBuildingElement == null)
                return type;
            //TODO: AdjacentSpaces needs to be brought back to BuildingElement for this to work.
            //else if (bHoMBuildingElement.AdjacentSpaces.Count == 0 && bHoMBuildingElement.BuildingElementProperties.BuildingElementType != BHE.Elements.BuildingElementType.Window && bHoMBuildingElement.BuildingElementProperties.BuildingElementType != BHE.Elements.BuildingElementType.Door)
                // shade = 5
                type = 5;
            if (bHoMBuildingElement.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
            {
                object aObject = bHoMBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"];
                if (aObject != null)
                    type = ToTBDSurfaceType(aObject.ToString()); //convert the string
            }

            //else if (bHoMBuildingElement != null)
            //{
            //    type = ToTBDSurfaceType((bHoMBuildingElement.BuildingElementGeometry as BHE.Elements.BuildingElementPanel).ElementType);
            //}
            else
                type = 17;

            return type;
        }

        /***************************************************/
        //String modification for surface types
        

        ///***************************************************/
        ///
        public static String ToTBDSurfaceType(this int type)
        {
            switch (type)

            {
                //Strings from Revit
                case 0:
                    return "Adiabatic";
                case 1:
                    return "Internal Wall";
                case 2:
                    return "External Wall";
                case 3:
                    return "Roof";
                case 4:
                    return "Internal Floor";
                case 5:
                    return "Shade";
                case 6:
                    return "Underground Wall";
                case 7:
                    return "Underground Slab";
                case 8:
                    return "Internal Ceiling";
                case 9:
                    return "Underground Ceiling";
                case 10:
                    return "Raised Floor";
                case 11:
                    return "Slab on Grade";
                case 12:
                    return "Glazing";
                case 13:
                    return "Rooflight";
                case 14:
                    return "Door";
                case 15:
                    return "Frame";
                case 16:
                    return "Curtain Wall";
                case 17:
                    return "Adiabatic";
                case 18:
                    return "Solar / PV Panel";
                case 19:
                    return "Exposed Floor";
                case 20:
                    return "Vehicle Door";

                default:
                    return "Adiabatic"; //0
            }
        }

    }
}