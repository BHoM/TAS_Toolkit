﻿/*
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
using BH.Engine.Geometry;
using BH.Engine.Environment;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Opening from a TAS TBD Opening Polygon")]
        [Input("tbdBuilding", "TAS TBD Building")]
        [Output("BHoM Environmental Opening")]
        public static BHE.Opening ToBHoMOpening(this TBD.Polygon tbdPolygon, TBD.RoomSurface roomSurface)
        {
            BHE.Opening opening = new oM.Environment.Elements.Opening();
            //roomSurface.parentSurface

            if (roomSurface != null)
            {
                TBD.zoneSurface tbdSurface = roomSurface.zoneSurface;
                TBD.buildingElement tbdElement = tbdSurface.buildingElement;

                //EnvironmentContextProperties
                BH.oM.Environment.Fragments.OriginContextFragment environmentContextProperties = new oM.Environment.Fragments.OriginContextFragment();
                environmentContextProperties.ElementID = tbdSurface.GUID.RemoveBrackets();
                environmentContextProperties.Description = tbdSurface.buildingElement.name + " - " + tbdSurface.buildingElement.GUID.RemoveBrackets();
                environmentContextProperties.TypeName = tbdSurface.buildingElement.name;
                opening.Fragments.Add(environmentContextProperties);

                opening.Name = environmentContextProperties.TypeName;
                opening.Type = ((TBD.BuildingElementType)tbdElement.BEType).ToBHoMOpeningType().FixType(tbdElement, tbdSurface);
                opening.OpeningConstruction = tbdElement.GetConstruction().ToBHoM();

                //BuildingElementAnalyticalProperties
                BH.oM.Environment.Fragments.PanelAnalyticalFragment buildingElementAnalyticalProperties = new oM.Environment.Fragments.PanelAnalyticalFragment();
                buildingElementAnalyticalProperties.Altitude = tbdSurface.altitude.Round();
                buildingElementAnalyticalProperties.AltitudeRange = tbdSurface.altitudeRange.Round();
                buildingElementAnalyticalProperties.Inclination = tbdSurface.inclination.Round();
                buildingElementAnalyticalProperties.Orientation = tbdSurface.orientation.Round();
                buildingElementAnalyticalProperties.GValue = tbdElement.GValue().Round();
                buildingElementAnalyticalProperties.LTValue = tbdElement.LTValue().Round();
                buildingElementAnalyticalProperties.UValue = tbdElement.UValue().Round();
                opening.Fragments.Add(buildingElementAnalyticalProperties);

                if (tbdPolygon != null)
                    opening.Edges = tbdPolygon.ToBHoM().CleanPolyline().ToEdges();
                else
                    opening.Edges = roomSurface.GetPerimeter().ToBHoM().CleanPolyline().ToEdges();

                if (roomSurface.parentSurface != null && roomSurface.parentSurface.zoneSurface != null && roomSurface.parentSurface.zoneSurface.buildingElement != null)
                {
                    opening.CustomData.Add("TAS_ParentBuildingElementGUID", roomSurface.parentSurface.zoneSurface.GUID.RemoveBrackets());
                    opening.CustomData.Add("TAS_ParentBuildingElementName", roomSurface.parentSurface.zoneSurface.buildingElement.name);
                }
            }

            return opening;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Polygon from a BHoM Environmental Opening")]
        [Input("opening", "BHoM Environmental Opening")]
        [Output("TAS TBD Polygon")]
        public static TBD.Polygon ToTAS(this BHE.Opening opening, TBD.Polygon tbdPolygon)
        {
            //TODO:Add properties for Opening
            tbdPolygon = opening.Polyline().ToTASPolygon(tbdPolygon);
            return tbdPolygon;
        }
         
        public static TBD.buildingElement ToTAS(this BHE.Opening opening, TBD.buildingElement tbdBuildingElement, TBD.Construction tbdConstruction)
        {
            BHE.Panel openingAsElement = new BHE.Panel();
            openingAsElement.ExternalEdges = new List<BHE.Edge>(opening.Edges);
            openingAsElement.Fragments = opening.Fragments;
            openingAsElement.BHoM_Guid = opening.BHoM_Guid;
            openingAsElement.Name = opening.Name;
            openingAsElement.CustomData = opening.CustomData;

            tbdBuildingElement = openingAsElement.ToTAS(tbdBuildingElement, tbdConstruction);

            return tbdBuildingElement;
        }
    }
}
