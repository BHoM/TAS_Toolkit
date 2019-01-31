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
        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.Emitter from TAS TBD Emitter")]
        [Input("tbdEmitter", "TAS TBD Emitter")]
        [Output("BHoM Environmental Emitter object")]
        public static BHE.Emitter ToBHoM(this TBD.Emitter tbdEmitter)
        {
            if (tbdEmitter == null) return null;

            BHE.Emitter emitter = new BHE.Emitter();
            emitter.Name = tbdEmitter.name;
            emitter.EmitterProperties.RadiantProportion = tbdEmitter.radiantProportion;
            emitter.EmitterProperties.ViewCoefficient = tbdEmitter.viewCoefficient;
            emitter.EmitterProperties.SwitchOffOutsideTemp = tbdEmitter.offOutsideTemp;
            emitter.EmitterProperties.MaxOutsideTemp = tbdEmitter.maxOutsideTemp;
            emitter.EmitterType = tbdEmitter.emitterType.ToBHoM();

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("EmitterDescription", tbdEmitter.description);

            emitter.CustomData = tasData;

            return emitter;            
        }

        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.EmitterType from TAS TBD EmitterTypes")]
        [Input("tbdEmitterType", "TAS TBD EmitterTypes object")]
        [Output("BHoM Environmental EmitterType enum value")]
        public static BHE.EmitterType ToBHoM(this TBD.EmitterTypes tbdEmitterType)
        {
            switch(tbdEmitterType)
            {
                case TBD.EmitterTypes.ticCooling:
                case TBD.EmitterTypes.ticCompensatedCooling:
                    return BHE.EmitterType.Cooling;
                case TBD.EmitterTypes.ticHeating:
                case TBD.EmitterTypes.ticCompensatedHeating:
                    return BHE.EmitterType.Heating;
                default:
                    return BHE.EmitterType.Undefined;
            }
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD Emitter from BH.oM.Environment.Elements.Emitter")]
        [Input("emitter", "BHoM Environmental Emitter object")]
        [Output("TAS TBD Emitter")]
        public static TBD.Emitter ToTAS(this BHE.Emitter emitter, TBD.Emitter tbdEmitter)
        {
            if (emitter == null) return tbdEmitter;

            tbdEmitter.name = emitter.Name;
            tbdEmitter.radiantProportion = (float)emitter.EmitterProperties.RadiantProportion;
            tbdEmitter.viewCoefficient = (float)emitter.EmitterProperties.ViewCoefficient;
            tbdEmitter.offOutsideTemp = (float)emitter.EmitterProperties.SwitchOffOutsideTemp;
            tbdEmitter.maxOutsideTemp = (float)emitter.EmitterProperties.MaxOutsideTemp;
            tbdEmitter.emitterType = emitter.EmitterType.ToTAS();

            Dictionary<string, object> tasData = emitter.CustomData;

            if (tasData != null)
            {
                tbdEmitter.description = (tasData.ContainsKey("EmitterDescription") ? tasData["EmitterDescription"].ToString() : "");
            }

            return tbdEmitter;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD EmitterTypes from BH.oM.Environment.Elements.EmitterType")]
        [Input("emitterType", "BHoM Environmental EmitterType enum value")]
        [Output("TAS TBD EmitterType enum value")]
        public static TBD.EmitterTypes ToTAS(this BHE.EmitterType emitterType)
        {
            switch(emitterType)
            {
                case BHE.EmitterType.Cooling:
                    return TBD.EmitterTypes.ticCooling;
                case BHE.EmitterType.Heating:
                    return TBD.EmitterTypes.ticHeating;
                default:
                    return TBD.EmitterTypes.ticHeating;
            }
        }
    }
}
