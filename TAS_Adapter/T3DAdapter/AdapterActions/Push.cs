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
using BH.Engine.Adapter;

namespace BH.Adapter.TAS
{
    public partial class TasT3DAdapter : BHoMAdapter
    {
        public override List<object> Push(IEnumerable<object> objects = null, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            if (actionConfig == null)
            {
                BH.Engine.Base.Compute.RecordError("To use this adapter, you must provide a valid TAST3DConfig");
                return new List<object>();
            }

            TAST3DConfig config = (TAST3DConfig)actionConfig;
            if (config == null)
            {
                BH.Engine.Base.Compute.RecordError("To use this adapter, you must provide a valid TAST3DConfig");
                return new List<object>();
            }

            // If unset, set the pushType to AdapterSettings' value (base AdapterSettings default is FullCRUD).
            if (pushType == PushType.AdapterDefault)
                pushType = m_AdapterSettings.DefaultPushType;

            IEnumerable<IBHoMObject> objectsToPush = ProcessObjectsForPush(objects, actionConfig); // Note: default Push only supports IBHoMObjects.

            T3DDocument document = (T3DDocument)Compute.OpenTASDocument(typeof(T3DDocument), config.T3DFile);

            bool success = true;

            document.Document.ImportGBXML(config.GBXMLFile.GetFullFileName(), 1, (FixNormals ? 1 : 0), 1); //Overwrite existing file (first '1') and create zones from spaces (second '1')
            Compute.RemoveUnusedZones(document);

            Compute.ICloseTASDocument(document, true);

            return success ? objects.ToList() : new List<object>();
        }
    }
}




