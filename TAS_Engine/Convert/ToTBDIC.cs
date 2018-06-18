using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using BHEE = BH.oM.Environmental.Elements;
using BH.Engine.Geometry;
using TBD;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        public static string ToTBDIC(this BHE.Elements.Space space)
        {
            if (space == null)
                return "";
            
            if (space.CustomData.ContainsKey("SAM_IC_ThermalTemplate"))
                return space.CustomData["SAM_IC_ThermalTemplate"].ToString();

            return "";
        }
    }
}
