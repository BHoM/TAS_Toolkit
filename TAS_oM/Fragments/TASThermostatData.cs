using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.TAS.Fragments
{
    public class TASThermostatData : IFragment
    {
        public virtual double RadiantProportion { get; set; } = 0.0;
    }
}
