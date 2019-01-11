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
using BH.Engine.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental BuildingElement from a TAS TBD BuildingElement and TAS TBD ZoneSurface")]
        [Input("tbdElement", "TAS TBD BuildingElement")]
        [Input("tbdSurface", "TAS TBD ZoneSurface")]
        [Output("BHoM Environmental BuildingElement")]
        public static BHE.BuildingElement ToBHoM(this TBD.buildingElement tbdElement, TBD.zoneSurface tbdSurface)
        {
            BHE.BuildingElement element = new BHE.BuildingElement();

            element.Name = tbdElement.name;
            element.BuildingElementProperties.Name = tbdElement.name;
            element.BuildingElementProperties.BuildingElementType = ((TBD.BuildingElementType)tbdElement.BEType).ToBHoM().FixType(tbdElement, tbdSurface);

            //Space/Adjacent IDs...
            element.CustomData.Add("SpaceID", tbdSurface.zone.name);
            if ((int)tbdSurface.type == 3)
                element.CustomData.Add("AdjacentSpaceID", tbdSurface.linkSurface.zone.name);
            else
                element.CustomData.Add("AdjacentSpaceID", -1);

            element.BuildingElementProperties.Construction = tbdElement.GetConstruction().ToBHoM();

            List<BHG.Polyline> panelCurve = new List<BHG.Polyline>();
            int surfaceIndex = 0;
            TBD.RoomSurface roomSurface = null;

            while((roomSurface = tbdSurface.GetRoomSurface(surfaceIndex)) != null)
            {
                TBD.Perimeter tbdPerimeter = roomSurface.GetPerimeter();
                if(tbdPerimeter != null)
                {
                    panelCurve.Add(tbdPerimeter.ToBHoM());

                    //Add openings
                    int openingIndex = 0;
                    TBD.Polygon openingPolygon = null;
                    while ((openingPolygon = tbdPerimeter.GetHole(openingIndex)) != null)
                    {
                        element.Openings.Add(openingPolygon.ToBHoMOpening());
                        openingIndex++;
                    }
                }

                surfaceIndex++;
            }

            if (panelCurve.Count == 1)
                element.PanelCurve = panelCurve.First();
            else
            {
                List<BHG.Polyline> polylines = Geometry.Compute.BooleanUnion(panelCurve, 1e-3);
                if (polylines.Count == 1)
                    element.PanelCurve = polylines.First();
                else
                    element.PanelCurve = Geometry.Create.PolyCurve(polylines);
            }

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("SurfaceGUID", tbdSurface.GUID);
            tasData.Add("SurfaceName", "Z_" + tbdSurface.zone.number + "_" + tbdSurface.number + "_" + tbdSurface.zone.name);
            tasData.Add("SurfaceType", tbdSurface.type);
            tasData.Add("SurfaceAltitude", tbdSurface.altitude);
            tasData.Add("SurfaceAltitudeRange", tbdSurface.altitudeRange);
            tasData.Add("SurfaceArea", tbdSurface.area);
            tasData.Add("SurfaceInclination", tbdSurface.inclination);
            tasData.Add("SurfaceInternalArea", tbdSurface.internalArea);
            tasData.Add("SurfaceOrientation", tbdSurface.orientation);
            tasData.Add("SurfaceReversed", tbdSurface.reversed);
            tasData.Add("ElementColour", Query.GetRGB(tbdElement.colour));
            tasData.Add("ElementDescription", tbdElement.description);
            tasData.Add("ElementIsAir", tbdElement.ghost != 0);
            tasData.Add("ElementIsGround", tbdElement.ground != 0);
            tasData.Add("ElementGUID", tbdElement.GUID);
            tasData.Add("ElementWidth", tbdElement.width);
            tasData.Add("ElementGValue", tbdElement.GValue());
            tasData.Add("ElementLTValue", tbdElement.LTValue());
            tasData.Add("ElementUValue", tbdElement.UValue());
            tasData.Add("MaterialLayersThickness", tbdElement.GetConstruction().ConstructionThickness());

            return element;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD BuildingElement from a BHoM Environmental BuildingElement")]
        [Input("element", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD BuildingElement")]
        public static TBD.buildingElementClass ToTAS(this BHE.BuildingElement element)
        {
            TBD.buildingElementClass tbdElement = new TBD.buildingElementClass();
            if (element == null) return tbdElement;

            tbdElement.name = element.Name;
            tbdElement.BEType = (int)element.BuildingElementProperties.BuildingElementType.ToTAS();

            TBD.ConstructionClass construction = element.BuildingElementProperties.Construction.ToTAS();
            tbdElement.AssignConstruction(construction);

            Dictionary<string, object> tasData = null;
            if (element.CustomData.ContainsKey("TASData"))
                tasData = element.CustomData["TASData"] as Dictionary<string, object>;

            if (tasData != null)
            {
                tbdElement.colour = (tasData.ContainsKey("ElementColour") ? System.Convert.ToUInt32(tasData["ElementColour"]) : 0);
                tbdElement.description = (tasData.ContainsKey("ElementDescription") ? tasData["ElementDescription"].ToString() : "");
                tbdElement.ghost = (tasData.ContainsKey("ElementIsAir") ? (((bool)tasData["ElementIsAir"]) ? 1 : 0) : 0);
                tbdElement.ground = (tasData.ContainsKey("ElementIsGround") ? (((bool)tasData["ElementIsGround"]) ? 1 : 0) : 0);
                tbdElement.GUID = (tasData.ContainsKey("ElementGUID") ? tasData["ElementGUID"].ToString() : "");
                tbdElement.width = (tasData.ContainsKey("ElementWidth") ? (float)System.Convert.ToDouble(tasData["ElementWidth"]) : 0);
            }

            return tbdElement;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD ZoneSurface from a BHoM Environmental BuildingElement")]
        [Input("element", "BHoM Environmental BuildingElement")]
        [Output("TAS TBD ZoneSurface")]
        public static TBD.zoneSurfaceClass ToTASSurface(this BHE.BuildingElement element)
        {
            //ToDo: Finish this, connect the geometry to the zoneSurface and other additional data as appropriate

            /*TBD.zoneSurfaceClass tbdElement = new TBD.zoneSurfaceClass();
            if (element == null) return tbdElement;

            if (element.CustomData.ContainsKey("SpaceID"))
                tbdElement.zone.name = element.CustomData["SpaceID"].ToString();

            if (element.CustomData.ContainsKey("AdjacentSpaceID"))
                tbdElement.linkSurface.zone.name = element.CustomData["AdjacentSpaceID"].ToString();

            

            return tbdElement;*/

            throw new NotImplementedException("Not yet implemented");
        }
    }
}
