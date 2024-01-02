/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BHE = BH.oM.Environment.SpaceCriteria;
using BHG = BH.oM.Geometry;
using TBD;

using BH.oM.Base.Attributes;
using System.ComponentModel;
using BH.oM.Adapters.TAS.Fragments;

using BH.Engine.Base;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets BHoM InternalGain from TAS TBD InternalGain")]
        [Input("tbdInternalGain", "TAS TBD InternalGain")]
        [Output("BHoM Environmental InternalGain object")]
        public static List<BHE.IGain> FromTAS(this TBD.InternalGain tbdInternalGain)
        {
            if (tbdInternalGain == null) return null;

            List<BHE.IGain> gains = new List<BHE.IGain>();

            TASInternalGainData tasData = new TASInternalGainData();
            tasData.ActivityID = System.Convert.ToInt32(tbdInternalGain.activityID);
            tasData.DomesticHotWater = System.Convert.ToDouble(tbdInternalGain.domesticHotWater);
            tasData.TargetIlluminance = System.Convert.ToDouble(tbdInternalGain.targetIlluminance);

            TASDescription tASDescription = new TASDescription();
            tASDescription.Description = tbdInternalGain.description.RemoveBrackets();

            //Lighting
            BHE.Lighting lightGain = new BHE.Lighting();
            lightGain.Name = "L " + tbdInternalGain.name;
            lightGain.RadiantFraction = tbdInternalGain.lightingRadProp;

            TBD.profile tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticLG);
            BHE.Profile aProfile = tbdProfile.FromTAS();
            lightGain.Profile = aProfile;
            lightGain.Fragments.Add(tasData);
            lightGain.Fragments.Add(tASDescription);
            gains.Add(lightGain);

            //Occupancy
            BHE.People occupantGain = new BHE.People();
            occupantGain.Name = "O " + tbdInternalGain.name;
            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticOSG);
            //curent limitation it works if we use hourly or yearl profile apprach with 0-1 values and factor as max
            occupantGain.Sensible = tbdProfile.factor; //Unit W/m2 sensible gain
            TBD.profile tbdProfileLat = tbdInternalGain.GetProfile((int)TBD.Profiles.ticOLG);
            occupantGain.Latent = tbdProfileLat.factor; //Unit W/m2 latent gain
            double aPeopleDesity = (occupantGain.Sensible + occupantGain.Latent) / tbdInternalGain.personGain; //Unit people/m2
            aProfile = tbdProfile.FromTAS();

            for (int i = 0; i < aProfile.HourlyValues.Count; i++)
                aProfile.HourlyValues[i] = aProfile.HourlyValues[i] * aPeopleDesity;

            occupantGain.RadiantFraction = tbdInternalGain.occupantRadProp;

            occupantGain.Profile = aProfile;
            occupantGain.Fragments.Add(tasData);
            occupantGain.Fragments.Add(tASDescription);
            gains.Add(occupantGain);

            //Equipment
            BHE.Equipment equipGain = new BHE.Equipment();
            equipGain.Name = "Equipment " + tbdInternalGain.name;
            equipGain.RadiantFraction = tbdInternalGain.equipmentRadProp;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticESG);
            aProfile = tbdProfile.FromTAS();
            equipGain.Profile = aProfile;

            equipGain.Fragments.Add(tasData);
            equipGain.Fragments.Add(tASDescription);
            gains.Add(equipGain);


            //Pollutant
            BHE.Pollutant pollGain = new BHE.Pollutant();
            pollGain.Name = "P " + tbdInternalGain.name;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticCOG);
            aProfile = tbdProfile.FromTAS();
            pollGain.Profile = aProfile;

            pollGain.Fragments.Add(tasData);
            pollGain.Fragments.Add(tASDescription);
            gains.Add(pollGain);

            //Infiltration
            BHE.Infiltration infGain = new BHE.Infiltration();
            infGain.Name = "I " + tbdInternalGain.name;

            tbdProfile = tbdInternalGain.GetProfile((int)TBD.Profiles.ticI);
            aProfile = tbdProfile.FromTAS();
            infGain.Profile = aProfile;

            infGain.Fragments.Add(tasData);
            infGain.Fragments.Add(tASDescription);
            gains.Add(infGain);


            //tbdInternalGain.freshAirRate; //ToDo: Figure this one out later...

            return gains;
        }

        [Description("Gets TAS TBD InternalGain from BHoM InternalGain")]
        [Input("internalGains", "BHoM Environmental InternalGain object")]
        [Output("TAS TBD InternalGain")]
        //public static TBD.InternalGain ToTAS(this List<BHE.Gain> internalGains)
        public static TBD.InternalGain ToTAS(this List<BHE.IGain> internalGains, TBD.InternalGain tbdInternalGain)
        {
            //TODO: Only name is working, add everything else

            if (internalGains == null) return null;
            foreach (BHE.IGain internalGain in internalGains)
            {
                if (internalGain.GetType() == typeof(BHE.Lighting))
                {
                    tbdInternalGain.name = internalGain.Name;
                }
                if (internalGain.GetType() == typeof(BHE.Equipment))
                {
                    tbdInternalGain.name = internalGain.Name;
                }

                //Lighting	                  
                //TBD.InternalGain lightGain = new TBD.InternalGain();	                
                //BHE.Gain lightGain = new BHE.Gain();	                

                //    tbdInternalGain.occupantRadProp = (float)occupantGain.RadiantFraction;
                //tbdInternalGain.name = internalGains[292].Name;	                
                //BHP.GainPropertiesLighting lightingGain = new BHP.GainPropertiesLighting();	

                //tbdInternalGain.lightingViewCoefficient = (float)lightingGain.ViewCoefficient;	

                //lightingGain.GainType = BHE.GainType.Lighting;	
                //lightingGain.GainUnit = BHE.GainUnit.WattsPerSquareMetre;	
                //lightingGain.RadiantFraction = tbdInternalGain.lightingRadProp;	
                //lightingGain.ViewCoefficient = tbdInternalGain.lightingViewCoefficient;	

                //if (internalGains == null) return tbdInternalGain;	
              
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
                */
                //ToDo: Fix this

                //if (internalGain.GainType == BHE.GainType.Equipment)
                //{
                //    BHP.GainPropertiesEquipmentLatent equipmentLatentGain = new BHP.GainPropertiesEquipmentLatent();
                //}

                //if (internalGain.GainType == BHE.GainType.People)  
                //{
                //    BHP.GainPropertiesPeople occupantGain = internalGain.GainProperties as BHP.GainPropertiesPeople;
                //    tbdInternalGain.occupantRadProp = (float)occupantGain.RadiantFraction;
                //    tbdInternalGain.occupantViewCoefficient = (float)occupantGain.ViewCoefficient;
                //}
            }
            return tbdInternalGain;
        }
    }
}






