using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using TAS3D;
using TBD;

namespace BH.Adapter.TAS
{
    public static partial class Convert
    {
        /***************************************/
        //Object Converters
        /***************************************/
            

        /***************************************/
        //Geometry Converters
        /***************************************/

        public static TBD.TasPoint FromBHoMGeometry(BHG.Point BHoMPoint)
        {
            TBD.TasPoint TASPoint = new TBD.TasPoint();
            TASPoint.x = (BHoMPoint.X as dynamic);
            TASPoint.y = (BHoMPoint.Y as dynamic);
            TASPoint.z = (BHoMPoint.Z as dynamic);
            return TASPoint;
        }
                
        
        /***************************************/
        //Property converter
        /***************************************/

        /***************************************/
        //List Converter
        /***************************************/


    }
}
