/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;
using BH.Engine;
using TSD;
using BH.Engine.Adapters.TAS;
using BH.oM.Environment.Results;

using BH.oM.Adapter;
using TCD;
using BH.oM.Adapters.TAS;
using BH.Engine.Adapter;
using System.IO;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList ids, ActionConfig actionConfig = null)
        {
            IEnumerable<IBHoMObject> objects = new List<IBHoMObject>();
            if (actionConfig == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return objects;
            }

            TASTSDConfig config = (TASTSDConfig)actionConfig;
            if (config == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return objects;
            }
            else if (!config.ValidateInput())
            {
                return objects;
            }

            if (config.TSDFile == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TBDFile FileSettings object to use this adapter.");
                return objects;
            }
            else if (!File.Exists(config.TSDFile.GetFullFileName()))
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid existing TBD file to read from.");
            }

            TSDDocument document = new TSDDocument().OpenTASDocument(config.TSDFile);

            try
            {
                objects = Read(null, document, config);

                Compute.ICloseTASDocument(document, true);

                return objects;
            }
            catch (Exception ex)
            {
                BH.Engine.Base.Compute.RecordError($"An error occurred when reading or saving the TAS file: {ex}.");
                Compute.ICloseTASDocument(document, false);
                return objects;
            }
        }

         /***************************************************/

        protected IEnumerable<IBHoMObject> Read(Type type, TSDDocument document, TASTSDConfig actionConfig)
        {
            switch (actionConfig.SimulationType)
            {
                case SimulationResultType.BuildingResult:
                    return ReadBuildingResults(document, actionConfig);
                case SimulationResultType.SpaceResult:
                    return ReadSpaceResults(document, actionConfig);
                case SimulationResultType.BuildingElementResult:
                    return ReadBuildingElementResults(document, actionConfig);
                default:
                    return new List<IBHoMObject>();
            }
        }

        /***************************************************/

        public List<IBHoMObject> ReadBuildingResults(TSDDocument document, TASTSDConfig actionConfig)
        {
            TSD.BuildingData tsdBuildingData = document.Document.SimulationData.GetBuildingData();
            List<IBHoMObject> buildingResults = new List<IBHoMObject>
            {
                BH.Engine.Adapters.TAS.Convert.FromTAS(tsdBuildingData, actionConfig.ResultUnit, actionConfig.ResultType, actionConfig.Hour, actionConfig.Day)
            };

            return buildingResults;
        }

        /***************************************************/

        public List<IBHoMObject> ReadSpaceResults(TSDDocument document, TASTSDConfig actionConfig)
        {
            switch(actionConfig.ResultQuery)
            {
                case TSDResultType.Simulation:
                    return ReadSpaceSimulationResults(document, actionConfig);
                case TSDResultType.HeatingDesignDay:
                    return ReadSpaceHeatingResults(document, actionConfig);
                case TSDResultType.CoolingDesignDay:
                    return ReadSpaceCoolingResults(document, actionConfig);
            }

            return new List<IBHoMObject>();
        }

        /***************************************************/

        public List<IBHoMObject> ReadSpaceSimulationResults(TSDDocument document, TASTSDConfig actionConfig)
        {
            List<IBHoMObject> spaceResults = new List<IBHoMObject>();

            int zoneIndex = 1;
            TSD.ZoneData zoneData = null;

            while ((zoneData = document.Document.SimulationData.GetBuildingData().GetZoneData(zoneIndex)) != null)
            {
                spaceResults.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(zoneData, actionConfig.ResultUnit, actionConfig.ResultType, actionConfig.Hour, actionConfig.Day));
                zoneIndex++;
            }

            return spaceResults;
        }

        /***************************************************/

        public List<IBHoMObject> ReadSpaceHeatingResults(TSDDocument document, TASTSDConfig actionConfig)
        {
            List<IBHoMObject> spaceResults = new List<IBHoMObject>();

            int heatingIndex = 0;
            TSD.HeatingDesignData heatData = null;
            TSD.ZoneData zoneData = null;

            while ((heatData = document.Document.SimulationData.GetHeatingDesignData(heatingIndex)) != null)
            {
                int zoneIndex = 1;
                while ((zoneData = heatData.GetZoneData(zoneIndex)) != null)
                {
                    spaceResults.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(zoneData, actionConfig.ResultUnit, actionConfig.ResultType, actionConfig.Hour, actionConfig.Day));
                    zoneIndex++;
                }
                heatingIndex++;
            }

            return spaceResults;
        }

        /***************************************************/

        public List<IBHoMObject> ReadSpaceCoolingResults(TSDDocument document, TASTSDConfig actionConfig)
        {
            List<IBHoMObject> spaceResults = new List<IBHoMObject>();

            int coolingIndex = 0;
            TSD.CoolingDesignData coolData = null;
            TSD.ZoneData zoneData = null;

            while ((coolData = document.Document.SimulationData.GetCoolingDesignData(coolingIndex)) != null)
            {
                int zoneIndex = 1;
                while ((zoneData = coolData.GetZoneData(zoneIndex)) != null)
                {
                    spaceResults.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(zoneData, actionConfig.ResultUnit, actionConfig.ResultType, actionConfig.Hour, actionConfig.Day));
                    zoneIndex++;
                }
                coolingIndex++;
            }

            return spaceResults;
        }

        /***************************************************/

        public List<IBHoMObject> ReadBuildingElementResults(TSDDocument document, TASTSDConfig actionConfig)
        {
            List<IBHoMObject> buildingElementResults = new List<IBHoMObject>();

            int zoneIndex = 1;
            TSD.ZoneData zoneData = null;

            while ((zoneData = document.Document.SimulationData.GetBuildingData().GetZoneData(zoneIndex)) != null)
            {
                int srfIndex = 1;
                TSD.SurfaceData srfData = null;
                while((srfData = zoneData.GetSurfaceData(srfIndex)) != null)
                {
                    buildingElementResults.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(srfData, actionConfig.ResultUnit, actionConfig.ResultType, actionConfig.Hour, actionConfig.Day));
                    srfIndex++;
                }
                zoneIndex++;
            }

            return buildingElementResults;      
        }
    }
}
