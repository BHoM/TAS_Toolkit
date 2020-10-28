using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
namespace BH.oM.Adapters.TAS.Fragments
{
    public class TASInternalGainData : IFragment
    {
        public virtual int ActivityID { get; set; } = 0;
        public virtual double DomesticHotWater { get; set; } = 0;
        public virtual double TargetIlluminance { get; set; } = 0;
    }
}
