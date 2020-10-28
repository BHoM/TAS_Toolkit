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
using BHE = BH.oM.Environment.SpaceCriteria;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.oM.Adapters.TAS.Fragments;

using BH.Engine.Base;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets a BHoM Environmental Profile from a TAS TBD Profile")]
        [Input("tbdProfile", "TAS TBD Profile")]
        [Input("profileType", "BHoM Environmental Profile Type")]
        [Output("BHoM Environmental Profile")]
        public static BHE.Profile FromTAS(this TBD.profile tbdProfile)
        {
            BHE.Profile profile = new BHE.Profile();

            profile.Name = tbdProfile.name;

            switch (tbdProfile.type)
            {
                case TBD.ProfileTypes.ticValueProfile:
                    profile.HourlyValues.Add(tbdProfile.value);
                    break;
                case TBD.ProfileTypes.ticHourlyProfile:
                    for (int i = 1; i < 25; i++)
                        profile.HourlyValues.Add(tbdProfile.hourlyValues[i]);
                    break;
                case TBD.ProfileTypes.ticYearlyProfile:
                    profile.HourlyValues = ToDoubleList(tbdProfile.GetYearlyValues());
                    break;
            }

            TASDescription tasData = new TASDescription();
            tasData.Description = tbdProfile.description.RemoveBrackets();
            profile.Fragments.Add(tasData);

            return profile;
        }

        [Description("Gets a TAS TBD Profile from a BHoM Environmental Profile")]
        [Input("profile", "BHoM Environmental Profile")]
        [Output("TAS TBD Profile")]
        public static TBD.profile ToTAS(this BHE.Profile profile, TBD.profile tbdProfile)
        {
            if (profile == null) return tbdProfile;

            tbdProfile.name = profile.Name;

            TASDescription tasFragment = profile.FindFragment<TASDescription>(typeof(TASDescription));
            if (tasFragment != null)
            {
                tbdProfile.description = tasFragment.Description;
            }

            return tbdProfile;
        }
    }
}

