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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;
using BH.Engine.Environment;
using TSD;
using BH.Engine.Adapters.TAS;

using BH.oM.Adapter;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        protected override bool ICreate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
        {
            bool success = true;
            if (typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                success = CreateCollection(objects as dynamic);
            }
            return success;
        }
        public static void ClearCOMObject(object Object)
        {
            if (Object == null) return;
            int intrefcount = 0;
            do
            {
                intrefcount = Marshal.FinalReleaseComObject(Object);
            } while (intrefcount > 0);
            Object = null;
        }

        private bool CreateCollection(IEnumerable<IBHoMObject> objects)
        {
            bool success = true;
            foreach (IBHoMObject obj in objects)
            {
                success &= Create(obj as dynamic);
            }
            return success;
        }
        private bool CreateCollection(IEnumerable<BHE.Elements.Space> spaces)
        {
            bool success = true;
            foreach (BHE.Elements.Space space in spaces)
            {
                //success &= Create(space, spaces);
            }
            return success;
        }

        private bool Create(BHE.Results.SimulationResult buildingResult)
        {
            TSD.BuildingData tsdBuildingData = tsdDocument.SimulationData.GetBuildingData();
            TSD.CoolingDesignData tsdCoolingDesignData = tsdDocument.SimulationData.GetCoolingDesignData(0);
            TSD.HeatingDesignData tsdHeatingDesignData = tsdDocument.SimulationData.GetHeatingDesignData(0);

            tsdBuildingData.GetHourlyBuildingResult(1, (short)tsdBuildingArray.externalTemperature);
            
            return true;
        }
        /*
        private bool Create(BHE.Properties.BuildingElementProperties buildingElementProperties)
        {
            TSD.Construction tsdConstruction = tsdDocument.Building.AddConstruction(null);
            tsdConstruction.name = buildingElementProperties.Name;
            //TODO: BuildingElementProperties can not handle Thickness
            //tsdConstruction.materialWidth[0] = (float)buildingElementProperties.Thickness; //which value in the array shall we use??

            return true;
        }

        private bool Create(BHE.Elements.Panel buildingElementPanel)
        {
            throw new NotImplementedException();
        }

        private bool Create(BHE.Elements.Opening buildingElementOpening)
        {
            throw new NotImplementedException();
        }

        private bool Create(BHE.Elements.InternalCondition internalCondition)
        {
            TSD.InternalCondition tsdInternalCondition = tsDocument.Building.AddIC(null);
            tsdInternalCondition.name = internalCondition.Name;
            return true; 
        }*/

    }
}





