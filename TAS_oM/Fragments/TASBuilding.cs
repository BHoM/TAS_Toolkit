using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.TAS.Fragments
{
    public class TASBuilding : IFragment
    {
        public virtual string ID { get; set; } = "";
        public virtual string TASID { get; set; } = "";
        public virtual string PathFile { get; set; } = "";
    }
}
