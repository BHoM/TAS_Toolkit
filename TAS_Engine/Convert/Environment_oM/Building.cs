﻿/*
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

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
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

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("BuildingGUID", tbdBuilding.GUID);
            tasData.Add("BuildingDescription", tbdBuilding.description);
            tasData.Add("BuildingNorthAngle", tbdBuilding.northAngle);
            tasData.Add("BuildingPath3DFile", tbdBuilding.path3DFile);
            tasData.Add("BuildingPeakCooling", tbdBuilding.peakCooling);
            tasData.Add("BuildingPeakHeating", tbdBuilding.peakHeating);
            tasData.Add("BuildingTBDGUID", tbdBuilding.TBDGUID);
            tasData.Add("BuildingTimeZone", tbdBuilding.timeZone);
            tasData.Add("BuildingYear", tbdBuilding.year);

            building.CustomData.Add("TASData", tasData);

            return building;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Building from a BHoM Environmental Building")]
        [Input("building", "BHoM Environmental Building")]
        [Output("TAS TBD Building")]
        public static TBD.BuildingClass ToTAS(this BHE.Building building)
        {
            TBD.BuildingClass tbdBuilding = new TBD.BuildingClass();

            tbdBuilding.name = building.Name;
            tbdBuilding.latitude = (float)building.Latitude;
            tbdBuilding.longitude = (float)building.Longitude;
            tbdBuilding.maxBuildingAltitude = (float)building.Elevation;

            Dictionary<string, object> tasData = null;
            if (building.CustomData.ContainsKey("TASData"))
                tasData = building.CustomData["TASData"] as Dictionary<string, object>;

            if (tasData != null)
            {
                tbdBuilding.GUID = (tasData.ContainsKey("BuildingGUID") ? tasData["BuildingGUID"].ToString() : "");
                tbdBuilding.description = (tasData.ContainsKey("BuildingDescription") ? tasData["BuildingDescription"].ToString() : "");
                tbdBuilding.northAngle = (tasData.ContainsKey("BuildingNorthAngle") ? (float)System.Convert.ToDouble(tasData["BuildingNorthAngle"]) : 0);
                tbdBuilding.path3DFile = (tasData.ContainsKey("BuildingPath3DFile") ? tasData["BuildingPath3DFile"].ToString() : "");
                tbdBuilding.peakCooling = (tasData.ContainsKey("BuildingPeakCooling") ? (float)System.Convert.ToDouble(tasData["BuildingPeakCooling"]) : 0);
                tbdBuilding.peakHeating = (tasData.ContainsKey("BuildingPeakHeating") ? (float)System.Convert.ToDouble(tasData["BuildingPeakHeating"]) : 0);
                tbdBuilding.TBDGUID = (tasData.ContainsKey("BuildingTBDGUID") ? tasData["BuildingTBDGUID"].ToString() : "");
                tbdBuilding.timeZone = (tasData.ContainsKey("BuildingTimeZone") ? (float)System.Convert.ToDouble(tasData["BuildingTimeZone"]) : 0);

                if(tasData.ContainsKey("BuildingYear"))
                {
                    short year = System.Convert.ToInt16(tasData["BuildingYear"]);
                    tbdBuilding.year = year;
                }
            }

            return tbdBuilding;
        }
    }
}