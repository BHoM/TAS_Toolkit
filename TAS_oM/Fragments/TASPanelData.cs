using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.TAS.Fragments
{
    public class TASPanelData : IFragment
    {
        public virtual string TASID { get; set; } = "";
        public virtual string TASName { get; set; } = "";
        public virtual string Type { get; set; } = "";
        public virtual double Area { get; set; } = 0;
        public virtual double InternalArea { get; set; } = 0;
        public virtual double Width { get; set; } =0;
        public virtual double MaterialLayersThickness { get; set; } = 0;
        public virtual bool PanelIsOpening { get; set; } = false;
        public virtual bool OpeningIsFrame { get; set; } = false;

    }
}
