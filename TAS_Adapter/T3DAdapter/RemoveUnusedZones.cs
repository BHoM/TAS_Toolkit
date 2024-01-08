using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS3D;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        public static void RemoveUnusedZones(this T3DDocument document)
        {
            List<TAS3D.Zone> zones = new List<Zone>();
            int index = 1;
            TAS3D.Zone zone = null;
            while ((zone = document.Document.Building.GetZone(index)) != null)
            {
                if (zone.isUsed == 0)
                    zones.Add(zone);
                index++;
            }

            zones.ForEach(x => x.Delete());
        }
    }
}
