using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environmental;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        protected override IEnumerable<BHoMObject> Read(Type type, IList ids)
        {
            throw new NotImplementedException();
        }
    }
}
