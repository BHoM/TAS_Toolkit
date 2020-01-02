/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.oM.TAS.Settings;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

using BH.oM.TAS;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Produces an TAS Adapter to allow interopability with TAS tBD files and the BHoM")]
        [Input("tBDFilePath", "Path to tBD file")]
        [Input("tasSettings", "Input additional settings the adapter should use")]
        [Output("adapter", "Adapter to TAS tBD")]
        public TasTBDAdapter(string tBDFilePath = "", TASSettings tasSettings = null)
        {
            if(tasSettings == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please set some TAS Settings on the TAS Adapter");
                return;
            }
            //TBD application
            tbdFilePath = tBDFilePath;
            _tasSettings = tasSettings;

            AdapterIdName = BH.Engine.TAS.Convert.TBDAdapterID;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TBD.TBDDocument tbdDocument = null;
        private string tbdFilePath = null;
        private TASSettings _tasSettings { get; set; } = null;

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private TBD.TBDDocument GetTbdDocument()
        {
            tbdDocument = new TBD.TBDDocument();
            if (!String.IsNullOrEmpty(tbdFilePath) && System.IO.File.Exists(tbdFilePath))
                tbdDocument.open(tbdFilePath);

            else if (!String.IsNullOrEmpty(tbdFilePath))
                tbdDocument.create(tbdFilePath); //TODO: what if an existing file has the same name? 

            else
                BH.Engine.Reflection.Compute.RecordError("The TBD file does not exist");
            return tbdDocument;
        }

        private TBD.TBDDocument GetTbdDocumentReadOnly()
        {
            tbdDocument = new TBD.TBDDocument();
            if (!String.IsNullOrEmpty(tbdFilePath) && System.IO.File.Exists(tbdFilePath))
                tbdDocument.openReadOnly(tbdFilePath);

            else if (!String.IsNullOrEmpty(tbdFilePath))
                tbdDocument.create(tbdFilePath); //TODO: what if an existing file has the same name? 

            else
                BH.Engine.Reflection.Compute.RecordError("The TBD file does not exist");
            return tbdDocument;
        }

        // we close and save TBD
        private void CloseTbdDocument(bool save = true)
        {
            if (tbdDocument != null)
            {
                if (save == true)
                    tbdDocument.save();

                tbdDocument.close();

                if (tbdDocument != null)
                {
                    // issue with closing files and not closing 
                    ClearCOMObject(tbdDocument);
                    tbdDocument = null;
                }

            }

        }
    }

    /***************************************************/
}


