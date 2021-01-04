/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.TAS.Fragments;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

using BH.Engine.Base;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.Adapters.TAS.Convert ToBHoM => gets a BHoM Environmental Building from a TAS TBD Building")]
        [Input("tbdBuilding", "TAS TBD Building")]
        [Output("BHoM Environmental Building")]
        public static BHE.Building FromTAS(this TBD.Building tbdBuilding)
        {
            BHE.Building building = new BHE.Building();
            building.Name = tbdBuilding.name;
            building.Location.Latitude = tbdBuilding.latitude;
            building.Location.Longitude = tbdBuilding.longitude;
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

            //Extended Properties

            TASBuilding tasData = new TASBuilding();
            tasData.ID = tbdBuilding.GUID.RemoveBrackets();
            tasData.TASID = tbdBuilding.TBDGUID;
            tasData.PathFile = tbdBuilding.path3DFile;
            building.Fragments.Add(tasData);

            return building;
        }

        [Description("Gets a TAS TBD Building from a BHoM Environmental Building")]
        [Input("building", "BHoM Environmental Building")]
        [Output("tasTBDBuilding")]
        public static TBD.Building ToTAS(this BHE.Building building, TBD.Building tbdBuilding)
        {
            //TODO:Add BuildingHeightAdjustmentFactor, MeanHeightOfSurroundings, TerrainType, NumberOfPreconditioningDays, GroundSolarReflectance, ExternalPollutant
            //TODO:Check if Longitude, Latitude, NorthAngle, Timezone, Path3Dfile, BuildingYear is pushed

            if (building == null) 
                return tbdBuilding;

            tbdBuilding.name = building.Name;
            tbdBuilding.latitude = (float)building.Location.Latitude;
            tbdBuilding.longitude = (float)building.Location.Longitude;
            tbdBuilding.maxBuildingAltitude = (float)building.Elevation;

            TASBuilding tasFragment = building.FindFragment<TASBuilding>(typeof(TASBuilding));
            if(tasFragment != null)
            {
                tbdBuilding.GUID = tasFragment.ID;
                tbdBuilding.TBDGUID = tasFragment.TASID;
                tbdBuilding.path3DFile = tasFragment.PathFile;
            }

            BHP.OriginContextFragment environmentContextFragment = new BHP.OriginContextFragment();
            if (environmentContextFragment != null)
            {
                tbdBuilding.description = environmentContextFragment.Description;
            }

            BHP.BuildingResultFragment buildingResultsFragment= building.FindFragment<BHP.BuildingResultFragment>(typeof(BHP.BuildingResultFragment));
            if (buildingResultsFragment != null)
            {
                tbdBuilding.peakCooling = (float)System.Convert.ToDouble(buildingResultsFragment.PeakCooling);
                tbdBuilding.peakHeating = (float)System.Convert.ToDouble(buildingResultsFragment.PeakHeating);
            }

            BHP.BuildingAnalyticalFragment analyticalFragment = building.FindFragment<BHP.BuildingAnalyticalFragment>(typeof(BHP.BuildingAnalyticalFragment));
            if (analyticalFragment != null)
            {
                tbdBuilding.northAngle = (float)System.Convert.ToDouble(analyticalFragment.NorthAngle);
                tbdBuilding.timeZone = (float)System.Convert.ToDouble(analyticalFragment.GMTOffset);
            }

            return tbdBuilding;
        }
    }
}


