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
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using BH.oM.DataManipulation.Queries;
using BH.oM.Base;
using BH.Engine;

using BH.oM.Environment.Results;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        public TasTSDAdapter(string tSDFilePath = "", SimulationResultType simType = SimulationResultType.BuildingResult, ProfileResultUnits resultUnit = ProfileResultUnits.Yearly, ProfileResultType resultType = ProfileResultType.TemperatureExternal, int hour = 1, int day = 1)
        {
            //TSD application
            tsdFilePath = tSDFilePath;
            SimulationResultType = simType;
            ProfileResultUnits = resultUnit;
            ProfileResultType = resultType;
            Hour = hour;
            Day = day;
            //Add Hour and Day here?


            if(SimulationResultType == SimulationResultType.Undefined)
            {
                BH.Engine.Reflection.Compute.RecordError("Simulation type cannot be undefined");
                return;
            }
            if(ProfileResultUnits == ProfileResultUnits.Undefined)
            {
                BH.Engine.Reflection.Compute.RecordError("Unit type cannot be undefined");
                return;
            }
            if(ProfileResultType == ProfileResultType.Undefined)
            {
                BH.Engine.Reflection.Compute.RecordError("Result type cannot be undefined");
                return;
            }

            AdapterId = BH.Engine.TAS.Convert.TSDAdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            GetTsdDocument();
            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic, false);
            }

            CloseTsdDocument();
            
            return success ? objects.ToList() : new List<IObject>();
        }

        public override IEnumerable<object> Pull(IQuery query, Dictionary<string, object> config = null)
        {
            try
            {
                List<IBHoMObject> returnObjs = new List<IBHoMObject>();


                FilterQuery aFilterQuery = query as FilterQuery;
                GetTsdDocumentReadOnly (); //Open the TSD Document for pulling data from

                if (tsdDocument != null)
                {
                    switch (BH.Engine.TAS.Query.QueryType(aFilterQuery))
                    {
                        case oM.Adapters.TAS.Enums.QueryType.IsExternal:
                            break;
                        default:
                            //modified to allow filtering element we need
                            returnObjs.AddRange(Read(aFilterQuery));
                            break;
                    }

                }
                CloseTsdDocument();
                return returnObjs;

            }
            catch
            {
                CloseTsdDocument();
                return null;

            }
            finally
            {
                CloseTsdDocument();

            }

        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TSD.TSDDocument tsdDocument=null;
        private string tsdFilePath = null;
        private SimulationResultType SimulationResultType = SimulationResultType.Undefined;
        private ProfileResultUnits ProfileResultUnits = ProfileResultUnits.Undefined;
        private ProfileResultType ProfileResultType = ProfileResultType.Undefined;
        private int Hour = 1;
        private int Day = 1;

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        //To get the TSD Document
        private TSD.TSDDocument GetTsdDocument()
        {
            tsdDocument = new TSD.TSDDocument();
            if (!String.IsNullOrEmpty(tsdFilePath) && System.IO.File.Exists(tsdFilePath))
                tsdDocument.open(tsdFilePath);

            else if (!String.IsNullOrEmpty(tsdFilePath))
                tsdDocument.create(tsdFilePath); //What if an existing file has the same name?

            else
                ErrorLog.Add("The TSD file does not exist");
            return tsdDocument;
        }

        //To get the TSD Document
        private TSD.TSDDocument GetTsdDocumentReadOnly()
        {
            tsdDocument = new TSD.TSDDocument();
            if (!String.IsNullOrEmpty(tsdFilePath) && System.IO.File.Exists(tsdFilePath))
                tsdDocument.openReadOnly(tsdFilePath);

            else if (!String.IsNullOrEmpty(tsdFilePath))
                tsdDocument.create(tsdFilePath); //What if an existing file has the same name?

            else
                ErrorLog.Add("The TSD file does not exist");
            return tsdDocument;
        }

        //Close and save the TSD Document
        private void CloseTsdDocument(bool save=true)
        {
            if (tsdDocument!=null)
            {
                if (save == true)
                    tsdDocument.save();
                tsdDocument.close();
                if(tsdDocument!=null)
                {
                    ClearCOMObject(tsdDocument);
                    tsdDocument = null;
                }
            }
        }
    }
}
