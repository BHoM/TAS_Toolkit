using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHG = BH.oM.Geometry;

namespace BH.Adapter.TAS
{
    public static partial class Convert
    {
        /***************************************/
        //Geometry Converters
        /***************************************/

        public static BHG.Point ToBHoMGeometry(TBD.TasPoint TASPoint)
        {

            BHG.Point BHoMPoint = new BHG.Point();
            BHoMPoint.X = (TASPoint.x);
            BHoMPoint.Y = (TASPoint.y);
            BHoMPoint.Z = (TASPoint.z);
            return BHoMPoint;
        }
               
    }
}
