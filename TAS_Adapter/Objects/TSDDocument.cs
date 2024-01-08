using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public class TSDDocument : ITASFile
    {
        public virtual TSD.TSDDocument Document { get; set; } = null;
    }
}
