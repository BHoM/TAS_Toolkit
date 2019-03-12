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
using BHEE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;
using TBD;

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
            tasData.Add("targetIlluminance", tbdInternalGain.targetIlluminance);

            //Lighting
            BHE.Gain lightGain = new BHE.Gain();
            lightGain.Name = "L " + tbdInternalGain.name;
            lightGain.GainType = BHE.GainType.Lighting;

            BHP.GainPropertiesLighting lightingGain = new BHP.GainPropertiesLighting();
            lightingGain.GainType = BHE.GainType.Lighting;
            lightingGain.GainUnit = BHE.GainUnit.WattsPerSquareMetre;
            lightingGain.RadiantFraction = tbdInternalGain.lightingRadProp;
            lightingGain.ViewCoefficient = tbdInternalGain.lightingViewCoefficient;


            TBD.profile tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticLG);
            BHEE.Profile aProfile = tbdProfile.ToBHoM(BHEE.ProfileCategory.Gain);
            lightingGain.Profile = aProfile;

            lightGain.GainProperties = lightingGain;
            lightGain.CustomData = tasData;
            gains.Add(lightGain);

            //Occupancy
            BHE.Gain occupantGain = new BHE.Gain();
            occupantGain.Name = "O " + tbdInternalGain.name;
            occupantGain.GainType = BHE.GainType.People;

            BHP.GainPropertiesPeople peopleGain = new BHP.GainPropertiesPeople();
            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticOSG);
            //curent limitation it works if we use hourly or yearl profile apprach with 0-1 values and factor as max
            peopleGain.SensibleGain = tbdProfile.factor; //Unit W/m2 sensible gain
            TBD.profile tbdProfileLat = tbdInternalGain.GetProfile((int)TBD.Profiles.ticOLG);
            peopleGain.LatentGain = tbdProfileLat.factor; //Unit W/m2 latent gain
            double aPeopleDesity = (peopleGain.SensibleGain + peopleGain.LatentGain) / tbdInternalGain.personGain; //Unit people/m2
            aProfile = tbdProfile.ToBHoM(BHEE.ProfileCategory.Gain);

            for (int i = 0; i < aProfile.Values.Count; i++)
                aProfile.Values[i] = aProfile.Values[i] * aPeopleDesity;

            peopleGain.GainUnit = BHE.GainUnit.PeoplePerSquareMetre;
            peopleGain.GainType = BHE.GainType.People;
            peopleGain.RadiantFraction = tbdInternalGain.occupantRadProp;
            peopleGain.ViewCoefficient = tbdInternalGain.occupantViewCoefficient;

            peopleGain.Profile = aProfile;
            occupantGain.GainProperties = peopleGain;
            occupantGain.CustomData = tasData;
            gains.Add(occupantGain);

            //Equipment Sens
            BHE.Gain equipGain = new BHE.Gain();
            equipGain.Name = "EqS " + tbdInternalGain.name;
            equipGain.GainType = BHE.GainType.Equipment;
            BHP.GainPropertiesEquipmentSensible equipmentSensibleGain = new BHP.GainPropertiesEquipmentSensible();
            equipmentSensibleGain.GainType = BHE.GainType.Equipment;
            equipmentSensibleGain.GainUnit = BHE.GainUnit.WattsPerSquareMetre;
            equipmentSensibleGain.RadiantFraction = tbdInternalGain.equipmentRadProp;
            equipmentSensibleGain.ViewCoefficient = tbdInternalGain.equipmentViewCoefficient;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticESG);
            aProfile = tbdProfile.ToBHoM(BHEE.ProfileCategory.Gain);
            equipmentSensibleGain.Profile = aProfile;

            equipGain.GainProperties = equipmentSensibleGain;
            equipGain.CustomData = tasData;
            gains.Add(equipGain);

            //Equipment Lat
            equipGain = new BHE.Gain();
            equipGain.Name = "EqL " + tbdInternalGain.name;
            equipGain.GainType = BHE.GainType.Equipment;
            BHP.GainPropertiesEquipmentLatent equipmentLatentGain = new BHP.GainPropertiesEquipmentLatent();
            equipmentLatentGain.GainType = BHE.GainType.Equipment;
            equipmentLatentGain.GainUnit = BHE.GainUnit.WattsPerSquareMetre;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticELG);
            aProfile = tbdProfile.ToBHoM(BHEE.ProfileCategory.Gain);
            equipmentLatentGain.Profile = aProfile;

            equipGain.GainProperties = equipmentLatentGain;
            equipGain.CustomData = tasData;
            gains.Add(equipGain);


            //Pollutant
            BHE.Gain pollGain = new BHE.Gain();
            pollGain.Name = "P " + tbdInternalGain.name;
            pollGain.GainType = BHE.GainType.Equipment;
            BHP.GainPropertiesPollutant pollutanttGain = new BHP.GainPropertiesPollutant();
            pollutanttGain.GainType = BHE.GainType.Pollutant;
            pollutanttGain.GainUnit = BHE.GainUnit.LitresPerHourPerSquareMetre;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticCOG);
            aProfile = tbdProfile.ToBHoM(BHEE.ProfileCategory.Gain);
            pollutanttGain.Profile = aProfile;

            pollGain.GainProperties = pollutanttGain;
            pollGain.CustomData = tasData;
            gains.Add(pollGain);

            //Infiltration
            BHE.Gain infGain = new BHE.Gain();
            infGain.Name = "I " + tbdInternalGain.name;
            infGain.GainType = BHE.GainType.Equipment;
            BHP.GainPropertiesInfiltration infiltrationGain = new BHP.GainPropertiesInfiltration();
            infiltrationGain.GainType = BHE.GainType.Infiltration;
            infiltrationGain.GainUnit = BHE.GainUnit.AirChangesPerHour;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticI);
            aProfile = tbdProfile.ToBHoM(BHEE.ProfileCategory.Gain);
            infiltrationGain.Profile = aProfile;

            infGain.GainProperties = infiltrationGain;
            infGain.CustomData = tasData;
            gains.Add(infGain);


            //tbdInternalGain.freshAirRate; //ToDo: Figure this one out later...

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
