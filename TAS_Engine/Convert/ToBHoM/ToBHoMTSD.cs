using System;
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
                Create.ProfileResult(ProfileResultType.AirMovementGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(10))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.BuildingHeatTransfer, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(11))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.CoolingLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(6))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.DryBulbTemperature, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(1))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.EquipmentLatentGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(19))
                );
            //EquipmentLatentGain 19 (&H13)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.EquipmentSensibleGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(15))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalConductionGlazing, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(13))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalConductionOpaque, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(12))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HeatingLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(5))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HumidityRatio, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(16))
                );
            //humidityRatio 16 (&H10)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Infiltration, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(26))
                );
            //infiltration 26 (&H1A)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.InfiltrationVentilationGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(9))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.IZAMIn, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(30))
                );
            //izamIn 30 (&H1E)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.IZAMOut, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(31))
                );
            //izamOut 31 (&H1F)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LatentAdditionLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(22))
                );
            //LatentAdditionLoad 22 (&H16)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LatentLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(20))
                );
            //LatentLoad 20 (&H14)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LatentRemovalLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(21))
                );
            //LatentRemovalLoad 21 (&H15)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LightingGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(8))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.MeanRadiantTemperature, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(2))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.OccupancyLatentGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(18))
                );
            //OccupancyLatentGain 18 (&H12)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.OccupantSensibleGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(14))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Pollutant, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(24))
                );
            //Pollutant 24 (&H18)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.RelativePressure, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(25))
                );
            //Pressure 25 (&H19)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.RelativeHumidity, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(17))
                );
            //RelativeHumidity 17 (&H11)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ResultantTemperature, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(3))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.SensibleLoad, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(4))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.SolarGain, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(7))
                );
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.VapourPressure, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(23))
                );
            //VapourPressure 23 (&H17)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.Ventilation, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(27))
                );
            //Ventilation 27 (&H1B)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowIn, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(28))
                );
            //ApertureFlowIn 28 (&H1C)?
            bHoMZoneResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowOut, ProfileResultUnits.Yearly, tsdZone.GetAnnualZoneResult(29))
                );
            //ApertureFlowOut 29 (&H1D)?

            return bHoMZoneResult;
          }
    }
}                                     