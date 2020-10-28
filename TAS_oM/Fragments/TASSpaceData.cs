using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.TAS.Fragments
{
    public class TASSpaceData : IFragment
    {
        public virtual uint Colour { get; set; } = 0;
        public virtual double DaylightFactor { get; set; } = 0.0;
        public virtual double ExposedPerimeter { get; set; } = 0.0;
        public virtual int External { get; set; } = 0;
        public virtual double FacadeLength { get; set; } = 0.0;
        public virtual double FixedConvectionCoefficient { get; set; } = 0.0;
        public virtual double FloorArea { get; set; } = 0.0;
        public virtual string TASID { get; set; } = "";
        public virtual double Length { get; set; } = 0.0;
        public virtual double SizeCooling { get; set; } = 0.0;
        public virtual double SizeHeating { get; set; } = 0.0;
        public virtual double Volume { get; set; } = 0.0;
        public virtual double WallFloorAreaRatio { get; set; } = 0.0;
    }
}
