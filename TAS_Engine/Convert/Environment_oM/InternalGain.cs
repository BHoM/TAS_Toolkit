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
using BHE = BH.oM.Environment.Gains;
using BHP = BH.oM.Environment.Properties;
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
        public static List<BHE.Gain> ToBHoM(this TBD.InternalGain tbdInternalGain)
        {
            if (tbdInternalGain == null) return null;

            List<BHE.Gain> gains = new List<BHE.Gain>();

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("InternalGainActivityID", tbdInternalGain.activityID);
            tasData.Add("InternalGainDescription", tbdInternalGain.description);
            tasData.Add("InternalDomesticHotWater", tbdInternalGain.domesticHotWater);

            BHE.Gain occupantGain = new BHE.Gain();
            occupantGain.Name = tbdInternalGain.name;

            BHP.GainPropertiesPeople peopleGain = new BHP.GainPropertiesPeople();
            peopleGain.GainType = BHE.GainType.People;
            peopleGain.GainUnit = BHE.GainUnit.NumberOfPeople;
            peopleGain.RadiantFraction = tbdInternalGain.occupantRadProp;
            peopleGain.ViewCoefficient = tbdInternalGain.occupantViewCoefficient;
            peopleGain.SensibleGain = tbdInternalGain.personGain; //ToDo - review if this is the best place for this!
            occupantGain.GainProperties = peopleGain;
            occupantGain.CustomData = tasData;

            gains.Add(occupantGain);

            BHE.Gain equipGain = new BHE.Gain();
            equipGain.Name = tbdInternalGain.name;

            BHP.GainPropertiesEquipmentSensible equipmentGain = new BHP.GainPropertiesEquipmentSensible();
            equipmentGain.GainType = BHE.GainType.Equipment;
            equipmentGain.GainUnit = BHE.GainUnit.WattsPerSquareMetre;
            equipmentGain.RadiantFraction = tbdInternalGain.equipmentRadProp;
            equipmentGain.ViewCoefficient = tbdInternalGain.equipmentViewCoefficient;
            equipGain.GainProperties = equipmentGain;
            equipGain.CustomData = tasData;

            gains.Add(equipGain);

            BHE.Gain lightGain = new BHE.Gain();
            lightGain.Name = tbdInternalGain.name;

            BHP.GainPropertiesLighting lightingGain = new BHP.GainPropertiesLighting();
            lightingGain.GainUnit = oM.Environment.Gains.GainUnit.Illuminance;
            lightingGain.RadiantFraction = tbdInternalGain.lightingRadProp;
            lightingGain.ViewCoefficient = tbdInternalGain.lightingViewCoefficient;
            lightingGain.Value = tbdInternalGain.targetIlluminance;

            //tbdInternalGain.freshAirRate; //ToDo: Figure this one out later...

            occupantGain.CustomData = tasData;
            return gains;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD InternalGain from BH.oM.Environment.Elements.InternalGain")]
        [Input("internalGain", "BHoM Environmental InternalGain object")]
        [Output("TAS TBD InternalGain")]
        public static TBD.InternalGainClass ToTAS(this List<BHE.Gain> gains)
        {
            TBD.InternalGainClass tbdInternalGain = new TBD.InternalGainClass();
            if (gains == null) return tbdInternalGain;
            /*
            tbdInternalGain.name = gains.Name;
            
            tbdInternalGain.targetIlluminance = (float)gain.Illuminance;
            tbdInternalGain.freshAirRate = (float)gain.OutsideAirRatePerPerson;
            tbdInternalGain.personGain = (float)gain.PersonGain;
            tbdInternalGain.equipmentRadProp = (float)gain.RadiationProperties.EquipmentRadiation;
            tbdInternalGain.lightingRadProp = (float)gain.RadiationProperties.LightingRadiation;
            tbdInternalGain.occupantRadProp = (float)gain.RadiationProperties.OccupantRadiation;
            tbdInternalGain.equipmentViewCoefficient = (float)gain.CoefficientProperties.EquipmentViewCoefficient;
            tbdInternalGain.lightingViewCoefficient = (float)gain.CoefficientProperties.LightingViewCoefficient;
            tbdInternalGain.occupantViewCoefficient = (float)gain.CoefficientProperties.OccupantViewCoefficient;
            
            Dictionary<string, object> tasData = gains.CustomData;

            if (tasData != null)
            {
                tbdInternalGain.activityID = (tasData.ContainsKey("InternalGainActivityID") ? System.Convert.ToInt32(tasData["InternalGainActivityID"]) : 0);
                tbdInternalGain.description = (tasData.ContainsKey("InternalGainDescription") ? tasData["InternalGainDescription"].ToString() : "");
                tbdInternalGain.domesticHotWater = (tasData.ContainsKey("InternalDomesticHotWater") ? (float)System.Convert.ToDouble(tasData["InternalDomesticHotWater"]) : 0);
            }
            */
            //ToDo: Fix this

            return tbdInternalGain;
        }
    }
}
