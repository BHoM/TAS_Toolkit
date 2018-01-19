using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Environmental;

namespace BH.Engine.TAS.Helpers
{
    public static class Helper
    {
        public static BHE.Elements.MaterialType GetMaterialType(this TBD.material tasMaterial)
        {
            if (tasMaterial.type == 1) // 1 = Opaque
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Opaque;
                return materialtype;
            }
            if (tasMaterial.type == 2) // 2 = Opaque
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Opaque;
                return materialtype;
            }
            else if (tasMaterial.type == 3) // 3 = Transparent
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Transparent;
                return materialtype;
            }
            else if (tasMaterial.type == 4) // 4 = Gas
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Gas;
                return materialtype;
            }
            else
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Opaque;
                return materialtype;
            }
          
        }

    }
}
