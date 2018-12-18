using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Interface;
using BHG = BH.oM.Geometry;
using BH.Engine;
using TSD;
using BH.Engine.TAS;
using BH.oM.Environment.Results;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            return ReadResults();
        }

        private List<IBHoMObject> ReadResults()
        {
            switch(SimulationResultType)
            {
                case SimulationResultType.BuildingResult:
                    return ReadBuildingResults();
                case SimulationResultType.SpaceResult:
                    return ReadSpaceResults();
                case SimulationResultType.BuildingElementResult:
                    return ReadBuildingElementResults();
                default:
                    return new List<IBHoMObject>();
            }
        }
        
        public List<IBHoMObject> ReadBuildingResults(List<string> ids = null)
        {
            TSD.BuildingData tsdBuildingData = tsdDocument.SimulationData.GetBuildingData();
            //TSD.CoolingDesignData tsdCoolingDesignData = tsdDocument.SimulationData.GetCoolingDesignData(0);
            //TSD.HeatingDesignData tsdHeatingDesignData = tsdDocument.SimulationData.GetHeatingDesignData(0);

            List<IBHoMObject> buildingResults = new List<IBHoMObject>();
            buildingResults.Add(Engine.TAS.Convert.ToBHoMTSDBuilding(tsdBuildingData, ProfileResultUnits, ProfileResultType));

            //buildingResults.Add(Engine.TAS.Convert.ToBHoMTSDBuilding(tsdCoolingDesignData));
            
            //TODO: Add to output 3 set of data CDD and HDD --DONE?
            
            return buildingResults;
        }
        
        public List<IBHoMObject> ReadSpaceResults(List<string> ids=null)
        {
            TSD.ZoneData tsdZoneData = tsdDocument.SimulationData.GetBuildingData();
            List<IBHoMObject> spaceResults = new List<IBHoMObject>();
            spaceResults.Add(Engine.TAS.Convert.ToBHoMTSDZone(tsdZoneData,ProfileResultUnits,ProfileResultType));
            return spaceResults;
        }

        public List<IBHoMObject> ReadBuildingElementResults(List<string> ids=null)
        {
            TSD.BuildingData tsdBuildingElementData = tsdDocument.SimulationData.GetBuildingData();
            List<IBHoMObject> buildingElementResults = new List<IBHoMObject>();
            buildingElementResults.Add(Engine.TAS.Convert.ToBHoMTSDBuilding(tsdBuildingElementData, ProfileResultUnits, ProfileResultType));
            return buildingElementResults;      
        }
    }
}
