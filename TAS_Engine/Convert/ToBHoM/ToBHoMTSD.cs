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
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using TBD;
using TSD;
using TAS3D;
using BHEE = BH.Engine.Environment;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Elements;
using System.Collections;

using BH.oM.Environment.Results;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/

        public static BHE.Results.SimulationResult ToBHoMTSDBuilding(this TSD.BuildingData tsdBuildingData, ProfileResultUnits unitType, ProfileResultType resultType, int hour, int day)
        {
            BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
            bHoMBuildingResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingResult;

            tsdBuildingArray? buildingType = resultType.ToTASBuildingType();
            if (buildingType == null)
            {
                BH.Engine.Reflection.Compute.RecordError("That Result Type is not valid for Building results - please choose a different result type");
                return null;
            }

            object aObject = null;
            List<float> aValueList = new List<float>();
            switch (unitType)
            {
                case ProfileResultUnits.Yearly:
                    aObject = tsdBuildingData.GetAnnualBuildingResult((int)buildingType.Value);
                    aValueList = ToFloatList(aObject);
                    if (day != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for day was set but never used");
                    if (hour != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for hour was set but never used");
                    break;
                case ProfileResultUnits.Daily:
                    aObject = tsdBuildingData.GetDailyBuildingResult(day, (int)buildingType.Value);
                    aValueList = ToFloatList(aObject);
                    if (day < 1 || day > 365)
                        BH.Engine.Reflection.Compute.RecordWarning("Please set a day between 1-365");
                    if (hour != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for hour was set but never used");
                    break;
                case ProfileResultUnits.Hourly:
                    aObject = tsdBuildingData.GetHourlyBuildingResult(hour, (int)buildingType.Value);
                    aValueList.Add((float)aObject);
                    if (hour < 1 || hour > 24)
                        BH.Engine.Reflection.Compute.RecordWarning("Please set an hour between 1-24");
                    if (day != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for day was set but never used");
                    break;
                default:
                    BH.Engine.Reflection.Compute.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }

            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(resultType, unitType, aValueList.ConvertAll(x => (double)x)));

            return bHoMBuildingResult;
        }

        public static BHE.Results.SimulationResult ToBHoMTSDZone(this TSD.ZoneData tsdZoneData, ProfileResultUnits unitType, ProfileResultType resultType, int hour, int day)
        {
            BHE.Results.SimulationResult bHoMZoneResult = new BHE.Results.SimulationResult();
            bHoMZoneResult.SimulationResultType = oM.Environment.Results.SimulationResultType.SpaceResult;

            tsdZoneArray? zoneType = resultType.ToTASSpaceType();
            if(zoneType == null)
            {
                BH.Engine.Reflection.Compute.RecordError("That Result Type is not valid for Space results - please choose a different result type");
                return null;
            }

            //TODO: Should the hour inputs go between 0-23 or 1-24? The default is set to 0 and gives no errors at the moment. (Similar issue for day inputs)

            object aObject = null;
            List<float> aValueList = ToFloatList(aObject);
            switch (unitType)

            {
                case ProfileResultUnits.Yearly:
                    aObject = tsdZoneData.GetAnnualZoneResult((int)zoneType.Value);
                    aValueList = ToFloatList(aObject);
                    break;
                case ProfileResultUnits.Daily:
                    aObject = tsdZoneData.GetDailyZoneResult(day, (int)zoneType.Value);
                    aValueList = ToFloatList(aObject);
                    break;
                case ProfileResultUnits.Hourly:
                    aObject = tsdZoneData.GetHourlyZoneResult(hour, (int)zoneType.Value);
                    aValueList.Add((float)aObject);
                    break;
                default:
                    BH.Engine.Reflection.Compute.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }

            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(resultType, unitType, aValueList.ConvertAll(x => (double)x)));

            return bHoMZoneResult;
        }

        public static BHE.Results.SimulationResult ToBHoMTSDSurface(this TSD.SurfaceData tsdSurfaceData, ProfileResultUnits unitType, ProfileResultType resultType, int hour, int day)
        {
            BHE.Results.SimulationResult bHoMSurfaceResult = new BHE.Results.SimulationResult();
            bHoMSurfaceResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingElementResult;

            tsdSurfaceArray? srfType = resultType.ToTASSurfaceType();
            if (srfType == null)
            {
                BH.Engine.Reflection.Compute.RecordError("That Result Type is not valid for Building Element results - please choose a different result type");
                return null;
            }

            object aObject = null;
            List<float> aValueList = ToFloatList(aObject);
            switch (unitType)
            {
                case ProfileResultUnits.Yearly:
                    aObject = tsdSurfaceData.GetAnnualSurfaceResult((int)srfType.Value);
                    aValueList = ToFloatList(aObject);
                    if (day != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for day was set but never used");
                    if (hour != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for hour was set but never used");
                    break;
                case ProfileResultUnits.Daily:
                    aObject = tsdSurfaceData.GetDailySurfaceResult(day, (int)srfType.Value);
                    aValueList = ToFloatList(aObject);
                    if (day < 1 || day > 365)
                        BH.Engine.Reflection.Compute.RecordWarning("Please set a day between 1-365");
                    if (hour != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for hour was set but never used");
                    break;
                case ProfileResultUnits.Hourly:
                    aObject = tsdSurfaceData.GetHourlySurfaceResult(hour, (int)srfType.Value);
                    aValueList.Add((float)aObject);
                    if (hour < 1 || hour > 24)
                        BH.Engine.Reflection.Compute.RecordWarning("Please set an hour between 1-24");
                    if (day != 0)
                        BH.Engine.Reflection.Compute.RecordNote("Input for day was set but never used");
                    break;
                default:
                    BH.Engine.Reflection.Compute.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }

            bHoMSurfaceResult.SimulationResults.Add(
                Create.ProfileResult(resultType, unitType, aValueList.ConvertAll(x => (double)x)));

            return bHoMSurfaceResult;

        }
        /*
        public static BHE.Results.SimulationResult ToBHoMTSDSurface(this TSD.HeatingDesignData tsdHeatingDesignData)
        {
            BHE.Results.SimulationResult bHoMSurfaceResult = new BHE.Results.SimulationResult();
            bHoMSurfaceResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingElementResult;
            //TODO: Do this in a similar way as SurfaceData?
            object aObject = tsdHeatingDesignData.GetPeakZoneGains((int)tsdZoneArray.airMovementGain);
            List<float> aValueList = ToFloatList(aObject);

            return bHoMSurfaceResult;
        }

        public static BHE.Results.SimulationResult ToBHoMTSDSurface(this TSD.CoolingDesignData tsdCoolingDesignData)
        {
            BHE.Results.SimulationResult bHoMSurfaceResult = new BHE.Results.SimulationResult();
            bHoMSurfaceResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingElementResult;
            //TODO: Do this in a similar way as SurfaceData?
            object aObject = tsdCoolingDesignData.GetZoneData((int)tsdZoneArray.airMovementGain);
            List<float> aValueList = ToFloatList(aObject);

            return bHoMSurfaceResult;
        }*/

        /***************************************************/
    }
}                                     