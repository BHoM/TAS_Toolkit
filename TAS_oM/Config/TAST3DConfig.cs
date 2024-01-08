using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.TAS
{
    public class TAST3DConfig : ActionConfig, ITASConfig
    {
        //TAST3D properties
        public virtual FileSettings T3DFile { get; set; } = null;
        public virtual string ProjectFolder { get; set; } = null;
        public virtual FileSettings GBXMLFile { get; set; } = null;
        public virtual bool FixNormals { get; set; } = false;

        //TASSettings properties
        public virtual double DistanceTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double PlanarTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double MinimumSegmentLength { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double AngleTolerance { get; set; } = BH.oM.Geometry.Tolerance.Angle;
    }
}
