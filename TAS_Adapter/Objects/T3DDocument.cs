using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public class T3DDocument : ITASFile
    {
        public virtual TAS3D.T3DDocument Document { get; set; } = new TAS3D.T3DDocument();

        public virtual string FilePath { get; set; } = "";
    }
}
