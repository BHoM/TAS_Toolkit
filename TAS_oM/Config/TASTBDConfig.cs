using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.TAS
{
    public class TASTBDConfig : ActionConfig, ITASConfig
    {
        //TODO - fill with all inputs to the TASTBDAdapter
        public virtual FileSettings TBDFile { get; set; } = null;
        public virtual double DistanceTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double PlanarTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double MinimumSegmentLength { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double AngleTolerance { get; set; } = BH.oM.Geometry.Tolerance.Angle;
    }
}
