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
using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BHP = BH.oM.Environment.Fragments;
using BHPC = BH.oM.Physical.Constructions;
using BH.oM.Base.Attributes;
using System.ComponentModel;
using BH.Engine.Environment;
using BH.oM.Adapters.TAS;
using BH.oM.Adapters.TAS.Settings;
using BH.oM.Adapters.TAS.Fragments;
using BH.Engine.Base;

using BH.Engine.Data;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.Adapters.TAS.Convert ToBHoM => gets a BHoM Environmental BuildingElement from a TAS TBD BuildingElement and TAS TBD ZoneSurface")]
        [Input("tbdElement", "TAS TBD BuildingElement")]
        [Input("tbdSurface", "TAS TBD ZoneSurface")]
        [Output("BHoM Environmental BuildingElement")]
        public static BHE.Panel FromTAS(this TBD.buildingElement tbdElement, TBD.zoneSurface tbdSurface, TASSettings tasSettings)
        {
            BHE.Panel element = new BHE.Panel();

            TBD.BuildingElementType tbdElementType = ((TBD.BuildingElementType)tbdElement.BEType);
            TASPanelData tasData = new TASPanelData();
            tasData.PanelIsOpening = tbdElementType.ElementIsOpening();

            //Add a flag on the element for the final read
            if(tbdElementType.ElementIsOpening())
            {
                //Find out what the fix was - frame or pane?
                BHE.OpeningType fixedOpeningType = tbdElementType.FromTASOpeningType().FixBuildingElementType(tbdElement, tbdSurface);
                tasData.OpeningIsFrame = fixedOpeningType.OpeningIsFrame();
            }

            BHE.PanelType elementType = ((TBD.BuildingElementType)tbdElement.BEType).FromTAS();
            BHPC.Construction elementConstruction = tbdElement.GetConstruction().FromTAS();

            element.Name = tbdElement.name;
            element.Type = elementType;
            element.Construction = elementConstruction;

            element.Fragments.Add(tasData);

            //EnvironmentContextProperties
            BHP.OriginContextFragment environmentContextProperties = new BHP.OriginContextFragment();
            environmentContextProperties.ElementID = tbdSurface.GUID.RemoveBrackets();
            environmentContextProperties.Description =tbdElement.description;
            environmentContextProperties.TypeName = tbdSurface.buildingElement.name;
            element.Fragments.Add(environmentContextProperties);

            //BuildingElementContextProperties
            BHP.PanelContextFragment buildingElementContextProperties = new BHP.PanelContextFragment();
            element.ConnectedSpaces.Add(tbdSurface.zone.name);
            if ((int)tbdSurface.type == 3)
                element.ConnectedSpaces.Add(tbdSurface.linkSurface.zone.name);
            else
                element.ConnectedSpaces.Add("-1");

            buildingElementContextProperties.IsAir = tbdElement.ghost != 0;
            buildingElementContextProperties.IsGround = tbdElement.ground != 0;
            buildingElementContextProperties.Colour = BH.Engine.Adapters.TAS.Query.GetRGB(tbdElement.colour).ToString();
            buildingElementContextProperties.Reversed = tbdSurface.reversed != 0;
            element.Fragments.Add(buildingElementContextProperties);

            //BuildingElementAnalyticalProperties
            BHP.PanelAnalyticalFragment buildingElementAnalyticalProperties = new BHP.PanelAnalyticalFragment();
            buildingElementAnalyticalProperties.Altitude = ((double)tbdSurface.altitude).Round();
            buildingElementAnalyticalProperties.AltitudeRange = ((double)tbdSurface.altitudeRange).Round();
            buildingElementAnalyticalProperties.Inclination = ((double)tbdSurface.inclination).Round();
            buildingElementAnalyticalProperties.Orientation = ((double)tbdSurface.orientation).Round();
            buildingElementAnalyticalProperties.GValue = tbdElement.GValue().Round();
            buildingElementAnalyticalProperties.LTValue = tbdElement.LTValue().Round();
            buildingElementAnalyticalProperties.UValue = tbdElement.UValue().Round();
            element.Fragments.Add(buildingElementAnalyticalProperties);

            List<BHG.Polyline> panelCurve = new List<BHG.Polyline>();
            int surfaceIndex = 0;
            TBD.RoomSurface roomSurface = null;

            while ((roomSurface = tbdSurface.GetRoomSurface(surfaceIndex)) != null)
            {
                TBD.Perimeter tbdPerimeter = roomSurface.GetPerimeter();
                if (tbdPerimeter != null)
                {
                    panelCurve.Add(tbdPerimeter.FromTAS());

                    //Add openings
                    int openingIndex = 0;
                    TBD.Polygon openingPolygon = null;
                    while ((openingPolygon = tbdPerimeter.GetHole(openingIndex)) != null)
                    {
                        element.Openings.Add(openingPolygon.FromTASOpening(roomSurface, tasSettings));
                        openingIndex++;
                    }
                }

                surfaceIndex++;
            }

            if (panelCurve.Count == 1)
                element.ExternalEdges = panelCurve.First().CleanPolyline(tasSettings.AngleTolerance, tasSettings.MinimumSegmentLength).ToEdges();
            else
            {
                try
                {
                    List<BHG.Polyline> polylines = Geometry.Compute.BooleanUnion(panelCurve, 1e-3);
                    if (polylines.Count == 1)
                        element.ExternalEdges = polylines.First().CleanPolyline(tasSettings.AngleTolerance, tasSettings.MinimumSegmentLength).ToEdges();
                    else
                        element.ExternalEdges = Geometry.Create.PolyCurve(polylines).ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).CleanPolyline(tasSettings.AngleTolerance, tasSettings.MinimumSegmentLength).ToEdges();
                }
                catch (Exception e)
                {
                    BH.Engine.Base.Compute.RecordWarning("An error occurred in building buildingElement ID - " + element.BHoM_Guid + " - error was: " + e.ToString());
                    element.ExternalEdges = Geometry.Create.PolyCurve(panelCurve).ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).CleanPolyline(tasSettings.AngleTolerance, tasSettings.MinimumSegmentLength).ToEdges();
                }
            }

            tasData.TASID = tbdSurface.GUID.RemoveBrackets();
            tasData.TASName = "Z_" + tbdSurface.zone.number + "_" + tbdSurface.number + "_" + tbdSurface.zone.name;
            tasData.Type = System.Convert.ToString(tbdSurface.type);
            tasData.Area = ((double)tbdSurface.area).Round();
            tasData.InternalArea = ((double)tbdSurface.internalArea).Round();
            tasData.Width = ((double)tbdElement.width).Round();
            tasData.MaterialLayersThickness = tbdElement.GetConstruction().ConstructionThickness().Round();

            element.Fragments.Add(tasData);

            //AddingExtended Properties for a frame
            BHE.OpeningType elementOpeningType = tbdElementType.FromTASOpeningType().FixBuildingElementType(tbdElement, tbdSurface);
            if (elementOpeningType == BHE.OpeningType.RooflightWithFrame || elementOpeningType == BHE.OpeningType.WindowWithFrame)
            {
                if(element.Openings.FirstOrDefault() != null)
                    element.Openings[0].FrameConstruction = elementConstruction;
            }

            return element;
        }

        [Description("BH.Engine.Adapters.TAS.Convert ToTAS => gets a TAS TBD BuildingElement from a BHoM Environmental BuildingElement")]
        [Input("buildingElement", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD BuildingElement")]
        public static TBD.buildingElement ToTAS(this BHE.Panel buildingElement, TBD.buildingElement tbdBuildingElement, TBD.Construction tbdConstruction)
        {
            if (buildingElement == null) return tbdBuildingElement;
            tbdBuildingElement.name = buildingElement.Name;

            BHP.OriginContextFragment envContextProperties = buildingElement.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));
            if (envContextProperties != null)
            {
                tbdBuildingElement.GUID = envContextProperties.ElementID;
                tbdBuildingElement.description = envContextProperties.Description;
            }

            tbdBuildingElement.BEType = (int)buildingElement.Type.ToTAS();

            TBD.Construction construction = buildingElement.Construction.ToTAS(tbdConstruction);
            tbdBuildingElement.AssignConstruction(construction);
            return tbdBuildingElement;

            //TODO: Make Colour, GUID work for Pushing. Assign Construction.
            //TODO:Number of BuildingElements on the TAS Building Summary is too high
            //TODO:BuildingElements are showing up as duplicated in the TAS folder
            //TODO:What about ApertureType, FeatureShading, SubstituteElement?

            //BHP.BuildingElementContextProperties BEContextProperties = buildingElement.ContextProperties() as BHP.BuildingElementContextProperties;
            //if (BEContextProperties != null)
            /*tbdBuildingElement.colour = BEContextProperties.Colour(); *///BH.Engine.Environment.Query.GetRGB((uint)BEContextProperties.Colour);         

        }

        [Description("Gets a TAS TBD ZoneSurface from a BHoM Environmental BuildingElement")]
        [Input("element", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD ZoneSurface")]
        public static TBD.zoneSurfaceClass ToTASSurface(this BHE.Panel element)
        {
            //ToDo: Finish this, connect the geometry to the zoneSurface and other additional data as appropriate

            throw new NotImplementedException("Not yet implemented");
        }
    }
}


