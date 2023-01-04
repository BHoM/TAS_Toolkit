/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using TAS3D;

using BH.oM.Adapters.TAS;

using System.ComponentModel;
using BH.oM.Base.Attributes;

using System.Runtime.InteropServices;

namespace BH.Adapter.TAS
{
    public partial class TasT3DAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/
        public TasT3DAdapter()
        {
            BH.Engine.Base.Compute.RecordError("The TAS3D Adapter has not been fully implemented yet and cannot be used. For queries please contact the CODEOWNERS");
            throw new NotImplementedException();
        }

        private void RemoveUnusedZones()
        {
            List<TAS3D.Zone> zones = new List<Zone>();
            int index = 1;
            TAS3D.Zone zone = null;
            while ((zone = t3dDocument.Building.GetZone(index)) != null)
            {
                if (zone.isUsed == 0)
                    zones.Add(zone);
                index++;
            }

            zones.ForEach(x => x.Delete());
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TAS3D.T3DDocument t3dDocument = null;
        private string ProjectFolder = null;
        private string GBXMLFile = null;
        private string T3DFile = null;
        private bool FixNormals = false;

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private TAS3D.T3DDocument GetT3DDocument()
        {
            t3dDocument = new TAS3D.T3DDocument();
            if (!String.IsNullOrEmpty(ProjectFolder) && System.IO.File.Exists(ProjectFolder))
                t3dDocument.Open(T3DFile);

            else if (!String.IsNullOrEmpty(ProjectFolder))
                t3dDocument.Create();

            else
                BH.Engine.Base.Compute.RecordError("The TBD file does not exist");
            return t3dDocument;
        }

        // we close and save TBD
        private void CloseT3DDocument(bool save = true)
        {
            if (t3dDocument != null)
            {
                if (save == true)
                    t3dDocument.Save(T3DFile);

                t3dDocument.Close();

                if (t3dDocument != null)
                {
                    // issue with closing files and not closing 
                    ClearCOMObject(t3dDocument);
                    t3dDocument = null;
                }

            }

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
    }

    /***************************************************/
}





