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

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental BuildingElement from a TAS T3D BuildingElement and TAS T3D ZoneSurface")]
        [Input("t3dBuildingElement", "TAS T3D BuildingElement")]
        [Input("t3dSurface", "TAS T3D ZoneSurface")]
        [Output("BHoM Environmental BuildingElement")]
        public static BHE.Panel ToBHoM(this TAS3D.Element t3dElement)
        {
            BHE.Panel element = new BHE.Panel();
            element.Name = t3dElement.name;
            
            //TAS3D.BuildingElementType tbdElementType = ((TAS3D.BuildingElementType)tbdElement.BEType);
            //Add a flag on the element for the final read
            //element.CustomData.Add("ElementIsOpening", tbdElementType.ElementIsOpening());
            
            //if (t3dElementType.ElementIsOpening())
            //{
            //    //Find out what the fix was - frame or pane?
            //    BHE.OpeningType fixedOpeningType = tbdElementType.ToBHoMOpeningType().FixType(t3dElement, t3dSurface);
            //    element.CustomData.Add("OpeningIsFrame", fixedOpeningType.OpeningIsFrame());
            //}

            //BHE.PanelType elementType = ((TAS3D.BuildingElementType)t3dElement.BEType).ToBHoM();
            //BHPC.Construction elementConstruction = t3dElement.GetConstruction().ToBHoM();
            //element.Type = t3dElement.BEType;
            //element.Construction = elementConstruction;

            //EnvironmentContextProperties
            BHP.OriginContextFragment environmentContextProperties = new BHP.OriginContextFragment();
            environmentContextProperties.ElementID = t3dElement.GUID.RemoveBrackets();
            environmentContextProperties.Description = t3dElement.description;
            //environmentContextProperties.TypeName = t3dElement.buildingElement.name;
            element.Fragments.Add(environmentContextProperties);

            //BuildingElementContextProperties
            BHP.PanelContextFragment buildingElementContextProperties = new BHP.PanelContextFragment();
            //element.ConnectedSpaces.Add(tbdSurface.zone.name);
            //if ((int)tbdSurface.type == 3)
            //    element.ConnectedSpaces.Add(tbdSurface.linkSurface.zone.name);
            //else
            //    element.ConnectedSpaces.Add("-1");
            if (buildingElementContextProperties.IsAir == true)
                t3dElement.ghost = true;
            else
                t3dElement.ghost = false;

            if (buildingElementContextProperties.IsGround == true)
                t3dElement.ground = true;
            else
                t3dElement.ground = false;
            
            buildingElementContextProperties.Colour = BH.Engine.TAS.Query.GetRGB(t3dElement.colour).ToString();
            //buildingElementContextProperties.Reversed = t3dElement.reversed != 0;
            element.Fragments.Add(buildingElementContextProperties);

            //BuildingElementAnalyticalProperties
            BHP.PanelAnalyticalFragment buildingElementAnalyticalProperties = new BHP.PanelAnalyticalFragment();
            //buildingElementAnalyticalProperties.Altitude = t3dElement.altitude.Round();
            //buildingElementAnalyticalProperties.AltitudeRange = t3dElement.altitudeRange.Round();
            //buildingElementAnalyticalProperties.Inclination = t3dElement.inclination.Round();
            //buildingElementAnalyticalProperties.Orientation = t3dElement.orientation.Round();
            //buildingElementAnalyticalProperties.GValue = t3dElement.GValue().Round();
            //buildingElementAnalyticalProperties.LTValue = t3dElement.LTValue().Round();
            //buildingElementAnalyticalProperties.UValue = t3dElement.UValue().Round();
            element.Fragments.Add(buildingElementAnalyticalProperties);

            List<BHG.Polyline> panelCurve = new List<BHG.Polyline>();
            int surfaceIndex = 0;
            //TAS3D.RoomSurface roomSurface = null;

            //while ((roomSurface = t3dElement.GetRoomSurface(surfaceIndex)) != null)
            //{
            //    TAS3D.Perimeter t3dPerimeter = roomSurface.GetPerimeter();
            //    if (t3dPerimeter != null)
            //    {
            //        panelCurve.Add(t3dPerimeter.ToBHoM());

                    //Add openings
                    int openingIndex = 0;
                    //TAS3D.Polygon openingPolygon = null;
                    //while ((openingPolygon = t3dPerimeter.GetHole(openingIndex)) != null)
                    //{
                    //    element.Openings.Add(openingPolygon.ToBHoMOpening(roomSurface));
                    //    openingIndex++;
                    //}
                //}

                surfaceIndex++;
            //}

            if (panelCurve.Count == 1)
                element.ExternalEdges = panelCurve.First().CleanPolyline().ToEdges();
            else
            {
                try
                {
                    List<BHG.Polyline> polylines = Geometry.Compute.BooleanUnion(panelCurve, 1e-3);
                    if (polylines.Count == 1)
                        element.ExternalEdges = polylines.First().CleanPolyline().ToEdges();
                    else
                        element.ExternalEdges = Geometry.Create.PolyCurve(polylines).ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).CleanPolyline().ToEdges();
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordWarning("An error occurred in building buildingElement ID - " + element.BHoM_Guid + " - error was: " + e.ToString());
                    element.ExternalEdges = Geometry.Create.PolyCurve(panelCurve).ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).CleanPolyline().ToEdges();
                }

                //element.CustomData.Add("SurfaceGUID", t3dSurface.GUID.RemoveBrackets());
                //element.CustomData.Add("SurfaceName", "Z_" + t3dSurface.zone.number + "_" + t3dSurface.number + "_" + t3dSurface.zone.name);
                //element.CustomData.Add("SurfaceType", t3dSurface.type);
                //element.CustomData.Add("SurfaceArea", t3dSurface.area.Round());
                //element.CustomData.Add("SurfaceInternalArea", t3dSurface.internalArea.Round());
                element.CustomData.Add("ElementWidth", t3dElement.width.Round());
                //element.CustomData.Add("MaterialLayersThickness", t3dElement.GetConstruction().ConstructionThickness().Round());

                element.CustomData = element.CustomData;
            }

            //AddingExtended Properties for a frame

            //BHE.OpeningType elementOpeningType = t3dElementType.ToBHoMOpeningType().FixType(t3dElement, t3dSurface);
            //if (elementOpeningType == BHE.OpeningType.RooflightWithFrame || elementOpeningType == BHE.OpeningType.WindowWithFrame)
            //{
            //    if (element.Openings.FirstOrDefault() != null)
            //        element.Openings[0].FrameConstruction = elementConstruction;
            //}

            return element;
        }

        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental BuildingElement from a TAS TBD BuildingElement and TAS TBD ZoneSurface")]
        [Input("tbdBuildingElement", "TAS TBD BuildingElement")]
        [Input("tbdSurface", "TAS TBD ZoneSurface")]
        [Output("BHoM Environmental BuildingElement")]
        public static BHE.Panel ToBHoM(this TBD.buildingElement tbdElement, TBD.zoneSurface tbdSurface)
        {
            BHE.Panel element = new BHE.Panel();

            TBD.BuildingElementType tbdElementType = ((TBD.BuildingElementType)tbdElement.BEType);
            //Add a flag on the element for the final read
            element.CustomData.Add("ElementIsOpening", tbdElementType.ElementIsOpening());

            if(tbdElementType.ElementIsOpening())
            {
                //Find out what the fix was - frame or pane?
                BHE.OpeningType fixedOpeningType = tbdElementType.ToBHoMOpeningType().FixType(tbdElement, tbdSurface);
                element.CustomData.Add("OpeningIsFrame", fixedOpeningType.OpeningIsFrame());
            }

            BHE.PanelType elementType = ((TBD.BuildingElementType)tbdElement.BEType).ToBHoM();
            BHPC.Construction elementConstruction = tbdElement.GetConstruction().ToBHoM();

            element.Name = tbdElement.name;
            element.Type = elementType;
            element.Construction = elementConstruction;
            
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
            buildingElementContextProperties.Colour = BH.Engine.TAS.Query.GetRGB(tbdElement.colour).ToString();
            buildingElementContextProperties.Reversed = tbdSurface.reversed != 0;
            element.Fragments.Add(buildingElementContextProperties);

            //BuildingElementAnalyticalProperties
            BHP.PanelAnalyticalFragment buildingElementAnalyticalProperties = new BHP.PanelAnalyticalFragment();
            buildingElementAnalyticalProperties.Altitude = tbdSurface.altitude.Round();
            buildingElementAnalyticalProperties.AltitudeRange = tbdSurface.altitudeRange.Round();
            buildingElementAnalyticalProperties.Inclination = tbdSurface.inclination.Round();
            buildingElementAnalyticalProperties.Orientation = tbdSurface.orientation.Round();
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
                    panelCurve.Add(tbdPerimeter.ToBHoM());

                    //Add openings
                    int openingIndex = 0;
                    TBD.Polygon openingPolygon = null;
                    while ((openingPolygon = tbdPerimeter.GetHole(openingIndex)) != null)
                    {
                        element.Openings.Add(openingPolygon.ToBHoMOpening(roomSurface));
                        openingIndex++;
                    }
                }

                surfaceIndex++;
            }

            if (panelCurve.Count == 1)
                element.ExternalEdges = panelCurve.First().CleanPolyline().ToEdges();
            else
            {
                try
                {
                    List<BHG.Polyline> polylines = Geometry.Compute.BooleanUnion(panelCurve, 1e-3);
                    if (polylines.Count == 1)
                        element.ExternalEdges = polylines.First().CleanPolyline().ToEdges();
                    else
                        element.ExternalEdges = Geometry.Create.PolyCurve(polylines).ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).CleanPolyline().ToEdges();
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordWarning("An error occurred in building buildingElement ID - " + element.BHoM_Guid + " - error was: " + e.ToString());
                    element.ExternalEdges = Geometry.Create.PolyCurve(panelCurve).ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).CleanPolyline().ToEdges();
                }
            }

            element.CustomData.Add("SurfaceGUID", tbdSurface.GUID.RemoveBrackets());
            element.CustomData.Add("SurfaceName", "Z_" + tbdSurface.zone.number + "_" + tbdSurface.number + "_" + tbdSurface.zone.name);
            element.CustomData.Add("SurfaceType", tbdSurface.type);
            element.CustomData.Add("SurfaceArea", tbdSurface.area.Round());
            element.CustomData.Add("SurfaceInternalArea", tbdSurface.internalArea.Round());
            element.CustomData.Add("ElementWidth", tbdElement.width.Round());
            element.CustomData.Add("MaterialLayersThickness", tbdElement.GetConstruction().ConstructionThickness().Round());

            element.CustomData = element.CustomData;

            //AddingExtended Properties for a frame

            BHE.OpeningType elementOpeningType = tbdElementType.ToBHoMOpeningType().FixType(tbdElement, tbdSurface);
            if (elementOpeningType == BHE.OpeningType.RooflightWithFrame || elementOpeningType == BHE.OpeningType.WindowWithFrame)
            {
                if(element.Openings.FirstOrDefault() != null)
                    element.Openings[0].FrameConstruction = elementConstruction;
            }

            return element;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS T3D BuildingElement from a BHoM Environmental BuildingElement")]
        [Input("buildingElement", "BHoM Environmental BuildingElement")]
        [Output("TAS T3D BuildingElement")]
        public static TAS3D.Element ToTAS3D(this BHE.Panel buildingElement, TAS3D.Element t3dBuildingElement)
        {
            if (buildingElement == null) return t3dBuildingElement;
            t3dBuildingElement.name = buildingElement.Name;
            t3dBuildingElement.BEType = (int)buildingElement.Type.ToTAS();

            BHP.OriginContextFragment envContextProperties = buildingElement.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));
            if (envContextProperties != null)
            {
                t3dBuildingElement.GUID = envContextProperties.ElementID;
                t3dBuildingElement.description = envContextProperties.Description;
            }

            BHP.PanelContextFragment panelContextProperties = buildingElement.FindFragment<BHP.PanelContextFragment>(typeof(BHP.PanelContextFragment));
            if (panelContextProperties != null)
            {
                t3dBuildingElement.ghost = panelContextProperties.IsAir;
                t3dBuildingElement.ground = panelContextProperties.IsGround;
                Dictionary<string, object> tasData = panelContextProperties.CustomData;
                if (tasData != null)
                {
                    t3dBuildingElement.colour = (tasData.ContainsKey("SpaceColour") ? System.Convert.ToUInt32(tasData["SpaceColour"]) : 0);
                }
            }

            //Add Thickness, Transparent, InternalShadows, IncludeSlopingFloorArea.
            return t3dBuildingElement;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD BuildingElement from a BHoM Environmental BuildingElement")]
        [Input("buildingElement", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD BuildingElement")]
        public static TBD.buildingElement ToTAS(this BHE.Panel buildingElement, TBD.buildingElement tbdBuildingElement, TBD.Construction tbdConstruction)
        {
            if (buildingElement == null) return tbdBuildingElement;
            tbdBuildingElement.name = buildingElement.Name;

            BHP.OriginContextFragment envContextProperties = buildingElement.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));
            if (envContextProperties != null)
                tbdBuildingElement.GUID = envContextProperties.ElementID;
                tbdBuildingElement.description = envContextProperties.Description;

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
            //tbdBuildingElement.colour = BEContextProperties.Colour(); *///BH.Engine.Environment.Query.GetRGB((uint)BEContextProperties.Colour);         

        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD ZoneSurface from a BHoM Environmental BuildingElement")]
        [Input("buildingElement", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD ZoneSurface")]
        public static TBD.zoneSurfaceClass ToTASSurface(this BHE.Panel element)
        {
            //ToDo: Finish this, connect the geometry to the zoneSurface and other additional data as appropriate

            /*TBD.zoneSurfaceClass tbdBuildingElement = new TBD.zoneSurfaceClass();
            if (buildingElement == null) return tbdBuildingElement;

            if (buildingElement.CustomData.ContainsKey("SpaceID"))
                tbdBuildingElement.zone.name = buildingElement.CustomData["SpaceID"].ToString();

            if (buildingElement.CustomData.ContainsKey("AdjacentSpaceID"))
                tbdBuildingElement.linkSurface.zone.name = buildingElement.CustomData["AdjacentSpaceID"].ToString();

            return tbdBuildingElement;*/

            throw new NotImplementedException("Not yet implemented");
        }
    }
}
