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
using BHG = BH.oM.Geometry;
using BHEG = BH.oM.Environment.Gains;
using BH.Engine.Environment;
using BH.oM.Geometry;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static List<BHEG.Profile> Profiles(this TBD.Thermostat tbdThermostat)
        {
            List<BHEG.Profile> bHoMProfiles = new List<BHEG.Profile>();

            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticUL).FromTAS(BHEG.ProfileType.Thermostat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticUL).FromTAS(BHEG.ProfileType.Thermostat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticHUL).FromTAS(BHEG.ProfileType.Humidistat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticHLL).FromTAS(BHEG.ProfileType.Humidistat));

            return bHoMProfiles;
        }

        /***************************************************/

        public static List<BHEG.Profile> Profiles(this TBD.InternalGain tbdInternalGain)
        {
            List<BHEG.Profile> bHoMProfiles = new List<BHEG.Profile>();

            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticI).FromTAS(BHEG.ProfileType.EquipmentGain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticV).FromTAS(BHEG.ProfileType.EquipmentGain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticLG).FromTAS(BHEG.ProfileType.EquipmentGain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticOSG).FromTAS(BHEG.ProfileType.EquipmentGain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticOLG).FromTAS(BHEG.ProfileType.EquipmentGain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticESG).FromTAS(BHEG.ProfileType.EquipmentGain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticELG).FromTAS(BHEG.ProfileType.EquipmentGain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticCOG).FromTAS(BHEG.ProfileType.EquipmentGain));


            return bHoMProfiles;
        }

        /***************************************************/
    }
}

