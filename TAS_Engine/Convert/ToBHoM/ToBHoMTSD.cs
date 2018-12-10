﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using TBD;
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

        public static BHE.Results.SimulationResult ToBHoMTSDBuilding(this TSD.BuildingData tsdBuildingData)
        {
        //externalHumidity = 1,
        //externalTemperature = 2,
        //globalRadiation = 3,
        //diffuseRadiation = 4,
        //windSpeed = 5,
        //windDirection = 6,
        //heatingProfile = 7,
        //coolingProfile = 8,
        //additionProfile = 9,
        //removalProfile = 10,
        //cloudCover = 11
            BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
            bHoMBuildingResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingResult;
            TSD.BuildingData tsdBuilding = new TSD.BuildingData();

            bHoMBuildingResult.SimulationResults.Add(
                    Create.ProfileResult(ProfileResultType.LatentAdditionLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(9))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.CloudCover, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(11))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.CoolingLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(8))
                );
                //CoolingLoad = CoolingProfile?
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.DiffuseRadiation, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(4))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalHumidity, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(1))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalTemperature, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(2))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GlobalRadiation, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(3))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HeatingLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(7))
                );
                //HeatingLoad = HeatingProfile?
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LatentRemovalLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(10))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.WindDirection, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(6))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.WindSpeed, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult(5))
                );
            
            return bHoMBuildingResult;
        }

        public static BHE.Results.SimulationResult ToBHoMTSDZone(this TSD.ZoneResult tsdZoneResult)
        
          {

              BHE.Results.SimulationResult bHoMZoneResult = new BHE.Results.SimulationResult();
            bHoMZoneResult.SimulationResultType = oM.Environment.Results.SimulationResultType.SpaceResult;
              TSD.ZoneData tsdZone = new TSD.ZoneData();

            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.AirMovementGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.airMovementGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.BuildingHeatTransfer, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.buildingHeatTransfer))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.CoolingLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.coolingLoad))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.DryBulbTemperature, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.dryBulbTemp))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.EquipmentLatentGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.equipmentLatentGain))
                );
            //EquipmentLatentGain 19 (&H13)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.EquipmentSensibleGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.equipmentSensibleGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalConductionGlazing, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.externalConductionGlazing))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalConductionOpaque, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.externalConductionOpaque))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HeatingLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.heatingLoad))
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
                Create.ProfileResult(ProfileResultType.InfiltrationVentilationGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.infVentGain))
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
                Create.ProfileResult(ProfileResultType.LatentAdditionLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.latentAdditionLoad))
                );
            //LatentAdditionLoad 22 (&H16)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LatentLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.latentLoad))
                );
            //LatentLoad 20 (&H14)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LatentRemovalLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.latentRemovalLoad))
                );
            //LatentRemovalLoad 21 (&H15)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LightingGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.lightingGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.MeanRadiantTemperature, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.MRTemp))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.OccupancyLatentGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.occupancyLatentGain))
                );
            //OccupancyLatentGain 18 (&H12)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.OccupantSensibleGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.occupantSensibleGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Pollutant, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.pollutant))
                );
            //Pollutant 24 (&H18)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.RelativePressure, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.pressure))
                );
            //Pressure 25 (&H19)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.RelativeHumidity, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.relativeHumidity))
                );
            //RelativeHumidity 17 (&H11)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ResultantTemperature, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.resultantTemp))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.SensibleLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.sensibleLoad))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.SolarGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.solarGain))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.VapourPressure, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.vapourPressure))
                );
            //VapourPressure 23 (&H17)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Ventilation, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.ventilation))
                );
            //Ventilation 27 (&H1B)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowIn, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.zoneApertureFlowIn))
                );
            //ApertureFlowIn 28 (&H1C)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowOut, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult((int)tsdZoneArray.zoneApertureFlowOut))
                );
            //ApertureFlowOut 29 (&H1D)?

            return bHoMZoneResult;
          }

        /***************************************************/
        //TAS TBD Building Element Type

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