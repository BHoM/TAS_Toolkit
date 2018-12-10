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

        public static BHE.Results.SimulationResult ToBHoMTSDBuilding(this TSD.BuildingData tsdBuildingData)
        {

            BHE.Results.SimulationResult bHoMBuildingResult = new BHE.Results.SimulationResult();
            bHoMBuildingResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingResult;
            TSD.BuildingData tsdBuilding = new TSD.BuildingData();

            bHoMBuildingResult.SimulationResults.Add(
                    Create.ProfileResult(ProfileResultType.LatentAdditionLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.additionProfile))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.CloudCover, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.cloudCover))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.CoolingLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.coolingProfile))
                );
                //CoolingLoad = CoolingProfile?
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.DiffuseRadiation, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.diffuseRadiation))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalHumidity, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.externalHumidity))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ExternalTemperature, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.externalTemperature))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.GlobalRadiation, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.globalRadiation))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.HeatingLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.heatingProfile))
                );
                //HeatingLoad = HeatingProfile?
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.LatentRemovalLoad, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.removalProfile))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.WindDirection, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.windDirection))
                );
            bHoMBuildingResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.WindSpeed, ProfileResultUnits.Yearly, tsdBuilding.GetAnnualBuildingResult((int)tsdBuildingArray.windSpeed))
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

        public static BHE.Results.SimulationResult ToBHoMTSDSurface(this TSD.SurfaceResult tsdSurfaceeResult)

        {

            BHE.Results.SimulationResult bHoMSurfaceResult = new BHE.Results.SimulationResult();
            bHoMSurfaceResult.SimulationResultType = oM.Environment.Results.SimulationResultType.BuildingElementResult;
            TSD.ZoneData tsdZone = new TSD.ZoneData();
            TSD.SurfaceData tsdSurface = new TSD.SurfaceData();

            bHoMSurfaceResult.SimulationResults.Add(
                Create.ProfileResult(ProfileResultType.ApertureFlowIn, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.apertureFlowIn))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.ApertureFlowOut, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.apertureFlowOut))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.ApertureOpening, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.apertureOpening))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.ExternalCondensation, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.extCondensation))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.ExternalConduction, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.extConduction))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.ExternalConvection, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.extConvection))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.ExternalLongWave, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.extLongWave))
                );
            //bHoMSurfaceResult.SimulationResults.Add(
            //     Create.ProfileResult(ProfileResultType.SolarGain, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.extSolarGain))
            //    );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.ExternalTemperature, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.extTemp))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.InternalCondensation, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.intCondensation))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.InternalConduction, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.intConduction))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.InternalConvection, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.intConvection))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.InterstitialCondensation, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.interCondensation))
                );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.InternalLongWave, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.intLongWave))
                );
            //bHoMSurfaceResult.SimulationResults.Add(
            //     Create.ProfileResult(ProfileResultType.Int, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.intSolarGain))
            //    );
            bHoMSurfaceResult.SimulationResults.Add(
                 Create.ProfileResult(ProfileResultType.DryBulbTemperature, ProfileResultUnits.Yearly, tsdSurface.GetAnnualSurfaceResult((int)tsdSurfaceArray.intTemp))
                );

            return bHoMSurfaceResult;
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