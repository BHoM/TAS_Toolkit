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

            //Const additionProfile = 9
            bHoMBuildingResult.LatentAdditionProfile = tsdBuilding.GetAnnualBuildingResult(9);
            //Const cloudCover = 11
            bHoMBuildingResult.CloudCover = tsdBuilding.GetAnnualBuildingResult(11);
            //Const coolingProfile = 8
            bHoMBuildingResult.CoolingProfile = tsdBuilding.GetAnnualBuildingResult(8);
            //Const diffuseRadiation = 4
            bHoMBuildingResult.DiffuseRadiation = tsdBuilding.GetAnnualBuildingResult(4);
            //Const externalHumidity = 1
            bHoMBuildingResult.ExternalHumidity = tsdBuilding.GetAnnualBuildingResult(1);
            //Const externalTemperature = 2
            bHoMBuildingResult.ExternalTemperature =  tsdBuilding.GetAnnualBuildingResult(2);
            //Const globalRadiation = 3
            bHoMBuildingResult.GlobalRadiation = tsdBuilding.GetAnnualBuildingResult(3);
            //Const heatingProfile = 7
            bHoMBuildingResult.HeatingProfile = tsdBuilding.GetAnnualBuildingResult(7);
            //Const removalProfile = 10
            bHoMBuildingResult.LatentRemovalProfile = tsdBuilding.GetAnnualBuildingResult(10);
            //Const windDirection = 6
            bHoMBuildingResult.WindDirection = tsdBuilding.GetAnnualBuildingResult(6);
            //Const windSpeed = 5
            bHoMBuildingResult.WindSpeed = tsdBuilding.GetAnnualBuildingResult(5);
            
            return bHoMBuildingResult;
        }
        public static BHE.Results.SpaceResult ToBHoMTSDZone(this TSD.ZoneResult tsdZoneResult)
        
          {
              BHE.Results.SpaceResult bHoMZoneResult = new BHE.Results.SpaceResult();
              TSD.ZoneData tsdZone = new TSD.ZoneData();

            //Const dryBulbTemp = 1
            bHoMZoneResult.DryBulbTemperature = tsdZone.GetPeakZoneGain(1);

              return bHoMZoneResult;
          }
        
    }
}                                     