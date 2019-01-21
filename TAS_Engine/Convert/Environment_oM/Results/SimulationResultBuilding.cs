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

using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHR = BH.oM.Environment.Results;
using BHG = BH.oM.Geometry;
using BHER = BH.Engine.Reflection.Compute;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Simulation Result from a TAS TSD Building Simulation")]
        [Input("tsdData", "TAS TSD Building Data")]
        [Output("BHoM Environmental Simulation Result")]
        public static BHR.SimulationResult ToBHoM(this TSD.BuildingData tsdData, BHR.ProfileResultUnits unitType, BHR.ProfileResultType resultType, int hour, int day)
        {
            TSD.tsdBuildingArray? buildingType = resultType.ToTASBuildingType();
            if(buildingType == null)
            {
                BHER.RecordError("That Result Type is not valid for Building results - please choose a different result type");
                return null;
            }

            List<double> results = new List<double>();
            switch(unitType)
            {
                case BHR.ProfileResultUnits.Yearly:
                    object yearRes = tsdData.GetAnnualBuildingResult((int)buildingType.Value);
                    results = ToDoubleList(yearRes);
                    break;
                case BHR.ProfileResultUnits.Daily:
                    if (day < 1 || day > 365)
                    {
                        BHER.RecordError("Please set a day between 1 and 365 inclusive");
                        return null;
                    }
                    object dayRes = tsdData.GetDailyBuildingResult(day, (int)buildingType.Value);
                    results = ToDoubleList(dayRes);
                    break;
                case BHR.ProfileResultUnits.Hourly:
                    if(hour < 1 || hour > 24)
                    {
                        BHER.RecordError("Please set an hour between 1 and 24 inclusive");
                        return null;
                    }
                    results.Add(tsdData.GetHourlyBuildingResult(hour, (int)buildingType.Value));
                    break;
                default:
                    BHER.RecordError("That unit type is not valid for pulling results from TAS TSD. Please select a different result unit type");
                    return null;
            }

            BHR.SimulationResult result = new BHR.SimulationResult();
            result.SimulationResultType = BHR.SimulationResultType.BuildingResult;
            result.SimulationResults.Add(Create.ProfileResult(resultType, unitType, results));

            return result;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TSD Building Data object from a BHoM Environmental Simulation Result")]
        [Input("result", "BHoM Environmental Simulation Result")]
        [Output("TAS TSD Building Data")]
        public static TSD.BuildingData ToTASBuilding(this BHR.SimulationResult result)
        {
            throw new NotImplementedException("This method has not yet been created");
        }
    }
}
