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

        public static List<BH.oM.Environment.Elements.Profile> Profiles(TBD.Thermostat tbdThermostat)
        {
            List<BH.oM.Environment.Elements.Profile> bHoMProfiles = new List<BH.oM.Environment.Elements.Profile>();

            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticUL).ToBHoMProfile(BHEE.ProfileCategory.Thermostat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticUL).ToBHoMProfile(BHEE.ProfileCategory.Thermostat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticHUL).ToBHoMProfile(BHEE.ProfileCategory.Humidistat));
            bHoMProfiles.Add(tbdThermostat.GetProfile((int)TBD.Profiles.ticHLL).ToBHoMProfile(BHEE.ProfileCategory.Humidistat));

            return bHoMProfiles;
        }

        /***************************************************/

        public static List<BH.oM.Environment.Elements.Profile> Profiles(TBD.InternalGain tbdInternalGain)
        {
            List<BH.oM.Environment.Elements.Profile> bHoMProfiles = new List<BH.oM.Environment.Elements.Profile>();

            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticI).ToBHoMProfile(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticV).ToBHoMProfile(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticLG).ToBHoMProfile(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticOSG).ToBHoMProfile(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticOLG).ToBHoMProfile(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticESG).ToBHoMProfile(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticELG).ToBHoMProfile(BHEE.ProfileCategory.Gain));
            bHoMProfiles.Add(tbdInternalGain.GetProfile((int)TBD.Profiles.ticCOG).ToBHoMProfile(BHEE.ProfileCategory.Gain));


            return bHoMProfiles;
        }

        /***************************************************/
    }
}
