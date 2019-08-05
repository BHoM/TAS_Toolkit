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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.oM.Data.Requests;
using BH.oM.Base;
using TAS3D;
using BH.oM.TAS;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Adapter.TAS
{
    public partial class TasT3DAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public TasT3DAdapter(string t3DFilePath = "")
        {
            //T3D application
            t3dFilePath = t3DFilePath;

            AdapterId = BH.Engine.TAS.Convert.T3DAdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented 
        }

        public override List<IObject> Push(IEnumerable<IObject> objects = null, string tag = "", Dictionary<string, object> config = null)
        {
            GetT3DDocument();

            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic, false);
            }

            CloseT3DDocument();
            return success ? objects.ToList() : new List<IObject>();
        }

        //private void RemoveUnusedZones()
        //{
        //    List<TAS3D.Zone> zones = new List<Zone>();
        //    int index = 1;
        //    TAS3D.Zone zone = null;
        //    while((zone = t3dDocument.Building.GetZone(index)) != null)
        //    {
        //        if (zone.isUsed == 0)
        //            zones.Add(zone);
        //        index++;
        //    }

        //    zones.ForEach(x => x.Delete());
        //}

        public override IEnumerable<object> Pull(IRequest request, Dictionary<string, object> config = null)
        {
            try
            {
                List<IBHoMObject> returnObjs = new List<IBHoMObject>();

                FilterRequest aFilterQuery = request as FilterRequest;
                GetT3DDocument(); //Open the T3D Document for pulling data from
               
                if (t3dDocument != null)
                {
                    switch (BH.Engine.TAS.Query.RequestType(aFilterQuery))
                    {
                        case BH.oM.TAS.RequestType.IsExternal:
                            //returnObjs.AddRange(ReadExternalBuildingElements());
                            break;
                        default:
                            //modified to allow filtering element we need
                            returnObjs.AddRange(Read(aFilterQuery));
                            break;
                    }
                }

                CloseT3DDocument();
                return returnObjs;


            }
            catch (Exception e)
            {
                ErrorLog.Add(e.ToString());
                BH.Engine.Reflection.Compute.RecordError(e.ToString());
                CloseT3DDocument();
                return null;
            }
            finally
            {
                CloseT3DDocument();
            }
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TAS3D.T3DDocument t3dDocument = null;
        private string t3dFilePath = null;
        //private string ProjectFolder = null;
        //private string GBXMLFile = null;
        //private string T3DFile = null;
        //private string TBDFile = null;
        //private bool RunShadingCalculations = false;
        //private bool FixNormals = false;


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private TAS3D.T3DDocument GetT3DDocument()
        {
            t3dDocument = new TAS3D.T3DDocument();
            if (!String.IsNullOrEmpty(t3dFilePath) && System.IO.File.Exists(t3dFilePath))
                t3dDocument.Open(t3dFilePath);

            else if (!String.IsNullOrEmpty(t3dFilePath))
                t3dDocument.Create(); //Use Create(t3dFilePath); ?

            else
                ErrorLog.Add("The T3D file does not exist");
            return t3dDocument;
        }

        //private TAS3D.T3DDocument GetT3DDocumentReadOnly()
        //{
        //    t3dDocument = new TAS3D.T3DDocument();
        //    if (!String.IsNullOrEmpty(t3dFilePath) && System.IO.File.Exists(t3dFilePath))
        //        t3dDocument.openReadOnly(t3dFilePath);

        //    else if (!String.IsNullOrEmpty(t3dFilePath))
        //        t3dDocument.Create(); //TODO: what if an existing file has the same name? 

        //    else
        //        ErrorLog.Add("The TBD file does not exist");
        //    return t3dDocument;
        //}

        // we close and save T3D
        private void CloseT3DDocument(bool save = true)
        {
            if (t3dDocument != null)
            {
                if (save == true)
                    t3dDocument.Save(t3dFilePath); //Use Save(); ?

                t3dDocument.Close(); // Use close(t3dFilePath); ?

                if (t3dDocument != null)
                {
                    // issue with closing files and not closing 
                    ClearCOMObject(t3dDocument);
                    t3dDocument = null;
                }
            }
        }
    }
    /***************************************************/
}


