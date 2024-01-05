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
        private static void ClearCOMObject(object obj)
        {
            if (obj == null) return;
            int intrefcount;
            do
            {
                intrefcount = Marshal.FinalReleaseComObject(obj);
            } while (intrefcount > 0);
        }
    }
}
