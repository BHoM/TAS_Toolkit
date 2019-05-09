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
using BH.oM.DataManipulation.Queries;
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

        public TasT3DAdapter(string gbXMLFile = "", string tbdFile = "", string t3dFile = "", bool runShadingCalculations = false, bool fixNormals = false)
        {
            //TBD application
            //ProjectFolder = projectFolder;
            GBXMLFile = gbXMLFile;
            TBDFile = tbdFile;
            T3DFile = t3dFile;
            RunShadingCalculations = runShadingCalculations;
            FixNormals = fixNormals;


            AdapterId = BH.Engine.TAS.Convert.TBDAdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            GetT3DDocument();

            bool success = true;
            /*MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic, false);
            }*/

            t3dDocument.ImportGBXML(GBXMLFile, 1, (FixNormals ? 1 : 0), 1); //Overwrite existing file (first '1') and create zones from spaces (second '1')

            CloseT3DDocument();
            return success ? objects.ToList() : new List<IObject>();
        }

        public override IEnumerable<object> Pull(IQuery query, Dictionary<string, object> config = null)
        {
            try
            {
                List<IBHoMObject> returnObjs = new List<IBHoMObject>();


                FilterQuery aFilterQuery = query as FilterQuery;
                GetT3DDocument(); //Open the TBD Document for pulling data from

                if (t3dDocument != null)
                {
                    switch (BH.Engine.TAS.Query.QueryType(aFilterQuery))
                    {
                        case BH.oM.TAS.QueryType.IsExternal:
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
        private string ProjectFolder = null;
        private string GBXMLFile = null;
        private string T3DFile = null;
        private string TBDFile = null;
        private bool RunShadingCalculations = false;
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
                ErrorLog.Add("The TBD file does not exist");
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
    }

    /***************************************************/
}


