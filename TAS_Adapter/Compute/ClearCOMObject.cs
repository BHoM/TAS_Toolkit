using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        public static void ClearCOMObject(object obj)
        {
            if (obj == null)
                return;
            
            int refCount = 1;
            
            do
            {
                refCount = Marshal.FinalReleaseComObject(obj);
            }
            while (refCount > 0);
        }
    }
}
