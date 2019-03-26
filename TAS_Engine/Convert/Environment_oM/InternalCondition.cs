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
using BHEE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.InternalCondition from TAS TBD InternalCondition")]
        [Input("tbdCondition", "TAS TBD InternalCondition")]
        [Output("BHoM Environmental InternalCondition object")]
        public static BHEE.InternalCondition ToBHoM(this TBD.InternalCondition tbdCondition)
        {
            if (tbdCondition == null) return null;

            BHEE.InternalCondition internalCondition = new BHEE.InternalCondition();

            internalCondition.Name = tbdCondition.name;
            internalCondition.IncludeSolarInMeanRadiantTemp = tbdCondition.includeSolarInMRT != 0;

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("InternalConditionDescription", tbdCondition.description);
            internalCondition.CustomData = tasData;

            //int getTypeIndex = 0;
            //TBD.dayType tbdDayType = null;
            //while ((tbdDayType = tbdCondition.GetDayType(getTypeIndex)) != null)
            //{
            //    internalCondition.DayTypes.Add(tbdDayType.ToBHoM());
            //    getTypeIndex++;
            //}

            internalCondition.Emitters.Add(tbdCondition.GetHeatingEmitter().ToBHoM());
            internalCondition.Emitters.Add(tbdCondition.GetCoolingEmitter().ToBHoM());
            //internalCondition.Gains = tbdCondition.GetInternalGain().ToBHoM();
            internalCondition.Thermostat = tbdCondition.GetThermostat().ToBHoM();

            return internalCondition;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD InternalCondition from BH.oM.Environment.Elements.InternalCondition")]
        [Input("internalCondition", "BHoM Environmental InternalCondition object")]
        [Output("TAS TBD InternalCondition")]
        public static TBD.InternalCondition ToTAS(this BHEE.InternalCondition internalCondition, TBD.InternalCondition tbdCondition)
        {
            if (internalCondition == null) return tbdCondition;

            tbdCondition.name = internalCondition.Name;
            tbdCondition.includeSolarInMRT = (internalCondition.IncludeSolarInMeanRadiantTemp ? 1 : 0);

            Dictionary<string, object> tasData = internalCondition.CustomData;

            if (tasData != null)
            {
                tbdCondition.description = (tasData.ContainsKey("InternalConditionDescription") ? tasData["InternalConditionDescription"].ToString() : "");
            }

            //foreach (BHEE.SimulationDayType dayType in internalCondition.DayTypes)
            //    tbdCondition.SetDayType(dayType.ToTAS(), true);

            TBD.Emitter heatingEmitter = tbdCondition.GetHeatingEmitter();
            heatingEmitter = internalCondition.Emitters.Where(x => x.EmitterType == BHEE.EmitterType.Heating).First().ToTAS(heatingEmitter);

            TBD.Emitter coolingEmitter = tbdCondition.GetCoolingEmitter();
            coolingEmitter = internalCondition.Emitters.Where(x => x.EmitterType == BHEE.EmitterType.Cooling).First().ToTAS(coolingEmitter);

            //TBD.InternalGain internalGain = tbdCondition.GetInternalGain();
            //internalGain = internalCondition.Gains.ToTAS();

            TBD.Thermostat thermostat = tbdCondition.GetThermostat();
            thermostat = internalCondition.Thermostat.ToTAS(thermostat);

            return tbdCondition;
        }
    }
}
