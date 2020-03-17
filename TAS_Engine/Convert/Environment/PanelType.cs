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

using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.Engine.Environment;
using BHP = BH.oM.Environment.Fragments;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("Gets a BHoM Environmental PanelType from a TAS TBD BuildingElementType")]
        [Input("tbdType", "TAS TBD BuildingElementType")]
        [Output("panelType", "BHoM Environmental PanelType")]
        public static BHE.PanelType FromTAS(this TBD.BuildingElementType tbdType)
        {
            switch (tbdType)
            {
                case TBD.BuildingElementType.EXTERNALWALL:
                    return BHE.PanelType.WallExternal;
                case TBD.BuildingElementType.INTERNALWALL:
                    return BHE.PanelType.WallInternal;
                case TBD.BuildingElementType.UNDERGROUNDWALL:
                    return BHE.PanelType.UndergroundWall;

                case TBD.BuildingElementType.ROOFELEMENT:
                    return BHE.PanelType.Roof;

                case TBD.BuildingElementType.CEILING:
                    return BHE.PanelType.Ceiling;
                case TBD.BuildingElementType.UNDERGROUNDCEILING:
                    return BHE.PanelType.UndergroundCeiling;

                case TBD.BuildingElementType.EXPOSEDFLOOR:
                    return BHE.PanelType.FloorExposed;
                case TBD.BuildingElementType.INTERNALFLOOR:
                    return BHE.PanelType.FloorInternal;
                case TBD.BuildingElementType.RAISEDFLOOR:
                    return BHE.PanelType.FloorRaised;
                case TBD.BuildingElementType.SLABONGRADE:
                    return BHE.PanelType.SlabOnGrade;
                case TBD.BuildingElementType.UNDERGROUNDSLAB:
                    return BHE.PanelType.UndergroundSlab;

                case TBD.BuildingElementType.CURTAINWALL:
                    return BHE.PanelType.CurtainWall;

                case TBD.BuildingElementType.NOBETYPE:
                case TBD.BuildingElementType.NULLELEMENT:
                    return BHE.PanelType.Undefined;

                case TBD.BuildingElementType.SHADEELEMENT:
                    return BHE.PanelType.Shade;
                case TBD.BuildingElementType.SOLARPANEL:
                    return BHE.PanelType.SolarPanel;

                default:
                    return BHE.PanelType.Wall;
            }
        }

        [Description("Gets a BHoM Environmental OpeningType from a TAS TBD BuildingElementType")]
        [Input("tbdType", "TAS TBD BuildingElementType")]
        [Output("openingType", "BHoM Environmental OpeningType")]
        public static BHE.OpeningType FromTASOpeningType(this TBD.BuildingElementType tbdType)
        {
            switch (tbdType)
            {
                case TBD.BuildingElementType.ROOFLIGHT:
                    return BHE.OpeningType.Rooflight;

                case TBD.BuildingElementType.DOORELEMENT:
                    return BHE.OpeningType.Door;
                case TBD.BuildingElementType.VEHICLEDOOR:
                    return BHE.OpeningType.VehicleDoor;

                case TBD.BuildingElementType.GLAZING:
                    return BHE.OpeningType.Glazing;

                case TBD.BuildingElementType.CURTAINWALL:
                    return BHE.OpeningType.CurtainWall;

                case TBD.BuildingElementType.FRAMEELEMENT:
                    return BHE.OpeningType.Frame;

                case TBD.BuildingElementType.NOBETYPE:
                case TBD.BuildingElementType.NULLELEMENT:
                    return BHE.OpeningType.Undefined;

                default:
                    return BHE.OpeningType.Window;
            }
        }

        [Description("Gets a TAS TBD BuildingElementType from a BHoM Environmental BuildingElementType")]
        [Input("element", "BHoM Environmental BuildingElement")]
        [Input("spaces", "Collection of BHoM Environmental BuildingElements that define a set of spaces for the building")]
        [Output("TAS TBD BuildingElementType")]
        public static TBD.BuildingElementType ToTASBuildingElementType(this BHE.Panel element, List<List<BHE.Panel>> spaces = null)
        {
            TBD.BuildingElementType tbdType = TBD.BuildingElementType.NULLELEMENT;
            if (element == null) return tbdType;
            if (spaces == null) spaces = new List<List<BHE.Panel>>();
            
            int adjacentSpaces = element.AdjacentSpaces(spaces).Count;
            if (adjacentSpaces == 0)
                tbdType = TBD.BuildingElementType.SHADEELEMENT;

            if (element.CustomData.ContainsKey("SAM_BuildingElementType"))
            {
                object obj = element.CustomData["SAM_BuildingElementType"];
                if (obj != null)
                    tbdType = (TBD.BuildingElementType)ToTBDSurfaceType(obj.ToString());
            }
            else
                tbdType = TBD.BuildingElementType.NULLELEMENT;

            return tbdType;
        }

        [Description("Gets an int representing a TAS TBD BuildingElementType from a string defining the type")]
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

        [Description("Gets a TAS TBD BuildingElementType from a BHoM Environmental BuildingElementType")]
        [Input("bHoMBuildingElementTyp", "Set the BuildingElementType")]
        [Output("TAS TBD BuildingElementType")]
        public static TBD.BuildingElementType ToTAS(this BHE.PanelType bHoMBuildingElementType)
        {
            switch (bHoMBuildingElementType) // This is just a test, it does not match. We have more BETypes in TAS than in BHoM
            {
                // here we will need to have two levels or recognision ASHRAEBuilidingElementType as per new idraw graph
                //Agreed - but we also need to implement our extended building element types in core BHoM so this might solve that?

                case BHE.PanelType.Ceiling:
                    return TBD.BuildingElementType.CEILING;
                case BHE.PanelType.UndergroundCeiling:
                    return TBD.BuildingElementType.UNDERGROUNDCEILING;
                case BHE.PanelType.Roof:
                    return TBD.BuildingElementType.ROOFELEMENT;

                case BHE.PanelType.CurtainWall:
                    return TBD.BuildingElementType.CURTAINWALL;
                case BHE.PanelType.WallInternal:
                    return TBD.BuildingElementType.INTERNALWALL;
                case BHE.PanelType.WallExternal:
                    return TBD.BuildingElementType.EXTERNALWALL;
                case BHE.PanelType.UndergroundWall:
                    return TBD.BuildingElementType.UNDERGROUNDWALL;
                case BHE.PanelType.Wall:
                    return TBD.BuildingElementType.EXTERNALWALL;

                case BHE.PanelType.FloorExposed:
                    return TBD.BuildingElementType.EXPOSEDFLOOR;
                case BHE.PanelType.FloorInternal:
                    return TBD.BuildingElementType.INTERNALFLOOR;
                case BHE.PanelType.FloorRaised:
                    return TBD.BuildingElementType.RAISEDFLOOR;
                case BHE.PanelType.SlabOnGrade:
                    return TBD.BuildingElementType.INTERNALFLOOR;
                case BHE.PanelType.Floor:
                    return TBD.BuildingElementType.INTERNALFLOOR;
                case BHE.PanelType.UndergroundSlab:
                    return TBD.BuildingElementType.UNDERGROUNDSLAB;

                case BHE.PanelType.Shade:
                    return TBD.BuildingElementType.SHADEELEMENT;
                case BHE.PanelType.SolarPanel:
                    return TBD.BuildingElementType.SOLARPANEL;

                default:
                    return TBD.BuildingElementType.NULLELEMENT;
            }
        }

        [Description("Gets a TAS TBD BuildingElementType from a BHoM Environmental OpeningType")]
        [Input("bHoMBuildingElementType", "BHoM Environmental OpeningType")]
        [Output("TAS TBD BuildingElementType")]
        public static TBD.BuildingElementType ToTAS(this BHE.OpeningType bHoMBuildingElementType)
        {
            switch (bHoMBuildingElementType) // This is just a test, it does not match. We have more BETypes in TAS than in BHoM
            {
                // here we will need to have two levels or recognision ASHRAEBuilidingElementType as per new idraw graph
                //Agreed - but we also need to implement our extended building element types in core BHoM so this might solve that?

                case BHE.OpeningType.Rooflight:
                    return TBD.BuildingElementType.ROOFLIGHT;
                case BHE.OpeningType.RooflightWithFrame:
                    return TBD.BuildingElementType.ROOFLIGHT;

                case BHE.OpeningType.CurtainWall:
                    return TBD.BuildingElementType.CURTAINWALL;

                case BHE.OpeningType.Frame:
                    return TBD.BuildingElementType.FRAMEELEMENT;
                case BHE.OpeningType.Door:
                    return TBD.BuildingElementType.DOORELEMENT;
                case BHE.OpeningType.VehicleDoor:
                    return TBD.BuildingElementType.VEHICLEDOOR;

                case BHE.OpeningType.Glazing:
                    return TBD.BuildingElementType.GLAZING;
                case BHE.OpeningType.Window:
                    return TBD.BuildingElementType.GLAZING;
                case BHE.OpeningType.WindowWithFrame:
                    return TBD.BuildingElementType.GLAZING;

                default:
                    return TBD.BuildingElementType.NULLELEMENT;
            }
        }
    }
}

