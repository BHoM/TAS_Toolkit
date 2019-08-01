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
using BHP = BH.oM.Environment.Fragments;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Building from a TAS T3D Building")]
        [Input("t3dBuilding", "TAS T3D Building")]
        [Output("BHoM Environmental Building")]
        public static BHE.Building ToBHoM(this TAS3D.Building t3dBuilding)
        {
            BHE.Building building = new BHE.Building();
            building.Name = t3dBuilding.name;
            building.Latitude = t3dBuilding.latitude;
            building.Longitude = t3dBuilding.longitude;
            //building.Elevation = t3dBuilding.maxBuildingAltitude;
            //building.Elevation = tbdBuilding.GetWeatherYear().altitude; //Consider switching to this if maxBuildingAltitude does not work

            //EnvironmentContextProperties
            BHP.OriginContextFragment environmentContextProperties = new BHP.OriginContextFragment();
            environmentContextProperties.ElementID = t3dBuilding.GUID.RemoveBrackets();
            environmentContextProperties.Description = t3dBuilding.description;
            environmentContextProperties.TypeName = t3dBuilding.name;
            building.Fragments.Add(environmentContextProperties);

            //BuildingAnalyticalProperties
            BHP.BuildingAnalyticalFragment buildingAnalyticalProperties = new BHP.BuildingAnalyticalFragment();
            buildingAnalyticalProperties.NorthAngle = t3dBuilding.northAngle; //North Angle (degrees) Measured clockwise with respect to the Y - axis of the building plan. 
            buildingAnalyticalProperties.Year = t3dBuilding.year;
            buildingAnalyticalProperties.GMTOffset = t3dBuilding.timeZone;
            building.Fragments.Add(buildingAnalyticalProperties);

            //BuildingContextProperties
            //TBD.WeatherYear weatherYear = t3dBuilding.GetWeatherYear();
            //if (weatherYear != null)
            //{
            //    BHP.BuildingContextFragment buildingContextProperties = new BHP.BuildingContextFragment();
            //    buildingContextProperties.PlaceName = weatherYear.name;
            //    buildingContextProperties.WeatherStation = weatherYear.description;
            //    building.Fragments.Add(buildingContextProperties);
            //}

            //BuildingResultsProperties
            //BHP.BuildingResultFragment buildingResultsProperties = new BHP.BuildingResultFragment();
            //buildingResultsProperties.PeakCooling = t3dBuilding.peakCooling;
            //buildingResultsProperties.PeakHeating = t3dBuilding.peakHeating;
            //building.Fragments.Add(buildingResultsProperties);

            //Extended Poroperties-------------------------------------------------------------------------------------------------------------------------

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("BuildingGUID", t3dBuilding.GUID.RemoveBrackets());
            tasData.Add("BuildingDescription", t3dBuilding.description);
            tasData.Add("BuildingNorthAngle", t3dBuilding.northAngle);
            //tasData.Add("BuildingPath3DFile", t3dBuilding.path3DFile);
            //tasData.Add("BuildingPeakCooling", t3dBuilding.peakCooling);
            //tasData.Add("BuildingPeakHeating", t3dBuilding.peakHeating);
            //tasData.Add("BuildingTBDGUID", t3dBuilding.TBDGUID);
            tasData.Add("BuildingTimeZone", t3dBuilding.timeZone);
            tasData.Add("BuildingYear", t3dBuilding.year);

            building.CustomData = tasData;

            return building;
        }
        
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Building from a TAS TBD Building")]
        [Input("tbdBuilding", "TAS TBD Building")]
        [Output("BHoM Environmental Building")]
        public static BHE.Building ToBHoM(this TBD.Building tbdBuilding)
        {
            BHE.Building building = new BHE.Building();
            building.Name = tbdBuilding.name;
            building.Latitude = tbdBuilding.latitude;
            building.Longitude = tbdBuilding.longitude;
            building.Elevation = tbdBuilding.maxBuildingAltitude;
            //building.Elevation = tbdBuilding.GetWeatherYear().altitude; //Consider switching to this if maxBuildingAltitude does not work

            //EnvironmentContextProperties
            BHP.OriginContextFragment environmentContextProperties = new BHP.OriginContextFragment();
            environmentContextProperties.ElementID = tbdBuilding.GUID.RemoveBrackets();
            environmentContextProperties.Description = tbdBuilding.description;
            environmentContextProperties.TypeName = tbdBuilding.name;
            building.Fragments.Add(environmentContextProperties);

            //BuildingAnalyticalProperties
            BHP.BuildingAnalyticalFragment buildingAnalyticalProperties = new BHP.BuildingAnalyticalFragment();
            buildingAnalyticalProperties.NorthAngle = tbdBuilding.northAngle; //North Angle (degrees) Measured clockwise with respect to the Y - axis of the building plan. 
            buildingAnalyticalProperties.Year = tbdBuilding.year;
            buildingAnalyticalProperties.GMTOffset = tbdBuilding.timeZone;
            building.Fragments.Add(buildingAnalyticalProperties);

            //BuildingContextProperties
            TBD.WeatherYear weatherYear = tbdBuilding.GetWeatherYear();
            if (weatherYear != null)
            {
                BHP.BuildingContextFragment buildingContextProperties = new BHP.BuildingContextFragment();
                buildingContextProperties.PlaceName = weatherYear.name;
                buildingContextProperties.WeatherStation = weatherYear.description;
                building.Fragments.Add(buildingContextProperties);
            }

            //BuildingResultsProperties
            BHP.BuildingResultFragment buildingResultsProperties = new BHP.BuildingResultFragment();
            buildingResultsProperties.PeakCooling = tbdBuilding.peakCooling;
            buildingResultsProperties.PeakHeating = tbdBuilding.peakHeating;
            building.Fragments.Add(buildingResultsProperties);

            //Extended Poroperties-------------------------------------------------------------------------------------------------------------------------

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("BuildingGUID", tbdBuilding.GUID.RemoveBrackets());
            tasData.Add("BuildingDescription", tbdBuilding.description);
            tasData.Add("BuildingNorthAngle", tbdBuilding.northAngle);
            tasData.Add("BuildingPath3DFile", tbdBuilding.path3DFile);
            tasData.Add("BuildingPeakCooling", tbdBuilding.peakCooling);
            tasData.Add("BuildingPeakHeating", tbdBuilding.peakHeating);
            tasData.Add("BuildingTBDGUID", tbdBuilding.TBDGUID);
            tasData.Add("BuildingTimeZone", tbdBuilding.timeZone);
            tasData.Add("BuildingYear", tbdBuilding.year);

            building.CustomData = tasData;

            return building;
        }
        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS T3D Building from a BHoM Environmental Building")]
        [Input("building", "BHoM Environmental Building")]
        [Output("TAS T3D Building")]
        public static TAS3D.Building ToTAS3D(this BHE.Building building, TAS3D.Building t3dBuilding)
        {
            if (building == null) return t3dBuilding;
            t3dBuilding.name = building.Name;
            t3dBuilding.latitude = building.Latitude;
            t3dBuilding.longitude = building.Longitude;

            Dictionary<string, object> tasData = building.CustomData;
            if(tasData!=null)
            {
                t3dBuilding.description = (tasData.ContainsKey("BuildingDescription") ? tasData["BuildingDescription"].ToString() : "");
                t3dBuilding.northAngle = (tasData.ContainsKey("BuildingNorthAngle") ? (float)System.Convert.ToDouble(tasData["BuildingNorthAngle"]) : 0);
                //t3dBuilding.path3DFile = (tasData.ContainsKey("BuildingPath3DFile") ? tasData["BuildingPath3DFile"].ToString() : "");
                //t3dBuilding.peakCooling = (tasData.ContainsKey("BuildingPeakCooling") ? (float)System.Convert.ToDouble(tasData["BuildingPeakCooling"]) : 0);
                //t3dBuilding.peakHeating = (tasData.ContainsKey("BuildingPeakHeating") ? (float)System.Convert.ToDouble(tasData["BuildingPeakHeating"]) : 0);
                t3dBuilding.GUID = (tasData.ContainsKey("BuildingTBDGUID") ? tasData["BuildingTBDGUID"].ToString() : "");
                t3dBuilding.timeZone = (tasData.ContainsKey("BuildingTimeZone") ? (float)System.Convert.ToDouble(tasData["BuildingTimeZone"]) : 0);
            }
            return t3dBuilding;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Building from a BHoM Environmental Building")]
        [Input("building", "BHoM Environmental Building")]
        [Output("TAS TBD Building")]
        public static TBD.Building ToTAS(this BHE.Building building, TBD.Building tbdBuilding)
        {
            //TODO:Add BuildingHeightAdjustmentFactor, MeanHeightOfSurroundings, TerrainType, NumberOfPreconditioningDays, GroundSolarReflectance, ExternalPollutant
            //TODO:Check if Longitude, Latitude, NorthAngle, Timezone, Path3Dfile, BuildingYear is pushed

            if (building == null) return tbdBuilding;
            tbdBuilding.name = building.Name;
            tbdBuilding.latitude = (float)building.Latitude;
            tbdBuilding.longitude = (float)building.Longitude;
            tbdBuilding.maxBuildingAltitude = (float)building.Elevation;

            Dictionary<string, object> tasData = building.CustomData;
            if (tasData != null)
            {
                tbdBuilding.description = (tasData.ContainsKey("BuildingDescription") ? tasData["BuildingDescription"].ToString() : "");
                tbdBuilding.northAngle = (tasData.ContainsKey("BuildingNorthAngle") ? (float)System.Convert.ToDouble(tasData["BuildingNorthAngle"]) : 0);
                tbdBuilding.path3DFile = (tasData.ContainsKey("BuildingPath3DFile") ? tasData["BuildingPath3DFile"].ToString() : "");
                tbdBuilding.peakCooling = (tasData.ContainsKey("BuildingPeakCooling") ? (float)System.Convert.ToDouble(tasData["BuildingPeakCooling"]) : 0);
                tbdBuilding.peakHeating = (tasData.ContainsKey("BuildingPeakHeating") ? (float)System.Convert.ToDouble(tasData["BuildingPeakHeating"]) : 0);
                tbdBuilding.GUID = (tasData.ContainsKey("BuildingTBDGUID") ? tasData["BuildingTBDGUID"].ToString() : "");
                tbdBuilding.timeZone = (tasData.ContainsKey("BuildingTimeZone") ? (float)System.Convert.ToDouble(tasData["BuildingTimeZone"]) : 0);
                if (tasData.ContainsKey("BuildingYear"))
                {
                    short year = System.Convert.ToInt16(tasData["BuildingYear"]);
                    tbdBuilding.year = year;
                }
            }
            return tbdBuilding;
        }
    }
}
