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

using BHPC = BH.oM.Physical.Properties.Construction;

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
            element.FragmentProperties.Add(environmentContextProperties);

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
            element.FragmentProperties.Add(buildingElementContextProperties);

            //BuildingElementAnalyticalProperties
            BHP.PanelAnalyticalFragment buildingElementAnalyticalProperties = new BHP.PanelAnalyticalFragment();
            buildingElementAnalyticalProperties.Altitude = tbdSurface.altitude;
            buildingElementAnalyticalProperties.AltitudeRange = tbdSurface.altitudeRange;
            buildingElementAnalyticalProperties.Inclination = tbdSurface.inclination;
            buildingElementAnalyticalProperties.Orientation = tbdSurface.orientation;
            buildingElementAnalyticalProperties.GValue = tbdElement.GValue();
            buildingElementAnalyticalProperties.LTValue = tbdElement.LTValue();
            buildingElementAnalyticalProperties.UValue = tbdElement.UValue();
            element.FragmentProperties.Add(buildingElementAnalyticalProperties);

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
                element.ExternalEdges = panelCurve.First().ToEdges();
            else
            {
                try
                {
                    List<BHG.Polyline> polylines = Geometry.Compute.BooleanUnion(panelCurve, 1e-3);
                    if (polylines.Count == 1)
                        element.ExternalEdges = polylines.First().ToEdges();
                    else
                        element.ExternalEdges = Geometry.Create.PolyCurve(polylines).ToEdges();
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordWarning("An error occurred in building buildingElement ID - " + element.BHoM_Guid + " - error was: " + e.ToString());
                    element.ExternalEdges = Geometry.Create.PolyCurve(panelCurve).ToEdges();
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

            BHE.OpeningType elementOpeningType = tbdElementType.ToBHoMOpeningType().FixType(tbdElement, tbdSurface);
            if (elementOpeningType == BHE.OpeningType.RooflightWithFrame || elementOpeningType == BHE.OpeningType.WindowWithFrame)
            {
                if(element.Openings.FirstOrDefault() != null)
                    element.Openings[0].FrameConstruction = elementConstruction;
            }

            return element;
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
            /*tbdBuildingElement.colour = BEContextProperties.Colour(); *///BH.Engine.Environment.Query.GetRGB((uint)BEContextProperties.Colour);         

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
