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
        /*
        public static BHE.Results.SimulationResult ToBHoM(string Name)
        {
            BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
            {
                Name = bHoMBuildingResult.SimulationResults.name;
            };
            bHoMBuildingResult.SimulationResults.Name = bHoMBuildingResult.name;

            string buildingElementName = bHoMBuildingResult.name;
            bhomBuildingElementProperties.CustomData.Add("buildingElementName", buildingElementName);

            return bHoMBuildingResult; 
    }*/

        public static BHE.Results.SimulationResult ToBHoMTSDBuilding(this TSD.BuildingData tsdBuildingData, ProfileResultUnits unitType, ProfileResultType resultType)
        {

            BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
            bHoMBuildingResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingResult;

            tsdBuildingArray? buildingType = resultType.ToTASBuildingType();
            if (buildingType == null)
            {
                BH.Engine.Reflection.Compute.RecordError("That Result Type is not valid for Building results - please choose a different result type");
                return null;
            }

            object aObject = tsdBuildingData.GetAnnualBuildingResult((int)buildingType.Value);
            List<float> aValueList = Generic.Functions.GetList(aObject);
            //UValues = (U(tbdConstructionLayer) as List<float>).ConvertAll(x => (double)x),


            bHoMBuildingResult.SimulationResults.Add(ToBHoM(tsdBuildingData, resultType, unitType));

            return bHoMBuildingResult;
        }

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

            object aObject = null;// tsdBuildingData.GetAnnualBuildingResult((int)tsdBuildingArray.additionProfile);
            List<float> aValueList = new List<float>();
            switch (unitType)
            {
                case ProfileResultUnits.Yearly:
                    aObject = tsdBuildingData.GetAnnualBuildingResult((int)buildingType.Value);
                    aValueList = Generic.Functions.GetList(aObject);
                    break;
                case ProfileResultUnits.Daily:
                    aObject = tsdBuildingData.GetDailyBuildingResult(day, (int)buildingType.Value);
                    aValueList = Generic.Functions.GetList(aObject);
                    break;
                case ProfileResultUnits.Hourly:
                    aObject = tsdBuildingData.GetHourlyBuildingResult(hour, (int)buildingType.Value);
                    aValueList.Add((float)aObject);
                    break;
                default:
                    BH.Engine.Reflection.Compute.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }

            
            //List<float> aValueList = Generic.Functions.GetList(aObject);
            //UValues = (U(tbdConstructionLayer) as List<float>).ConvertAll(x => (double)x),


            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(resultType, unitType, aValueList.ConvertAll(x => (double)x)));

            return bHoMBuildingResult;
        }

        public static BHE.Results.SimulationResult ToBHoMTSDBuilding(this TSD.HeatingDesignData tsdHeatingDesignData)
        {
            BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
            bHoMBuildingResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingResult;

            object aObject = tsdHeatingDesignData.GetPeakZoneGains((int)tsdBuildingArray.additionProfile);
            List<float> aValueList = Generic.Functions.GetList(aObject);
            //UValues = (U(tbdConstructionLayer) as List<float>).ConvertAll(x => (double)x),


            //bHoMBuildingResult.SimulationResults.Add(ToBHoM(tsdHeatingDesignData, ProfileResultType.HumidityExternal, ProfileResultUnits.Yearly, tsdBuildingArray.externalHumidity));

            // TODO: reference to new function that will pull zones from building
            return bHoMBuildingResult;
        }

        public static BHE.Results.SimulationResult ToBHoMTSDZone(this TSD.ZoneData tsdZoneData, ProfileResultUnits unitType, ProfileResultType resultType, int hour, int day)
        {
            BHE.Results.SimulationResult bHoMZoneResult = new BHE.Results.SimulationResult();
            bHoMZoneResult.SimulationResultType = oM.Environment.Results.SimulationResultType.SpaceResult;
            //object aObject=tsdZoneData.GetAnnualZoneResult((int)tsdZoneArray.)
            hour = 0;
            day = 0;
            tsdZoneArray? zoneType = resultType.ToTASSpaceType();
            if(zoneType == null)
            {
                BH.Engine.Reflection.Compute.RecordError("That Result Type is not valid for Space results - please choose a different result type");
                return null;
            }

            //Input: Hour from 1-24, Day from 1-365
            //Error message for no hour or day input?

            object aObject = null;
            switch (unitType)
            {
                case ProfileResultUnits.Yearly:
                    aObject = tsdZoneData.GetAnnualZoneResult((int)zoneType.Value);
                    break;
                case ProfileResultUnits.Daily:
                    aObject = tsdZoneData.GetDailyZoneResult(day, (int)zoneType.Value);
                    break;
                case ProfileResultUnits.Hourly:
                    aObject = tsdZoneData.GetHourlyZoneResult(hour, (int)zoneType.Value);
                    break;
                default:
                    BH.Engine.Reflection.Compute.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }

            List<float> aValueList = Generic.Functions.GetList(aObject);

            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(resultType, unitType, aValueList.ConvertAll(x => (double)x)));
            return bHoMZoneResult;

        }

        public static BHE.Results.SimulationResult ToBHoMTSDZone(this TSD.ZoneData tsdZoneData)
      
          {
              BHE.Results.SimulationResult bHoMZoneResult = new BHE.Results.SimulationResult();
            bHoMZoneResult.SimulationResultType = oM.Environment.Results.SimulationResultType.SpaceResult;
              TSD.ZoneData tsdZone = new TSD.ZoneData();

            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainAirMovement, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.airMovementGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HeatTransferBuilding, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.buildingHeatTransfer))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LoadCooling, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.coolingLoad))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.TemperatureDryBulb, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.dryBulbTemp))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainEquipmentLatent, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.equipmentLatentGain))
                );
            //EquipmentLatentGain 19 (&H13)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainEquipmentSensible, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.equipmentSensibleGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ConductionExternalGlazing, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.externalConductionGlazing))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ConductionExternalOpaque, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.externalConductionOpaque))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LoadHeating, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.heatingLoad))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HumidityRatio, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.humidityRatio))
                );
            //humidityRatio 16 (&H10)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Infiltration, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.infiltration))
                );
            //infiltration 26 (&H1A)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainInfiltrationVentilation, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.infVentGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.IZAMIn, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.izamIn))
                );
            //izamIn 30 (&H1E)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.IZAMOut, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.izamOut))
                );
            //izamOut 31 (&H1F)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LoadLatentAddition, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.latentAdditionLoad))
                );
            //LatentAdditionLoad 22 (&H16)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LoadLatent, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.latentLoad))
                );
            //LatentLoad 20 (&H14)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LoadLatentRemoval, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.latentRemovalLoad))
                );
            //LatentRemovalLoad 21 (&H15)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainLighting, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.lightingGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.TemperatureMeanRadiant, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.MRTemp))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainOccupancyLatent, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.occupancyLatentGain))
                );
            //OccupancyLatentGain 18 (&H12)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainOccupantSensible, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.occupantSensibleGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Pollutant, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.pollutant))
                );
            //Pollutant 24 (&H18)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.PressureRelative, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.pressure))
                );
            //Pressure 25 (&H19)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HumidityRelative, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.relativeHumidity))
                );
            //RelativeHumidity 17 (&H11)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.TemperatureResultant, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.resultantTemp))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LoadSensible, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.sensibleLoad))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GainSolar, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.solarGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.PressureVapour, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.vapourPressure))
                );

            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Ventilation, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.ventilation))
                );

            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowIn, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.zoneApertureFlowIn))
                );

            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowOut, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.zoneApertureFlowOut))
                );


            return bHoMZoneResult;
          }

        public static BHE.Results.SimulationResult ToBHoMTSDSurface(this TSD.SurfaceData tsdSurfaceData, ProfileResultUnits unitType, ProfileResultType resultType)
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
            switch (unitType)
            {
                case ProfileResultUnits.Yearly:
                    aObject = tsdSurfaceData.GetAnnualSurfaceResult((int)srfType.Value);
                    break;
                case ProfileResultUnits.Daily:
                    aObject = tsdSurfaceData.GetDailySurfaceResult(1, (int)srfType.Value);
                    break;
                case ProfileResultUnits.Hourly:
                    aObject = tsdSurfaceData.GetHourlySurfaceResult(1, (int)srfType.Value);
                    break;
                default:
                    BH.Engine.Reflection.Compute.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }

            List<float> aValueList = Generic.Functions.GetList(aObject);

            bHoMSurfaceResult.SimulationResults.Add(
                Create.ProfileResult(resultType, unitType, aValueList.ConvertAll(x => (double)x)));

            return bHoMSurfaceResult;
        }

        //public static BHE.Results.SimulationResult ToBHoMTSDBuilding(this TSD.BuildingData tsdBuildingData)
        //{

        //    BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
        //    bHoMBuildingResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingResult;


        //    object aObject = tsdBuildingData.GetAnnualBuildingResult((int)tsdBuildingArray.additionProfile);
        //    List<float> aValueList = Generic.Functions.GetList(aObject);
        //    //UValues = (U(tbdConstructionLayer) as List<float>).ConvertAll(x => (double)x),

        //    bHoMBuildingResult.SimulationResults.Add(
        //            Create.ProfileResult(ProfileResultType.LoadLatentAddition, ProfileResultUnits.Yearly, aValueList.ConvertAll(x => (double)x))
        //        );
        public static ProfileResult ToBHoM(this TSD.BuildingData tsdBuildingData, ProfileResultType aProfileType, ProfileResultUnits aProfileResultUnits)
        {
            BHE.Results.ProfileResult bHoMProfileResult = new BHE.Results.ProfileResult();

            tsdBuildingArray? aBuildingResultsArray = aProfileType.ToTASBuildingType();
            if (aBuildingResultsArray == null)
            {
                BH.Engine.Reflection.Compute.RecordError("That Result Type is not valid for Building results - please choose a different result type");
                return null;
            }


            ProfileResult aProfileResult = null;
            //BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
            //bHoMBuildingResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingResult;
            object aObject = null;
            switch(aProfileResultUnits)
            {
                case ProfileResultUnits.Yearly:
                    aObject = tsdBuildingData.GetAnnualBuildingResult((int)aBuildingResultsArray.Value);
                    break;
                case ProfileResultUnits.Daily:
                    aObject = tsdBuildingData.GetDailyBuildingResult(1, (int)aBuildingResultsArray.Value);
                    break;
                case ProfileResultUnits.Hourly:
                    aObject = tsdBuildingData.GetHourlyBuildingResult(1, (int)aBuildingResultsArray.Value);
                    break;
                default:
                    BH.Engine.Reflection.Compute.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }
            //object aObject = tsdBuildingData.GetAnnualBuildingResult((int)aBuildingResultsArray);
            List<float> aValueList = Generic.Functions.GetList(aObject);
            aProfileResult = Create.ProfileResult(aProfileType, aProfileResultUnits, aValueList.ConvertAll(x => (double)x));

            /*
            BHE.Results.SimulationResult bHoMSurfaceResult = new BHE.Results.SimulationResult();
            bHoMSurfaceResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingElementResult;
            TSD.ZoneData tsdZone = new TSD.ZoneData();
            TSD.SurfaceData tsdSurface = new TSD.SurfaceData();

            bHoMSurfaceResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowIn, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.apertureFlowIn))
                );*/


            return aProfileResult;
        }

       

        /***************************************************/

        public static object AnnualBuildingResult(TSD.BuildingData tsdBuildingData, int Index)
        {

            return tsdBuildingData.GetAnnualBuildingResult(Index);
        }

        public static object GetAnnualZoneResult(TSD.ZoneData ZoneData, int Index)
        {
            return ZoneData.GetAnnualZoneResult(Index);
        }

        internal static class Functions
        {

            internal static List<float> GetAnnualZoneResultList(TSD.ZoneData ZoneData, TSD.tsdZoneArray ZoneArray)
            {
                object aObject = BH.Engine.TAS.Convert.GetAnnualZoneResult(ZoneData, (int)ZoneArray);
                return GetList(aObject);
            }

            internal static List<float> GetAnnualBuildingResultList(TSD.BuildingData tsdBuildingData, TSD.tsdBuildingArray BuildingArray)
            {
                object aObject = BH.Engine.TAS.Convert.AnnualBuildingResult(tsdBuildingData, (int)BuildingArray);
                return GetList(aObject);
            }

            internal static List<float> GetList(object Object)
            {
                List<float> aResult = new List<float>();
                if (Object != null)
                {
                    IEnumerable aValues = Object as IEnumerable;
                    if (aValues != null)
                        aResult = aValues.Cast<float>().ToList();
                }
                return aResult;
            }

            internal static void GetMaxBuildingResults(TSD.BuildingData tsdBuildingData, TSD.tsdBuildingArray TSDBuildingArray, out float Max, out int Index)
            {
                List<float> aFloatList = GetAnnualBuildingResultList(tsdBuildingData, TSDBuildingArray);
                Max = float.NaN;
                Index = -1;
                if (aFloatList.Count > 0)
                {
                    Max = aFloatList.Max();
                    Index = aFloatList.IndexOf(Max);
                }
            }

            internal static void GetMinBuildingResults(TSD.BuildingData tsdBuildingData, TSD.tsdBuildingArray TSDBuildingArray, out float Min, out int Index)
            {
                List<float> aFloatList = GetAnnualBuildingResultList(tsdBuildingData, TSDBuildingArray);
                Min = float.NaN;
                Index = -1;
                if (aFloatList.Count > 0)
                {
                    Min = aFloatList.Min();
                    Index = aFloatList.IndexOf(Min);
                }
            }

            internal static void AddValues(List<float> ListFloat, List<float> ListFloatToAdd)
            {
                for (int i = 0; i < ListFloat.Count; i++)
                    ListFloat[i] += ListFloatToAdd[i];
            }

            internal static float GetAtIndex(List<TSD.ZoneData> ZoneDataList, int Index, TSD.tsdZoneArray ZoneArray)
            {
                float aResult = 0;
                foreach (TSD.ZoneData aZoneData in ZoneDataList)
                {
                    List<float> aResutList = GetAnnualZoneResultList(aZoneData, ZoneArray);
                    if (aResutList[Index] > 0)
                        aResult += aResutList[Index];

                }
                return aResult;
            }


            internal static float GetTotalLatentGain(List<TSD.ZoneData> ZoneDataList, int Index, float Volume)
            {
                int aIndex_Temp = Index;
                if (aIndex_Temp < 2)
                    aIndex_Temp++;

                float aVal_1 = GetAtIndex(ZoneDataList, aIndex_Temp, TSD.tsdZoneArray.humidityRatio);
                float aVal_2 = GetAtIndex(ZoneDataList, aIndex_Temp - 1, TSD.tsdZoneArray.humidityRatio);
                float aVal_3 = GetAtIndex(ZoneDataList, aIndex_Temp, TSD.tsdZoneArray.latentLoad);
                return (float)(1.2 * Volume * (aVal_1 - aVal_2) / 3600 * 2257 * 1000 - aVal_3);
            }

        }


        /***************************************************/
        //results TSD enums for Building, Zone and Surface

        public enum tsdBuildingArray
        {
            externalHumidity = 1,
            externalTemperature = 2,
            globalRadiation = 3,
            diffuseRadiation = 4,
            windSpeed = 5,
            windDirection = 6,
            heatingProfile = 7,
            coolingProfile = 8,
            additionProfile = 9,
            removalProfile = 10,
            cloudCover = 11
        }

        public enum tsdZoneArray
        {
            dryBulbTemp = 1,
            MRTemp = 2,
            resultantTemp = 3,
            sensibleLoad = 4,
            heatingLoad = 5,
            coolingLoad = 6,
            solarGain = 7,
            lightingGain = 8,
            infVentGain = 9,
            airMovementGain = 10,
            buildingHeatTransfer = 11,
            externalConductionOpaque = 12,
            externalConductionGlazing = 13,
            occupantSensibleGain = 14,
            equipmentSensibleGain = 15,
            humidityRatio = 16,
            relativeHumidity = 17,
            occupancyLatentGain = 18,
            equipmentLatentGain = 19,
            latentLoad = 20,
            latentRemovalLoad = 21,
            latentAdditionLoad = 22,
            vapourPressure = 23,
            pollutant = 24,
            pressure = 25,
            infiltration = 26,
            ventilation = 27,
            zoneApertureFlowIn = 28,
            zoneApertureFlowOut = 29,
            izamIn = 30,
            izamOut = 31
        }


    }
}                                     