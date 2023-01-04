/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.TAS;
using BH.oM.Base.Attributes;
using System.ComponentModel;
using BH.oM.Adapters.TAS.Settings;
using BH.oM.Adapters.TAS.Fragments;

using BH.Engine.Data;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets a BHoM Environmental Opening from a TAS TBD Opening Polygon")]
        [Input("tbdPolygon", "tbdPolygon that represents an opening")]
        [Output("BHoM Environmental Opening")]
        public static BHE.Opening FromTASOpening(this TBD.Polygon tbdPolygon, TBD.RoomSurface roomSurface, TASSettings tasSettings)
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
                opening.Type = ((TBD.BuildingElementType)tbdElement.BEType).FromTASOpeningType().FixBuildingElementType(tbdElement, tbdSurface);
                opening.OpeningConstruction = tbdElement.GetConstruction().FromTAS();

                //BuildingElementAnalyticalProperties
                BH.oM.Environment.Fragments.PanelAnalyticalFragment buildingElementAnalyticalProperties = new oM.Environment.Fragments.PanelAnalyticalFragment();
                buildingElementAnalyticalProperties.Altitude = ((double)tbdSurface.altitude).Round();
                buildingElementAnalyticalProperties.AltitudeRange = ((double)tbdSurface.altitudeRange).Round();
                buildingElementAnalyticalProperties.Inclination = ((double)tbdSurface.inclination).Round();
                buildingElementAnalyticalProperties.Orientation = ((double)tbdSurface.orientation).Round();
                buildingElementAnalyticalProperties.GValue = tbdElement.GValue().Round();
                buildingElementAnalyticalProperties.LTValue = tbdElement.LTValue().Round();
                buildingElementAnalyticalProperties.UValue = tbdElement.UValue().Round();
                opening.Fragments.Add(buildingElementAnalyticalProperties);

                if (tbdPolygon != null)
                    opening.Edges = tbdPolygon.FromTAS().CleanPolyline(tasSettings.AngleTolerance, tasSettings.MinimumSegmentLength).ToEdges();
                else
                    opening.Edges = roomSurface.GetPerimeter().FromTAS().CleanPolyline(tasSettings.AngleTolerance, tasSettings.MinimumSegmentLength).ToEdges();

                if (roomSurface.parentSurface != null && roomSurface.parentSurface.zoneSurface != null && roomSurface.parentSurface.zoneSurface.buildingElement != null)
                {
                    TASOpeningData tasData = new TASOpeningData();
                    tasData.ParentGUID = roomSurface.parentSurface.zoneSurface.GUID.RemoveBrackets();
                    tasData.ParentName = roomSurface.parentSurface.zoneSurface.buildingElement.name;
                    opening.Fragments.Add(tasData);
                }
            }

            return opening;
        }

        [Description("Gets a TAS TBD Polygon from a BHoM Environmental Opening")]
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

            tbdBuildingElement = openingAsElement.ToTAS(tbdBuildingElement, tbdConstruction);

            return tbdBuildingElement;
        }
    }
}




