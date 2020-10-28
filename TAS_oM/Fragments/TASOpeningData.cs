using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.TAS.Fragments
{
    public class TASOpeningData : IFragment
    {
        public virtual string ParentGUID { get; set; } = "";
        public virtual string ParentName { get; set; } = "";
    }
}
