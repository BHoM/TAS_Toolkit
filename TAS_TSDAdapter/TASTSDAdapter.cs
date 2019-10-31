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
using BH.oM.Data.Requests;
using BH.oM.Base;
using BH.Engine;
using BH.oM.Environment.Results;

using BH.oM.TAS;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        public TasTSDAdapter(string tSDFilePath = "", TSDResultType tsdResultQuery = TSDResultType.Simulation, SimulationResultType simType = SimulationResultType.BuildingResult, ProfileResultUnit resultUnit = ProfileResultUnit.Yearly, ProfileResultType resultType = ProfileResultType.TemperatureExternal, int hour = -1, int day = -1)
        {
            tsdFilePath = tSDFilePath;
            tsdResultType = tsdResultQuery;
            SimulationResultType = simType;
            ProfileResultUnits = resultUnit;
            ProfileResultType = resultType;
            Hour = hour;
            Day = day;

            if (!CheckInputCombinations()) return;

            AdapterId = BH.Engine.TAS.Convert.TSDAdapterID;
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        private bool CheckInputCombinations()
        {
            if (tsdFilePath == "")
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide a valid TSD input file path");
                return false;
            }

            if (tsdResultType == TSDResultType.Undefined)
            {
                BH.Engine.Reflection.Compute.RecordError("Result output cannot be undefined");
                return false;
            }
            if (SimulationResultType == SimulationResultType.Undefined)
            {
                BH.Engine.Reflection.Compute.RecordError("Simulation type cannot be undefined");
                return false;
            }
            if (ProfileResultUnits == ProfileResultUnit.Undefined)
            {
                BH.Engine.Reflection.Compute.RecordError("Unit type cannot be undefined");
                return false;
            }
            if (ProfileResultType == ProfileResultType.Undefined)
            {
                BH.Engine.Reflection.Compute.RecordError("Result type cannot be undefined");
                return false;
            }

            if ((tsdResultType == TSDResultType.CoolingDesignDay || tsdResultType == TSDResultType.HeatingDesignDay) && (SimulationResultType == SimulationResultType.BuildingResult || SimulationResultType == SimulationResultType.BuildingElementResult))
            {
                BH.Engine.Reflection.Compute.RecordError("Heating and Cooling Design Day results are only available on Space Result Types");
                return false;
            }

            if (ProfileResultUnits == ProfileResultUnit.Daily && (Day < 1 || Day > 365))
            {
                BH.Engine.Reflection.Compute.RecordError("Please select a day between 1 and 365 inclusive for Daily Results");
                return false;
            }

            if (ProfileResultUnits == ProfileResultUnit.Hourly && (Hour < 1 || Hour > 24))
            {
                BH.Engine.Reflection.Compute.RecordError("Please select an hour between 1 and 24 inclusive for Hourly Results");
                return false;
            }

            if (ProfileResultUnits == ProfileResultUnit.Yearly && (Hour != -1 || Day != -1))
            {
                BH.Engine.Reflection.Compute.RecordWarning("Day and Hour inputs are not used when pulling Yearly Results");
            }

            return true;
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            /*GetTsdDocument();
            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic);
            }

            CloseTsdDocument();
            
            return success ? objects.ToList() : new List<IObject>();*/

            throw new NotImplementedException("Pushing to TAS TSD files has not been implemented yet");
        }

        public override IEnumerable<object> Pull(IRequest request, Dictionary<string, object> config = null)
        {
            try
            {
                List<IBHoMObject> returnObjs = new List<IBHoMObject>();

                FilterRequest aFilterQuery = request as FilterRequest;
                GetTsdDocumentReadOnly(); //Open the TSD Document for pulling data from

                if (tsdDocument != null)
                {
                    switch (BH.Engine.TAS.Query.RequestType(aFilterQuery))
                    {
                        case BH.oM.TAS.RequestType.IsExternal:
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
            catch (Exception ex)
            {
                BH.Engine.Reflection.Compute.RecordError(ex.ToString());
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

        private TSD.TSDDocument tsdDocument = null;
        private string tsdFilePath = null;
        private TSDResultType tsdResultType = TSDResultType.Undefined;
        private SimulationResultType SimulationResultType = SimulationResultType.Undefined;
        private ProfileResultUnit ProfileResultUnits = ProfileResultUnit.Undefined;
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
                BH.Engine.Reflection.Compute.RecordError("The TSD file does not exist");
            return tsdDocument;
        }
        //TODO: Do we need both of these?
        //To get the TSD Document
        private TSD.TSDDocument GetTsdDocumentReadOnly()
        {
            tsdDocument = new TSD.TSDDocument();
            if (!String.IsNullOrEmpty(tsdFilePath) && System.IO.File.Exists(tsdFilePath))
                tsdDocument.openReadOnly(tsdFilePath);

            else if (!String.IsNullOrEmpty(tsdFilePath))
                tsdDocument.create(tsdFilePath); //What if an existing file has the same name?

            else
                BH.Engine.Reflection.Compute.RecordError("The TSD file does not exist");
            return tsdDocument;
        }

        //Close and save the TSD Document
        private void CloseTsdDocument(bool save = true)
        {
            if (tsdDocument != null)
            {
                if (save == true)
                    tsdDocument.save();
                tsdDocument.close();
                if (tsdDocument != null)
                {
                    ClearCOMObject(tsdDocument);
                    tsdDocument = null;
                }
            }
        }
    }
}
