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
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Opening from a TAS TBD Opening Polygon")]
        [Input("tbdBuilding", "TAS TBD Building")]
        [Output("BHoM Environmental Opening")]
        public static BHE.Opening ToBHoMOpening(this TBD.Polygon tbdPolygon, TBD.RoomSurface roomSurface)
        {
            BHE.Opening opening = new oM.Environment.Elements.Opening();
            TBD.zoneSurface tbdSurface = roomSurface.zoneSurface;
            TBD.buildingElement tbdElement = tbdSurface.buildingElement;

            if (roomSurface != null)

            {

                //opening.CustomData.Add("TAS_ElementID", roomSurface.parentSurface.zoneSurface.GUID);
                opening.CustomData.Add("TAS_ElementID", roomSurface.zoneSurface.GUID);
                //SL will not work
                //opening.CustomData.Add("TAS_ParentBuildingElementName", roomSurface.parentSurface.zoneSurface.buildingElement.name);
                opening.CustomData.Add("TAS_BuildingElementName", roomSurface.zoneSurface.buildingElement.name);

                //Get Constructions for Opening
                TBD.buildingElement bE = roomSurface.zoneSurface.buildingElement;
                TBD.Construction Constuction = null;
                Constuction = bE.GetConstruction();
                string aName = Constuction.name;

                //Adding data to Extended Poroperties
                //BH.oM.Environment.Properties.ElementProperties elementProperties = new oM.Environment.Properties.ElementProperties();
                //elementProperties.Construction = Constuction.ToBHoM();
                //elementProperties.BuildingElementType = ((TBD.BuildingElementType)bE.BEType).ToBHoM().FixType(bE, roomSurface.zoneSurface);
                //opening.ExtendedProperties.Add(elementProperties);

                //If we have Frame we get polygon otherwise for pane we get perimeter
                try
                {

                    if (tbdPolygon != null)
                    {

                        opening.OpeningCurve = tbdPolygon.ToBHoM();

                    }
                    else
                    {
                        opening.OpeningCurve = roomSurface.GetPerimeter().ToBHoM();

                    }
                }
                catch (Exception ex)
                {
                    BH.Engine.Reflection.Compute.RecordError(ex.ToString());
                    opening.OpeningCurve = null;
                }


            }

            try
            {

                if (roomSurface.parentSurface.zoneSurface.GUID != null)
                {
                    opening.CustomData.Add("TAS_ParentBuildingElementName", roomSurface.parentSurface.zoneSurface.buildingElement.name);

                }
            }
            catch 
            {

            }


            //Adding data to Extended Poroperties--------------------------------------------------------------------------------------------------------------

            //EnvironmentContextProperties
            BH.oM.Environment.Properties.EnvironmentContextProperties environmentContextProperties = new oM.Environment.Properties.EnvironmentContextProperties();
            environmentContextProperties.ElementID = tbdSurface.GUID;
            environmentContextProperties.Description = tbdSurface.buildingElement.name + " - " + tbdSurface.buildingElement.GUID;
            environmentContextProperties.TypeName = tbdSurface.buildingElement.name;
            opening.ExtendedProperties.Add(environmentContextProperties);

            //BuildingElementContextProperties
            BH.oM.Environment.Properties.BuildingElementContextProperties buildingElementContextProperties = new oM.Environment.Properties.BuildingElementContextProperties();
            buildingElementContextProperties.ConnectedSpaces.Add(tbdSurface.zone.name);
            if ((int)tbdSurface.type == 3)
                buildingElementContextProperties.ConnectedSpaces.Add(tbdSurface.linkSurface.zone.name);
            else
                buildingElementContextProperties.ConnectedSpaces.Add("-1");

            buildingElementContextProperties.IsAir = tbdElement.ghost != 0;
            buildingElementContextProperties.IsGround = tbdElement.ground != 0;
            buildingElementContextProperties.Colour = BH.Engine.TAS.Query.GetRGB(tbdElement.colour).ToString(); //we need to fix Colot type to 'System.Drawing.Color' type currently transfrom to Int
            buildingElementContextProperties.Reversed = tbdSurface.reversed != 0;
            opening.ExtendedProperties.Add(buildingElementContextProperties);

            //BuildingElementAnalyticalProperties
            BH.oM.Environment.Properties.BuildingElementAnalyticalProperties buildingElementAnalyticalProperties = new oM.Environment.Properties.BuildingElementAnalyticalProperties();
            buildingElementAnalyticalProperties.Altitude = tbdSurface.altitude;
            buildingElementAnalyticalProperties.AltitudeRange = tbdSurface.altitudeRange;
            buildingElementAnalyticalProperties.Inclination = tbdSurface.inclination;
            buildingElementAnalyticalProperties.Orientation = tbdSurface.orientation;
            buildingElementAnalyticalProperties.GValue = tbdElement.GValue();
            buildingElementAnalyticalProperties.LTValue = tbdElement.LTValue();
            buildingElementAnalyticalProperties.UValue = tbdElement.UValue();
            opening.ExtendedProperties.Add(buildingElementAnalyticalProperties);

            //ElementProperties
            BH.oM.Environment.Properties.ElementProperties elementProperties = new oM.Environment.Properties.ElementProperties();
            elementProperties.BuildingElementType = ((TBD.BuildingElementType)tbdElement.BEType).ToBHoM().FixType(tbdElement, tbdSurface);
            elementProperties.Construction = tbdElement.GetConstruction().ToBHoM();
            opening.ExtendedProperties.Add(elementProperties);


            //Extended Poroperties-------------------------------------------------------------------------------------------------------------------------


            return opening;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Polygon from a BHoM Environmental Opening")]
        [Input("opening", "BHoM Environmental Opening")]
        [Output("TAS TBD Polygon")]
        public static TBD.PolygonClass ToTAS(this BHE.Opening opening)
        {
            TBD.PolygonClass tbdPolygon = opening.OpeningCurve.ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).ToTASPolygon();
            return tbdPolygon;
        }
    }
}
