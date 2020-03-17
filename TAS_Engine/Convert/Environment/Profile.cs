/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("Gets a BHoM Environmental Profile from a TAS TBD Profile")]
        [Input("tbdProfile", "TAS TBD Profile")]
        [Input("profileType", "BHoM Environmental Profile Type")]
        [Output("BHoM Environmental Profile")]
        public static BHE.Profile ToBHoM(this TBD.profile tbdProfile, BHE.ProfileCategory profileType)
        {
            BHE.Profile profile = new BHE.Profile();

            profile.Name = tbdProfile.name;
            profile.Category = profileType;
            profile.MultiplicationFactor = tbdProfile.factor;
            profile.SetBackValue = tbdProfile.setbackValue;

            switch (tbdProfile.type)
            {
                case TBD.ProfileTypes.ticValueProfile:
                    profile.ProfileType = BHE.ProfileType.Value;
                    profile.Values.Add(tbdProfile.value);
                    break;
                case TBD.ProfileTypes.ticHourlyProfile:
                    profile.ProfileType = BHE.ProfileType.Hourly;
                    for (int i = 1; i < 25; i++)
                        profile.Values.Add(tbdProfile.hourlyValues[i]);
                    break;
                case TBD.ProfileTypes.ticYearlyProfile:
                    profile.ProfileType = BHE.ProfileType.Yearly;
                    profile.Values = ToDoubleList(tbdProfile.GetYearlyValues());
                    break;
            }

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("ProfileDescription", tbdProfile.description);

            profile.CustomData = tasData;

            return profile;
        }

        [Description("Gets a TAS TBD Profile from a BHoM Environmental Profile")]
        [Input("profile", "BHoM Environmental Profile")]
        [Output("TAS TBD Profile")]
        public static TBD.profile ToTAS(this BHE.Profile profile, TBD.profile tbdProfile)
        {
            if (profile == null) return tbdProfile;

            tbdProfile.name = profile.Name;
            tbdProfile.factor = (float)profile.MultiplicationFactor;
            tbdProfile.setbackValue = (float)profile.SetBackValue;

            switch (profile.ProfileType)
            {
                case BHE.ProfileType.Value:
                    tbdProfile.value = (float)profile.Values[0];
                    break;
                case BHE.ProfileType.Hourly:
                    for (int i = 0; i < 24; i++)
                        tbdProfile.hourlyValues[i + 1] = (float)profile.Values[i];
                    break;
                case BHE.ProfileType.Yearly:
                    for (int i = 0; i < 8760; i++)
                        tbdProfile.yearlyValues[i + 1] = (float)profile.Values[i];
                    break;
            }

            Dictionary<string, object> tasData = profile.CustomData;

            if (tasData != null)
            {
                tbdProfile.description = (tasData.ContainsKey("ProfileDescriptionUL") ? tasData["ProfileDescriptionUL"].ToString() : "");
            }

            return tbdProfile;
        }
    }
}

