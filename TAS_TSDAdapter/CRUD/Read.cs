﻿using System;
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
            if (type == typeof(BH.oM.Environment.Results.SimulationResult))
                return ReadBuildingResults();
            //if (type == typeof(BHE.Results.SimulationResult))
            //    return ReadBuildingResults();
            else
                return null;
        }
        


        public List<BH.oM.Environment.Results.SimulationResult> ReadBuildingResults(List<string> ids = null)
        {
            TSD.BuildingData tsdBuildingData = tsdDocument.SimulationData.GetBuildingData();
            TSD.CoolingDesignData tsdCoolingDesignData = tsdDocument.SimulationData.GetCoolingDesignData(0);
            TSD.HeatingDesignData tsdHeatingDesignData = tsdDocument.SimulationData.GetHeatingDesignData(0);

            List<BH.oM.Environment.Results.SimulationResult> buildingResults = new List<BH.oM.Environment.Results.SimulationResult>();
            buildingResults.Add(Engine.TAS.Convert.ToBHoMTSDBuilding(tsdBuildingData));

            return buildingResults;
        }
    }
}
