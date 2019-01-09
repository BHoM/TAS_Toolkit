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
            internalCondition.Emitter = tbdCondition.GetHeatingEmitter().ToBHoM(); //TODO: Check with Michal on how we want to store both emitters given one is cooling and one is heating...
            //internalCondition.Emitter = tbdCondition.GetCoolingEmitter().ToBHoM(); //TODO: Check with Michal how to handle both...

            internalCondition.Thermostat = tbdCondition.GetThermostat().ToBHoM();

            return internalCondition;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD InternalCondition from BH.oM.Environment.Elements.InternalCondition")]
        [Input("internalCondition", "BHoM Environmental InternalCondition object")]
        [Output("TAS TBD InternalCondition")]
        public static TBD.InternalConditionClass ToTAS(BHE.InternalCondition internalCondition)
        {
            TBD.InternalConditionClass tbdCondition = new TBD.InternalConditionClass();

            //ToDo: Write the conversion!

            return tbdCondition;
        }
    }
}
