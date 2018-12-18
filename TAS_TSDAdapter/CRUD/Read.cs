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

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            return new List<IBHoMObject>();
        }

            /*
            protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
            {
                if (type == typeof(BH.oM.Environment.Results.SimulationResult))
                    return ReadBuildingResults();
                //TODO: Impletemt option for HDD and CDD
                else if (type == typeof(BH.oM.Environment.Results.SimulationResultType))

                    return ReadSpaces();
                //if (type == typeof(BHE.Results.SimulationResult))
                //    return ReadBuildingResults();
                else
                    return null;
            }

            public List<IBHoMObject> Read()
            {
                List<IBHoMObject> bhomObjects = new List<IBHoMObject>();
                bhomObjects.AddRange(ReadBuildingResults());
                // Add other results, all shall be shown
                return bhomObjects;
            }

            /*
            public List<BH.oM.Environment.Results.SimulationResult> ReadBuildingResults(List<string> ids = null)
            {
                TSD.BuildingData tsdBuildingData = tsdDocument.SimulationData.GetBuildingData();
                TSD.CoolingDesignData tsdCoolingDesignData = tsdDocument.SimulationData.GetCoolingDesignData(0);
                TSD.HeatingDesignData tsdHeatingDesignData = tsdDocument.SimulationData.GetHeatingDesignData(0);

                List<BH.oM.Environment.Results.SimulationResult> buildingResults = new List<BH.oM.Environment.Results.SimulationResult>();
                buildingResults.Add(Engine.TAS.Convert.ToBHoMTSDBuilding(tsdBuildingData));
                List<BH.oM.Environment.Results.SimulationResult> coolingDesignResults = new List<BH.oM.Environment.Results.SimulationResult>();
                cool.Add(Engine.TAS.Convert.ToBHoMTSDZone(tsdCoolingDesignData));
                List<BH.oM.Environment.Results.SimulationResult> heatingDesignResults = new List<BH.oM.Environment.Results.SimulationResult>();
                buildingResults.Add(Engine.TAS.Convert.ToBHoMTSDBuilding(tsdHeatingDesignData));
                //TODO: Add to ourput 3 set of data CDD and HDD --DONE?

                return buildingResults;
            }*/
            /*
                //TODO: Space/zone results, ReadSpaceresults similar as ReadBuildingResults
                public List<BH.oM.Environment.Results.SimulationResult> ReadSpaceResults(List<string> ids = null)
                {
                    //iterate through all spaces 
                    //TODO: ReadSpaces
                    TSD.BuildingData tsdBuildingData = tsdDocument.SimulationData.GetBuildingData();
                    TSD.CoolingDesignData tsdCoolingDesignData = tsdDocument.SimulationData.GetCoolingDesignData(0);
                    TSD.HeatingDesignData tsdHeatingDesignData = tsdDocument.SimulationData.GetHeatingDesignData(0);

                    List<BH.oM.Environment.Results.SimulationResult> buildingResults = new List<BH.oM.Environment.Results.SimulationResult>();
                    buildingResults.Add(Engine.TAS.Convert.ToBHoMTSDZone());
                    //TODO: Add to ourput 3 set of data CDD and HDD


                    return buildingResults;
                }*/
        }
}
