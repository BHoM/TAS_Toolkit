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

using BH.oM.Data.Requests;
using BH.oM.Adapter;
using BH.oM.Base;
using BH.oM.Adapters.TAS;
using System.IO;
using BH.Engine.Adapter;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        public override IEnumerable<object> Pull(IRequest request, PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            if (actionConfig == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return new List<object>();
            }

            TASTSDConfig config = (TASTSDConfig)actionConfig;
            if (config == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return new List<object>();
            }
            else if (!config.ValidateInput())
            {
                return new List<object>();
            }

            if (config.TSDFile == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TBDFile FileSettings object to use this adapter.");
                return new List<object>();
            }
            else if (!File.Exists(config.TSDFile.GetFullFileName()))
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid existing TBD file to read from.");
            }

            TSDDocument document = (TSDDocument)Compute.OpenTASDocument(typeof(TSDDocument), config.TSDFile);
            
            try
            {
                IEnumerable<object> objects = IRead(null, document, config);

                Compute.ICloseTASDocument(document, true);

                return objects;
            }
            catch (Exception ex)
            {
                BH.Engine.Base.Compute.RecordError($"An error occurred when reading or saving the TAS file: {ex}.");
                Compute.ICloseTASDocument(document, false);
            }

            return null;
        }
    }
}
