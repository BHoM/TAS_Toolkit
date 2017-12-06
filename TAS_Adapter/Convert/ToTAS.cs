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
using TSD;
using TasConv;
using TPD;
using EDSL;

namespace BH.Adapter.TAS
{
    public static partial class Convert
    {
        
        /***************************************/
        //Geometry Converters
        /***************************************/

        public static TBD.TasPointClass FromBHoMGeometry(BHG.Point BHoMPoint)
        {
            TBD.TasPointClass TasPoint = new TBD.TasPointClass();
            TasPoint.x = (BHoMPoint.X as dynamic);
            TasPoint.y = (BHoMPoint.Y as dynamic);
            TasPoint.z = (BHoMPoint.Z as dynamic);
            return TasPoint;
        }

        
        /***************************************/
        //Object Converters
        /***************************************/

        
                           
        public static TasPoint getCoord(TBD.zoneSurface srf)
        {
            TBD.zoneSurface TasSrf = new TBD.zoneSurface();
            TBD.Perimeter TasPerim = new TBD.Perimeter();
            TBD.Polygon TasPolygon = new TBD.Polygon();

            TasSrf.GetRoomSurface(1);
            TasPerim.GetFace();
            TasPolygon.GetPoint(1);

            return TasPolygon.GetPoint(1);
        }
          
      
        /***************************************/
        //Property converter
        /***************************************/

        /***************************************/
        //List Converter
        /***************************************/


    }
}
