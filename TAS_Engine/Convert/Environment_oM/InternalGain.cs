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
        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.InternalGain from TAS TBD InternalGain")]
        [Input("tbdInternalGain", "TAS TBD InternalGain")]
        [Output("BHoM Environmental InternalGain object")]
        public static BHE.InternalGain ToBHoM(this TBD.InternalGain tbdInternalGain)
        {
            if (tbdInternalGain == null) return null;

            BHE.InternalGain internalGain = new BHE.InternalGain();
            internalGain.Name = tbdInternalGain.name;
            internalGain.Illuminance = tbdInternalGain.targetIlluminance;
            internalGain.OutsideAirRatePerPerson = tbdInternalGain.freshAirRate;
            internalGain.PersonGain = tbdInternalGain.personGain;
            internalGain.RadiationProperties.EquipmentRadiation = tbdInternalGain.equipmentRadProp;
            internalGain.RadiationProperties.LightingRadiation = tbdInternalGain.lightingRadProp;
            internalGain.RadiationProperties.OccupantRadiation = tbdInternalGain.occupantRadProp;
            internalGain.CoefficientProperties.EquipmentViewCoefficient = tbdInternalGain.equipmentViewCoefficient;
            internalGain.CoefficientProperties.LightingViewCoefficient = tbdInternalGain.lightingViewCoefficient;
            internalGain.CoefficientProperties.OccupantViewCoefficient = tbdInternalGain.occupantViewCoefficient;

            internalGain.Profiles = tbdInternalGain.Profiles();

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("InternalGainActivityID", tbdInternalGain.activityID);
            tasData.Add("InternalGainDescription", tbdInternalGain.description);
            tasData.Add("InternalDomesticHotWater", tbdInternalGain.domesticHotWater);

            internalGain.CustomData.Add("TASData", tasData);
            return internalGain;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD InternalGain from BH.oM.Environment.Elements.InternalGain")]
        [Input("internalGain", "BHoM Environmental InternalGain object")]
        [Output("TAS TBD InternalGain")]
        public static TBD.InternalGainClass ToTAS(this BHE.InternalGain internalGain)
        {
            TBD.InternalGainClass tbdInternalGain = new TBD.InternalGainClass();
            if (internalGain == null) return tbdInternalGain;

            tbdInternalGain.name = internalGain.Name;
            tbdInternalGain.targetIlluminance = (float)internalGain.Illuminance;
            tbdInternalGain.freshAirRate = (float)internalGain.OutsideAirRatePerPerson;
            tbdInternalGain.personGain = (float)internalGain.PersonGain;
            tbdInternalGain.equipmentRadProp = (float)internalGain.RadiationProperties.EquipmentRadiation;
            tbdInternalGain.lightingRadProp = (float)internalGain.RadiationProperties.LightingRadiation;
            tbdInternalGain.occupantRadProp = (float)internalGain.RadiationProperties.OccupantRadiation;
            tbdInternalGain.equipmentViewCoefficient = (float)internalGain.CoefficientProperties.EquipmentViewCoefficient;
            tbdInternalGain.lightingViewCoefficient = (float)internalGain.CoefficientProperties.LightingViewCoefficient;
            tbdInternalGain.occupantViewCoefficient = (float)internalGain.CoefficientProperties.OccupantViewCoefficient;

            Dictionary<string, object> tasData = null;
            if (internalGain.CustomData.ContainsKey("TASData"))
                tasData = internalGain.CustomData["TASData"] as Dictionary<string, object>;

            if (tasData != null)
            {
                tbdInternalGain.activityID = (tasData.ContainsKey("InternalGainActivityID") ? System.Convert.ToInt32(tasData["InternalGainActivityID"]) : 0);
                tbdInternalGain.description = (tasData.ContainsKey("InternalGainDescription") ? tasData["InternalGainDescription"].ToString() : "");
                tbdInternalGain.domesticHotWater = (tasData.ContainsKey("InternalDomesticHotWater") ? (float)System.Convert.ToDouble(tasData["InternalDomesticHotWater"]) : 0);
            }

            return tbdInternalGain;
        }
    }
}
