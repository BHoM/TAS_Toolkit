using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BHP = BH.oM.Environment.Properties;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental BuildingElement from a TAS TBD BuildingElement and TAS TBD ZoneSurface")]
        [Input("tbdBuildingElement", "TAS TBD BuildingElement")]
        [Input("tbdSurface", "TAS TBD ZoneSurface")]
        [Output("BHoM Environmental BuildingElement")]
        public static BHE.BuildingElement ToBHoM(this TBD.buildingElement tbdElement, TBD.zoneSurface tbdSurface)
        {
            BHE.BuildingElement element = new BHE.BuildingElement();

            BHE.BuildingElementType elementType = ((TBD.BuildingElementType)tbdElement.BEType).ToBHoM().FixType(tbdElement, tbdSurface);
            BHE.Construction elementConstruction = tbdElement.GetConstruction().ToBHoM();

            element.Name = tbdElement.name;
            
            //ElementProperties
            BHP.ElementProperties elementProperties = new BHP.ElementProperties();
            elementProperties.BuildingElementType = elementType;
            elementProperties.Construction = elementConstruction;
            element.ExtendedProperties.Add(elementProperties);
            
            //EnvironmentContextProperties
            BHP.EnvironmentContextProperties environmentContextProperties = new BHP.EnvironmentContextProperties();
            environmentContextProperties.ElementID = tbdSurface.GUID.RemoveBrackets();
            environmentContextProperties.Description =tbdElement.description;
            environmentContextProperties.TypeName = tbdSurface.buildingElement.name;
            element.ExtendedProperties.Add(environmentContextProperties);

            //BuildingElementContextProperties
            BHP.BuildingElementContextProperties buildingElementContextProperties = new BHP.BuildingElementContextProperties();
            buildingElementContextProperties.ConnectedSpaces.Add(tbdSurface.zone.name);
            if ((int)tbdSurface.type == 3)
                buildingElementContextProperties.ConnectedSpaces.Add(tbdSurface.linkSurface.zone.name);
            else
                buildingElementContextProperties.ConnectedSpaces.Add("-1");

            buildingElementContextProperties.IsAir = tbdElement.ghost != 0;
            buildingElementContextProperties.IsGround = tbdElement.ground != 0;
            buildingElementContextProperties.Colour = BH.Engine.TAS.Query.GetRGB(tbdElement.colour).ToString();
            buildingElementContextProperties.Reversed = tbdSurface.reversed != 0;
            element.ExtendedProperties.Add(buildingElementContextProperties);

            //BuildingElementAnalyticalProperties
            BHP.BuildingElementAnalyticalProperties buildingElementAnalyticalProperties = new BHP.BuildingElementAnalyticalProperties();
            buildingElementAnalyticalProperties.Altitude = tbdSurface.altitude;
            buildingElementAnalyticalProperties.AltitudeRange = tbdSurface.altitudeRange;
            buildingElementAnalyticalProperties.Inclination = tbdSurface.inclination;
            buildingElementAnalyticalProperties.Orientation = tbdSurface.orientation;
            buildingElementAnalyticalProperties.GValue = tbdElement.GValue();
            buildingElementAnalyticalProperties.LTValue = tbdElement.LTValue();
            buildingElementAnalyticalProperties.UValue = tbdElement.UValue();
            element.ExtendedProperties.Add(buildingElementAnalyticalProperties);

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
                element.PanelCurve = panelCurve.First();
            else
            {
                try
                {
                    List<BHG.Polyline> polylines = Geometry.Compute.BooleanUnion(panelCurve, 1e-3);
                    if (polylines.Count == 1)
                        element.PanelCurve = polylines.First();
                    else
                        element.PanelCurve = Geometry.Create.PolyCurve(polylines);
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordWarning("An error occurred in building buildingElement ID - " + element.BHoM_Guid + " - error was: " + e.ToString());
                    element.PanelCurve = Geometry.Create.PolyCurve(panelCurve);
                }
            }

            element.CustomData.Add("SurfaceGUID", tbdSurface.GUID.RemoveBrackets());
            element.CustomData.Add("SurfaceName", "Z_" + tbdSurface.zone.number + "_" + tbdSurface.number + "_" + tbdSurface.zone.name);
            element.CustomData.Add("SurfaceType", tbdSurface.type);
            element.CustomData.Add("SurfaceArea", tbdSurface.area);
            element.CustomData.Add("SurfaceInternalArea", tbdSurface.internalArea);
            element.CustomData.Add("ElementWidth", tbdElement.width);
            element.CustomData.Add("MaterialLayersThickness", tbdElement.GetConstruction().ConstructionThickness());

            element.CustomData = element.CustomData;

            //AddingExtended Properties for a frame
            if (elementType == BHE.BuildingElementType.RooflightWithFrame || elementType == BHE.BuildingElementType.WindowWithFrame)
            {
                BHP.FrameProperties frameProperties = new BHP.FrameProperties();
                frameProperties.PaneCurve = (element.Openings.FirstOrDefault() != null ? element.Openings.FirstOrDefault().OpeningCurve : new BHG.PolyCurve());
                frameProperties.Construction = elementConstruction;
                //frameProperties.FramePercentage = null; //ToDo fix this
                element.ExtendedProperties.Add(frameProperties);
            }

            return element;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD BuildingElement from a BHoM Environmental BuildingElement")]
        [Input("buildingElement", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD BuildingElement")]
        public static TBD.buildingElement ToTAS(this BHE.BuildingElement buildingElement, TBD.buildingElement tbdBuildingElement, TBD.Construction tbdConstruction)
        {
            if (buildingElement == null) return tbdBuildingElement;
            tbdBuildingElement.name = buildingElement.Name;

            BHP.EnvironmentContextProperties envContextProperties = buildingElement.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;
            if (envContextProperties != null)
                tbdBuildingElement.GUID = envContextProperties.ElementID;
                tbdBuildingElement.description = envContextProperties.Description;

            BHP.ElementProperties elementProperties = buildingElement.ElementProperties() as BHP.ElementProperties;
            if (elementProperties != null)
                tbdBuildingElement.BEType = (int)elementProperties.BuildingElementType.ToTAS();
                TBD.Construction construction = elementProperties.Construction.ToTAS(tbdConstruction);
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

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD ZoneSurface from a BHoM Environmental BuildingElement")]
        [Input("buildingElement", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD ZoneSurface")]
        public static TBD.zoneSurfaceClass ToTASSurface(this BHE.BuildingElement element)
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
