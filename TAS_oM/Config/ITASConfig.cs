using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.TAS
{
    public interface ITASConfig
    {
        double DistanceTolerance { get; set; }
        double PlanarTolerance { get; set; }
        double MinimumSegmentLength { get; set; }
        double AngleTolerance { get; set; }
    }
}
