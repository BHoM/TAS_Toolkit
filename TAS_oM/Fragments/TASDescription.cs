using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.TAS.Fragments
{
    public class TASDescription : IFragment
    {
        public virtual string Description { get; set; } = "";
        public virtual string TASID { get; set; } = "";
    }
}
