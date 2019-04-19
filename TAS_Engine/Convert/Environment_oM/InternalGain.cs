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
            lightGain.Type = BHE.GainType.Lighting;
          

            BHP.LightingGain lightingGain = new BHP.LightingGain();
            lightingGain.Unit = BHE.GainUnit.WattsPerSquareMetre;
            lightingGain.RadiantFraction = tbdInternalGain.lightingRadProp;
            lightingGain.ViewCoefficient = tbdInternalGain.lightingViewCoefficient;

            TBD.profile tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticLG);
            BHE.Profile aProfile = tbdProfile.ToBHoM(BHE.ProfileCategory.Gain);
            lightingGain.Profile = aProfile;

            lightGain.Properties = lightingGain;
            lightGain.CustomData = tasData;
            gains.Add(lightGain);

            //Occupancy
            BHE.Gain occupantGain = new BHE.Gain();
            occupantGain.Name = "O " + tbdInternalGain.name;
            occupantGain.Type = BHE.GainType.People;

            BHP.PeopleGain peopleGain = new BHP.PeopleGain();
            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticOSG);
            //curent limitation it works if we use hourly or yearl profile apprach with 0-1 values and factor as max
            peopleGain.SensibleGain = tbdProfile.factor; //Unit W/m2 sensible gain
            TBD.profile tbdProfileLat = tbdInternalGain.GetProfile((int)TBD.Profiles.ticOLG);
            peopleGain.LatentGain = tbdProfileLat.factor; //Unit W/m2 latent gain
            double aPeopleDesity = (peopleGain.SensibleGain + peopleGain.LatentGain) / tbdInternalGain.personGain; //Unit people/m2
            aProfile = tbdProfile.ToBHoM(BHE.ProfileCategory.Gain);

            for (int i = 0; i < aProfile.Values.Count; i++)
                aProfile.Values[i] = aProfile.Values[i] * aPeopleDesity;

            peopleGain.Unit = BHE.GainUnit.PeoplePerSquareMetre;
            peopleGain.RadiantFraction = tbdInternalGain.occupantRadProp;
            peopleGain.ViewCoefficient = tbdInternalGain.occupantViewCoefficient;

            peopleGain.Profile = aProfile;
            occupantGain.Properties = peopleGain;
            occupantGain.CustomData = tasData;
            gains.Add(occupantGain);

            //Equipment Sens
            BHE.Gain equipGain = new BHE.Gain();
            equipGain.Name = "EqS " + tbdInternalGain.name;
            equipGain.Type = BHE.GainType.Equipment;

            BHP.SensibleEquipmentGain equipmentSensibleGain = new BHP.SensibleEquipmentGain();
            equipmentSensibleGain.Unit = BHE.GainUnit.WattsPerSquareMetre;
            equipmentSensibleGain.RadiantFraction = tbdInternalGain.equipmentRadProp;
            equipmentSensibleGain.ViewCoefficient = tbdInternalGain.equipmentViewCoefficient;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticESG);
            aProfile = tbdProfile.ToBHoM(BHE.ProfileCategory.Gain);
            equipmentSensibleGain.Profile = aProfile;

            equipGain.Properties = equipmentSensibleGain;
            equipGain.CustomData = tasData;
            gains.Add(equipGain);

            //Equipment Lat
            equipGain = new BHE.Gain();
            equipGain.Name = "EqL " + tbdInternalGain.name;
            equipGain.Type = BHE.GainType.Equipment;

            BHP.LatentEquipmentGain equipmentLatentGain = new BHP.LatentEquipmentGain();
            equipmentLatentGain.Unit = BHE.GainUnit.WattsPerSquareMetre;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticELG);
            aProfile = tbdProfile.ToBHoM(BHE.ProfileCategory.Gain);
            equipmentLatentGain.Profile = aProfile;

            equipGain.Properties = equipmentLatentGain;
            equipGain.CustomData = tasData;
            gains.Add(equipGain);


            //Pollutant
            BHE.Gain pollGain = new BHE.Gain();
            pollGain.Name = "P " + tbdInternalGain.name;
            pollGain.Type = BHE.GainType.Equipment;

            BHP.PollutantGain pollutanttGain = new BHP.PollutantGain();
            pollutanttGain.Unit = BHE.GainUnit.LitresPerHourPerSquareMetre;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticCOG);
            aProfile = tbdProfile.ToBHoM(BHE.ProfileCategory.Gain);
            pollutanttGain.Profile = aProfile;

            pollGain.Properties = pollutanttGain;
            pollGain.CustomData = tasData;
            gains.Add(pollGain);

            //Infiltration
            BHE.Gain infGain = new BHE.Gain();
            infGain.Name = "I " + tbdInternalGain.name;
            infGain.Type = BHE.GainType.Equipment;

            BHP.InfiltrationGain infiltrationGain = new BHP.InfiltrationGain();
            infiltrationGain.Unit = BHE.GainUnit.AirChangesPerHour;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticI);
            aProfile = tbdProfile.ToBHoM(BHE.ProfileCategory.Gain);
            infiltrationGain.Profile = aProfile;

            infGain.Properties = infiltrationGain;
            infGain.CustomData = tasData;
            gains.Add(infGain);


            //tbdInternalGain.freshAirRate; //ToDo: Figure this one out later...

            return gains;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD InternalGain from BH.oM.Environment.Elements.InternalGain")]
        [Input("internalGain", "BHoM Environmental InternalGain object")]
        [Output("TAS TBD InternalGain")]
        //public static TBD.InternalGain ToTAS(this List<BHE.Gain> internalGains)
        public static TBD.InternalGain ToTAS(this List<BHE.Gain> internalGains, TBD.InternalGain tbdInternalGain)
        {
            //TODO:Only name is working, add everything else

            if (internalGains == null) return null;
            foreach (BHE.Gain internalGain in internalGains)
            {
                if (internalGain.Type == BHE.GainType.Lighting)
                {
                    tbdInternalGain.name = internalGain.Name;
                    BHP.LightingGain lightingGain = new BHP.LightingGain();
                    Dictionary<string, object> lightingData = internalGain.CustomData;
                    if (lightingData != null)
                        tbdInternalGain.lightingRadProp = (lightingData.ContainsKey("LightingRadiation") ? (float)System.Convert.ToDouble(lightingData["LightingRadiation"]) : 0);
                        tbdInternalGain.lightingViewCoefficient = (lightingData.ContainsKey("LightViewCoefficient") ? (float)System.Convert.ToDouble(lightingData["LightViewCoefficient"]) : 0);
                    Dictionary<string, object> lightingData2 = lightingGain.CustomData;
                    if (lightingData2 != null)
                        tbdInternalGain.lightingRadProp = (lightingData2.ContainsKey("LightingRadiation") ? (float)System.Convert.ToDouble(lightingData2["LightingRadiation"]) : 0);
                        tbdInternalGain.lightingViewCoefficient = (lightingData2.ContainsKey("LightViewCoefficient") ? (float)System.Convert.ToDouble(lightingData2["LightViewCoefficient"]) : 0);
                }
                if (internalGain.Type == BHE.GainType.Equipment)
                {
                    tbdInternalGain.name = internalGain.Name;
                    BHP.LatentEquipmentGain equipmentLatentGain = new BHP.LatentEquipmentGain();
                    Dictionary<string, object> equipmentData = internalGain.CustomData;
                    if (equipmentData != null)
                        tbdInternalGain.equipmentRadProp = (equipmentData.ContainsKey("EquipmentRadiation") ? (float)System.Convert.ToDouble(equipmentData["EquipmentRadiation"]) : 0);
                        tbdInternalGain.equipmentViewCoefficient = (equipmentData.ContainsKey("EquipmentViewCoefficient") ? (float)System.Convert.ToDouble(equipmentData["EquipmentViewCoefficient"]) : 0);
                    Dictionary<string, object> equipmentData2 = equipmentLatentGain.CustomData;
                    if (equipmentData2 != null)
                        tbdInternalGain.equipmentRadProp = (equipmentData2.ContainsKey("EquipmentRadiation") ? (float)System.Convert.ToDouble(equipmentData2["EquipmentRadiation"]) : 0);
                        tbdInternalGain.equipmentViewCoefficient = (equipmentData2.ContainsKey("EquipmentViewCoefficient") ? (float)System.Convert.ToDouble(equipmentData2["EquipmentViewCoefficient"]) : 0);
                }

                //Dictionary<string, object> tasData = internalGain.CustomData;
                //List<BHE.Gain> gains = new List<BHE.Gain>();
                //foreach (BHE.Gain internalGain in internalGains)
                //{
                //    Dictionary<string, object> tasData = new Dictionary<string, object>();
                //    tasData.Add("InternalGainActivityID", tbdInternalGain.activityID);
                //    tasData.Add("InternalGainDescription", tbdInternalGain.description);
                //    tasData.Add("InternalDomesticHotWater", tbdInternalGain.domesticHotWater);
                //    tasData.Add("targetIlluminance", tbdInternalGain.targetIlluminance);
                //}

                //Lighting
                //TBD.InternalGain lightGain = new TBD.InternalGain();
                //BHE.Gain lightGain = new BHE.Gain();

                //tbdInternalGain.name = internalGains[292].Name;
                //BHP.GainPropertiesLighting lightingGain = new BHP.GainPropertiesLighting();

                //tbdInternalGain.lightingViewCoefficient = (float)lightingGain.ViewCoefficient;

                //lightingGain.GainType = BHE.GainType.Lighting;
                //lightingGain.GainUnit = BHE.GainUnit.WattsPerSquareMetre;
                //lightingGain.RadiantFraction = tbdInternalGain.lightingRadProp;
                //lightingGain.ViewCoefficient = tbdInternalGain.lightingViewCoefficient;

                //    Dictionary<string, object> tasData = internalGain.CustomData;
                //    if (tasData != null)
                //    {
                //        tbdInternalGain.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
                //        tbdInternalGain.equipmentViewCoefficient = (tasData.ContainsKey("EquipmentViewCoefficient") ? (float)System.Convert.ToDouble(tasData["EquipmentViewCoefficient"]) : 0);
                //        tbdInternalGain.lightingViewCoefficient = (tasData.ContainsKey("LightingViewCoefficient") ? (float)System.Convert.ToDouble(tasData["LightingViewCoefficient"]) : 0);
                //        tbdInternalGain.occupantViewCoefficient = (tasData.ContainsKey("OccupantViewCoefficient") ? (float)System.Convert.ToDouble(tasData["OccupantViewCoefficient"]) : 0);
                //        tbdInternalGain.equipmentRadProp = (tasData.ContainsKey("EquipmentRadiation") ? (float)System.Convert.ToDouble(tasData["EquipmentRadiation"]) : 0);
                //        tbdInternalGain.lightingRadProp = (tasData.ContainsKey("LightingRadiation") ? (float)System.Convert.ToDouble(tasData["LightingRadiation"]) : 0);
                //        tbdInternalGain.occupantRadProp = (tasData.ContainsKey("OccupantRadiation") ? (float)System.Convert.ToDouble(tasData["OccupantRadiation"]) : 0);
                //        tbdInternalGain.targetIlluminance = (tasData.ContainsKey("Illuminance") ? (float)System.Convert.ToDouble(tasData["Illuminance"]) : 0);
                //    }

                //if (internalGains == null) return tbdInternalGain;
                //    if (tasData != null)
                //    {
                //        tbdInternalGain.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
                //        tbdInternalGain.equipmentViewCoefficient = (tasData.ContainsKey("EquipmentViewCoefficient") ? (float)System.Convert.ToDouble(tasData["EquipmentViewCoefficient"]) : 0);
                //        tbdInternalGain.lightingViewCoefficient = (tasData.ContainsKey("LightingViewCoefficient") ? (float)System.Convert.ToDouble(tasData["LightingViewCoefficient"]) : 0);
                //        tbdInternalGain.occupantViewCoefficient = (tasData.ContainsKey("OccupantViewCoefficient") ? (float)System.Convert.ToDouble(tasData["OccupantViewCoefficient"]) : 0);
                //        tbdInternalGain.equipmentRadProp = (tasData.ContainsKey("EquipmentRadiation") ? (float)System.Convert.ToDouble(tasData["EquipmentRadiation"]) : 0);
                //        tbdInternalGain.lightingRadProp = (tasData.ContainsKey("LightingRadiation") ? (float)System.Convert.ToDouble(tasData["LightingRadiation"]) : 0);
                //        tbdInternalGain.occupantRadProp = (tasData.ContainsKey("OccupantRadiation") ? (float)System.Convert.ToDouble(tasData["OccupantRadiation"]) : 0);
                //        tbdInternalGain.targetIlluminance = (tasData.ContainsKey("Illuminance") ? (float)System.Convert.ToDouble(tasData["Illuminance"]) : 0);
                //    }

                //}
                //}
                //if (internalGain.GainType == BHE.GainType.Lighting)
                //    tbdGain.GetType() = BHE.GainType.Lighting;

                ////Lighting
                //BHE.Gain lightGain = new BHE.Gain();
                //lightGain.Name = "L " + tbdInternalGain.name;
                //lightGain.GainType = BHE.GainType.Lighting;

                //List<TBD.InternalGain> gains = new List<TBD.InternalGain>();
                ////TODO: Add new versions of Internal Gains
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
            }
            return tbdInternalGain;
        }
    }
}

