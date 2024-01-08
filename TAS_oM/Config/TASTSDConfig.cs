using BH.oM.Adapter;
using BH.oM.Environment.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.TAS
{
    public class TASTSDConfig : ActionConfig, ITASConfig
    {
        //TASTSD properties
        public virtual FileSettings TSDFile { get; set; } = null;
        public TSDResultType ResultQuery { get; set; } = TSDResultType.Simulation;
        public SimulationResultType SimulationType { get; set; } = SimulationResultType.BuildingResult;
        public ProfileResultUnit ResultUnit { get; set; } = ProfileResultUnit.Yearly;
        public ProfileResultType ResultType { get; set; } = ProfileResultType.TemperatureExternal;
        public int Hour { get; set; } = -1;
        public int Day { get; set; } = -1;

        //TASSettings properties
        public virtual double DistanceTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double PlanarTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double MinimumSegmentLength { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double AngleTolerance { get; set; } = BH.oM.Geometry.Tolerance.Angle;
    }
}
