using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public class TBDDocument : ITASFile
    {
        public virtual TBD.TBDDocument Document { get; set; } = new TBD.TBDDocument();
    }
}
