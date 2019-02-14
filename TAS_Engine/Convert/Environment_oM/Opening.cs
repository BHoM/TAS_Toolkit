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
            //roomSurface.parentSurface

            if (roomSurface != null)
            {
                TBD.zoneSurface tbdSurface = roomSurface.zoneSurface;
                TBD.buildingElement tbdElement = tbdSurface.buildingElement;

                //EnvironmentContextProperties
                BH.oM.Environment.Properties.EnvironmentContextProperties environmentContextProperties = new oM.Environment.Properties.EnvironmentContextProperties();
                environmentContextProperties.ElementID = tbdSurface.GUID.GetCleanGUIDFromTAS();
                environmentContextProperties.Description = tbdSurface.buildingElement.name + " - " + tbdSurface.buildingElement.GUID.GetCleanGUIDFromTAS();
                //environmentContextProperties.TypeName = tbdSurface.buildingElement.name;
                environmentContextProperties.TypeName = tbdSurface.buildingElement.name;
                opening.ExtendedProperties.Add(environmentContextProperties);

                opening.Name = environmentContextProperties.TypeName;

                //ElementProperties
                BH.oM.Environment.Properties.ElementProperties elementProperties = new oM.Environment.Properties.ElementProperties();
                elementProperties.BuildingElementType = ((TBD.BuildingElementType)tbdElement.BEType).ToBHoM().FixType(tbdElement, tbdSurface);
                elementProperties.Construction = tbdElement.GetConstruction().ToBHoM();
                opening.ExtendedProperties.Add(elementProperties);

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

                if (tbdPolygon != null)
                    opening.OpeningCurve = tbdPolygon.ToBHoM();
                else
                    opening.OpeningCurve = roomSurface.GetPerimeter().ToBHoM();

                if (roomSurface.parentSurface != null && roomSurface.parentSurface.zoneSurface != null && roomSurface.parentSurface.zoneSurface.buildingElement != null)
                {
                    opening.CustomData.Add("TAS_ParentBuildingElementGUID", roomSurface.parentSurface.zoneSurface.GUID.GetCleanGUIDFromTAS());
                    opening.CustomData.Add("TAS_ParentBuildingElementName", roomSurface.parentSurface.zoneSurface.buildingElement.name);
                }
            }

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
