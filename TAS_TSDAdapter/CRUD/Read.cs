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
            
            //TODO: Add to output 3 set of data CDD and HDD 
            
            return buildingResults;
        }
        
        public List<IBHoMObject> ReadSpaceResults(List<string> ids=null)
        {
            List<IBHoMObject> spaceResults = new List<IBHoMObject>();

            int zoneIndex = 1;
            TSD.ZoneData zoneData = null;

            while((zoneData = tsdDocument.SimulationData.GetBuildingData().GetZoneData(zoneIndex)) != null)
            {
                spaceResults.Add(Engine.TAS.Convert.ToBHoMTSDZone(zoneData, ProfileResultUnits, ProfileResultType));
                zoneIndex++;
            }

            return spaceResults;
        }

        public List<IBHoMObject> ReadBuildingElementResults(List<string> ids=null)
        {
            List<IBHoMObject> buildingElementResults = new List<IBHoMObject>();

            int zoneIndex = 0;
            TSD.ZoneData zoneData = null;

            while ((zoneData = tsdDocument.SimulationData.GetBuildingData().GetZoneData(zoneIndex)) != null)
            {
                int srfIndex = 0;
                TSD.SurfaceData srfData = null;
                while((srfData = zoneData.GetSurfaceData(srfIndex)) != null)
                {
                    buildingElementResults.Add(Engine.TAS.Convert.ToBHoMTSDSurface(srfData, ProfileResultUnits, ProfileResultType));
                    srfIndex++;
                }
                zoneIndex++;
            }

            return buildingElementResults;      
        }
    }
}
