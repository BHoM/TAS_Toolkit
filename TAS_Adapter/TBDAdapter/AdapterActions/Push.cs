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

using System.Reflection;
using BH.oM.Adapters.TAS;
using System.IO;
using System.Security.AccessControl;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        public override List<object> Push(IEnumerable<object> objects = null, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            if (actionConfig == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return new List<object>();
            }

            TASTBDConfig config = (TASTBDConfig)actionConfig;
            if (config == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return new List<object>();
            }

            if (config.TBDFile == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TBDFile FileSettings object to use this adapter.");
                return new List<object>();
            }

            TBDDocument tbdFile = new TBDDocument().OpenTASDocument(config.TBDFile);

            bool success = ICreate(objects, tbdFile, config);

            Compute.ICloseTASDocument(tbdFile, true);

            return success ? objects.ToList() : new List<object>();
        }
    }
}
