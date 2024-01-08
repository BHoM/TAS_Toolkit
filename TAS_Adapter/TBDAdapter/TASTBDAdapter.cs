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
using System.Reflection;
using BH.oM.Data.Requests;
using BH.oM.Base;
using System.ComponentModel;
using BH.oM.Base.Attributes;
using BH.oM.Adapters.TAS;
using BH.oM.Adapter;
using BH.Adapter.TAS;
using BH.Engine.Adapter;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Produces an TAS Adapter to allow interopability with TAS tBD files and the BHoM")]
        [Input("fileSettings", "Input the file settings to get the file name and directory the TAS TBD Adapter should use")]
        [Input("tasSettings", "Input additional settings the adapter should use")]
        [Output("adapter", "Adapter to TAS tBD")]
        [PreviousVersion("7.0", "BH.Adapter.TAS.TasTBDAdapter(BH.oM.Adapter.FileSettings, BH.oM.TAS.Settings.TASSettings)")]
        public TasTBDAdapter()
        {
            m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateOnly;
        }
    }
}




