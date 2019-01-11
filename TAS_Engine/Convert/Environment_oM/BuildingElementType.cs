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

using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental BuildingElementType from a TAS TBD BuildingElementType")]
        [Input("tbdType", "TAS TBD BuildingElementType")]
        [Output("BHoM Environmental BuildingElementType")]
        public static BHE.BuildingElementType ToBHoM(this TBD.BuildingElementType tbdType)
        {
            switch (tbdType)
            {
                case TBD.BuildingElementType.EXTERNALWALL:
                case TBD.BuildingElementType.INTERNALWALL:
                case TBD.BuildingElementType.UNDERGROUNDWALL:
                    return BHE.BuildingElementType.Wall;

                case TBD.BuildingElementType.ROOFELEMENT:
                case TBD.BuildingElementType.ROOFLIGHT:
                    return BHE.BuildingElementType.Roof;

                case TBD.BuildingElementType.CEILING:
                case TBD.BuildingElementType.UNDERGROUNDCEILING:
                    return BHE.BuildingElementType.Ceiling;

                case TBD.BuildingElementType.EXPOSEDFLOOR:
                case TBD.BuildingElementType.INTERNALFLOOR:
                case TBD.BuildingElementType.RAISEDFLOOR:
                case TBD.BuildingElementType.SLABONGRADE:
                case TBD.BuildingElementType.UNDERGROUNDSLAB:
                    return BHE.BuildingElementType.Floor;

                case TBD.BuildingElementType.DOORELEMENT:
                case TBD.BuildingElementType.VEHICLEDOOR:
                    return BHE.BuildingElementType.Door;

                case TBD.BuildingElementType.GLAZING:
                    return BHE.BuildingElementType.Window;

                case TBD.BuildingElementType.CURTAINWALL:
                    return BHE.BuildingElementType.CurtainWall;

                case TBD.BuildingElementType.FRAMEELEMENT:
                case TBD.BuildingElementType.NOBETYPE:
                case TBD.BuildingElementType.NULLELEMENT:
                case TBD.BuildingElementType.SHADEELEMENT:
                case TBD.BuildingElementType.SOLARPANEL:
                    return BHE.BuildingElementType.Undefined;

                default:
                    return BHE.BuildingElementType.Wall;
            }
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD BuildingElementType from a BHoM Environmental BuildingElementType")]
        [Input("element", "BHoM Environmental BuildingElement")]
        [Input("spaces", "Collection of BHoM Environmental BuildingElements that define a set of spaces for the building")]
        [Output("TAS TBD BuildingElementType")]
        public static TBD.BuildingElementType ToTASBuildingElementType(this BHE.BuildingElement element, List<List<BHE.BuildingElement>> spaces = null)
        {
            TBD.BuildingElementType tbdType = TBD.BuildingElementType.NULLELEMENT;
            if (element == null) return tbdType;
            if (spaces == null) spaces = new List<List<BHE.BuildingElement>>();

            int adjacentSpaces = element.AdjacentSpaces(spaces).Count;

            if (adjacentSpaces == 0 && element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && element.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)
                tbdType = TBD.BuildingElementType.SHADEELEMENT;

            if (element.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
            {
                object obj = element.BuildingElementProperties.CustomData["SAM_BuildingElementType"];
                if (obj != null)
                    tbdType = (TBD.BuildingElementType)ToTBDSurfaceType(obj.ToString());
            }
            else
                tbdType = TBD.BuildingElementType.NULLELEMENT;

            return tbdType;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets an int representing a TAS TBD BuildingElementType from a string defining the type")]
        [Input("type", "String naming the type of building element")]
        [Output("int representation of a TAS TBD BuildingElementType")]
        public static int ToTBDSurfaceType(string type)
        {
            switch (type)
            {
                //Strings from Revit
                case "No Type":
                    return 0;
                case "Internal Wall":
                    return 1;
                case "External Wall":
                    return 2;
                case "Roof":
                    return 3;
                case "Internal Floor":
                    return 4;
                case "Shade":
                    return 5;
                case "Underground Wall":
                    return 6;
                case "Underground Slab":
                    return 7;
                case "Internal Ceiling":
                    return 8;
                case "Underground Ceiling":
                    return 9;
                case "Raised Floor":
                    return 10;
                case "Slab on Grade":
                    return 11;
                case "Glazing":
                    return 12;
                case "Rooflight":
                    return 13;
                case "Door":
                    return 14;
                case "Frame":
                    return 15;
                case "Curtain Wall":
                    return 16;
                case "Air":
                    return 17;
                case "Solar / PV Panel":
                    return 18;
                case "Exposed Floor":
                    return 19;
                case "Vehicle Door":
                    return 20;

                default:
                    return 0; //Adiabatic
            }
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD BuildingElementType from a BHoM Environmental BuildingElementType")]
        [Input("type", "BHoM Environmental BuildingElementType")]
        [Output("TAS TBD BuildingElementType")]
        public static TBD.BuildingElementType ToTAS(this BHE.BuildingElementType bHoMBuildingElementType)
        {
            switch (bHoMBuildingElementType) // This is just a test, it doeas not match. We have more BETypes in TAS than in BHoM
            {
                // here we will need to have two levels or recognision ASHRAEBuilidingElementType as per new idraw graph

                //Check were we are refering this to
                case BHE.BuildingElementType.Wall:
                    return TBD.BuildingElementType.EXTERNALWALL; //What about the other TBD Wall types??
                case BHE.BuildingElementType.Roof:
                    return TBD.BuildingElementType.ROOFELEMENT;
                case BHE.BuildingElementType.Ceiling:
                    return TBD.BuildingElementType.UNDERGROUNDCEILING;
                case BHE.BuildingElementType.Floor:
                    return TBD.BuildingElementType.INTERNALFLOOR;
                default:
                    return TBD.BuildingElementType.EXTERNALWALL;
            }
        }
    }
}
