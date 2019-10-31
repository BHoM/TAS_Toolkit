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

using BH.oM.TAS;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public TasTBDAdapter(string tBDFilePath = "")
        {
            //TBD application
            tbdFilePath = tBDFilePath;

            AdapterId = BH.Engine.TAS.Convert.TBDAdapterID;
            Config.ProcessInMemory = false;
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            GetTbdDocument();

            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic);
            }

            CloseTbdDocument();
            return success ? objects.ToList() : new List<IObject>();
        }

        public override IEnumerable<object> Pull(IRequest request, Dictionary<string, object> config = null)
        {
            try
            {
                List<IBHoMObject> returnObjs = new List<IBHoMObject>();


                FilterRequest aFilterQuery = request as FilterRequest;
                GetTbdDocumentReadOnly(); //Open the TBD Document for pulling data from

                if (tbdDocument != null)
                {
                    switch (BH.Engine.TAS.Query.RequestType(aFilterQuery))
                    {
                        case BH.oM.TAS.RequestType.IsExternal:
                            returnObjs.AddRange(ReadExternalBuildingElements());
                            break;
                        default:
                            //modified to allow filtering element we need
                            returnObjs.AddRange(Read(aFilterQuery));
                            break;
                    }


                }

                CloseTbdDocument(false);
                return returnObjs;
            }
            catch (Exception e)
            {
                BH.Engine.Reflection.Compute.RecordError(e.ToString());
                BH.Engine.Reflection.Compute.RecordError(e.ToString());
                CloseTbdDocument(false);
                return null;
            }
            finally
            {
                CloseTbdDocument(false);
            }
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TBD.TBDDocument tbdDocument = null;
        private string tbdFilePath = null;

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


