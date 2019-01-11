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
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BHEI = BH.oM.Environment.Interface;
using BH.Engine.Environment;
using BH.oM.Geometry;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static List<BH.oM.Environment.Elements.Profile> Profiles(this TBD.Thermostat tbdThermostat)
        {
            List<BH.oM.Environment.Elements.Profile> bHoMProfiles = new List<BH.oM.Environment.Elements.Profile>();

            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticUL).ToBHoM(BHEE.ProfileCategory.Thermostat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticUL).ToBHoM(BHEE.ProfileCategory.Thermostat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticHUL).ToBHoM(BHEE.ProfileCategory.Humidistat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticHLL).ToBHoM(BHEE.ProfileCategory.Humidistat));

            return bHoMProfiles;
        }

        /***************************************************/

        public static List<BH.oM.Environment.Elements.Profile> Profiles(this TBD.InternalGain tbdInternalGain)
        {
            List<BH.oM.Environment.Elements.Profile> bHoMProfiles = new List<BH.oM.Environment.Elements.Profile>();

            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticI).ToBHoM(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticV).ToBHoM(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticLG).ToBHoM(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticOSG).ToBHoM(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticOLG).ToBHoM(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticESG).ToBHoM(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticELG).ToBHoM(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticCOG).ToBHoM(BHEE.ProfileCategory.Gain));


            return bHoMProfiles;
        }

        /***************************************************/
    }
}
