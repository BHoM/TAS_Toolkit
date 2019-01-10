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
        public static BHE.InternalCondition ToBHoM(this TBD.InternalCondition tbdCondition)
        {
            if (tbdCondition == null) return null;

            BHE.InternalCondition internalCondition = new BHE.InternalCondition();

            internalCondition.Name = tbdCondition.name;
            internalCondition.IncludeSolarInMeanRadiantTemp = tbdCondition.includeSolarInMRT != 0;

            int getTypeIndex = 0;
            TBD.dayType tbdDayType = null;
            while((tbdDayType = tbdCondition.GetDayType(getTypeIndex)) != null)
            {
                internalCondition.DayTypes.Add(tbdDayType.ToBHoM());
                getTypeIndex++;
            }

            internalCondition.InternalGain = tbdCondition.GetInternalGain().ToBHoM();
            internalCondition.Emitters.Add(tbdCondition.GetHeatingEmitter().ToBHoM());
            internalCondition.Emitters.Add(tbdCondition.GetCoolingEmitter().ToBHoM());

            internalCondition.Thermostat = tbdCondition.GetThermostat().ToBHoM();

            return internalCondition;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD InternalCondition from BH.oM.Environment.Elements.InternalCondition")]
        [Input("internalCondition", "BHoM Environmental InternalCondition object")]
        [Output("TAS TBD InternalCondition")]
        public static TBD.InternalConditionClass ToTAS(this BHE.InternalCondition internalCondition)
        {
            TBD.InternalConditionClass tbdCondition = new TBD.InternalConditionClass();

            tbdCondition.name = internalCondition.Name;
            tbdCondition.includeSolarInMRT = (internalCondition.IncludeSolarInMeanRadiantTemp ? 1 : 0);

            foreach (BHE.SimulationDayType dayType in internalCondition.DayTypes)
                tbdCondition.SetDayType(dayType.ToTAS(), true);

            TBD.EmitterClass heatingEmitter = internalCondition.Emitters.Where(x => x.EmitterType == BHE.EmitterType.Heating).First().ToTAS();
            tbdCondition.GetHeatingEmitter().name = heatingEmitter.name;
            tbdCondition.GetHeatingEmitter().radiantProportion = heatingEmitter.radiantProportion;
            tbdCondition.GetHeatingEmitter().viewCoefficient = heatingEmitter.viewCoefficient;
            tbdCondition.GetHeatingEmitter().offOutsideTemp = heatingEmitter.offOutsideTemp;
            tbdCondition.GetHeatingEmitter().maxOutsideTemp = heatingEmitter.maxOutsideTemp;
            tbdCondition.GetHeatingEmitter().emitterType = heatingEmitter.emitterType;
            tbdCondition.GetHeatingEmitter().description = heatingEmitter.description;

            TBD.EmitterClass coolingEmitter = internalCondition.Emitters.Where(x => x.EmitterType == BHE.EmitterType.Cooling).First().ToTAS();
            tbdCondition.GetCoolingEmitter().name = coolingEmitter.name;
            tbdCondition.GetCoolingEmitter().radiantProportion = coolingEmitter.radiantProportion;
            tbdCondition.GetCoolingEmitter().viewCoefficient = coolingEmitter.viewCoefficient;
            tbdCondition.GetCoolingEmitter().offOutsideTemp = coolingEmitter.offOutsideTemp;
            tbdCondition.GetCoolingEmitter().maxOutsideTemp = coolingEmitter.maxOutsideTemp;
            tbdCondition.GetCoolingEmitter().emitterType = coolingEmitter.emitterType;
            tbdCondition.GetCoolingEmitter().description = coolingEmitter.description;

            TBD.InternalGainClass internalGain = internalCondition.InternalGain.ToTAS();
            tbdCondition.GetInternalGain().name = internalGain.name;
            tbdCondition.GetInternalGain().targetIlluminance = internalGain.targetIlluminance;
            tbdCondition.GetInternalGain().freshAirRate = internalGain.freshAirRate;
            tbdCondition.GetInternalGain().personGain = internalGain.personGain;
            tbdCondition.GetInternalGain().equipmentRadProp = internalGain.equipmentRadProp;
            tbdCondition.GetInternalGain().lightingRadProp = internalGain.lightingRadProp;
            tbdCondition.GetInternalGain().occupantRadProp = internalGain.occupantRadProp;
            tbdCondition.GetInternalGain().equipmentViewCoefficient = internalGain.equipmentViewCoefficient;
            tbdCondition.GetInternalGain().lightingViewCoefficient = internalGain.lightingViewCoefficient;
            tbdCondition.GetInternalGain().occupantViewCoefficient = internalGain.occupantViewCoefficient;

            TBD.ThermostatClass thermostat = internalCondition.Thermostat.ToTAS();
            tbdCondition.GetThermostat().name = thermostat.name;
            tbdCondition.GetThermostat().controlRange = thermostat.controlRange;
            tbdCondition.GetThermostat().proportionalControl = thermostat.proportionalControl;
            tbdCondition.GetThermostat().radiantProportion = thermostat.radiantProportion;
            tbdCondition.GetThermostat().description = thermostat.description;

            return tbdCondition;
        }
    }
}
