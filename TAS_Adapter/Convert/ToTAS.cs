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
using TPD;

namespace BH.Adapter.TAS
{
    public static partial class Convert
    {   
                       
        /***************************************/
        //Geometry Converters
        /***************************************/

        public static TBD.TasPointClass ToTas(this BHG.Point BHoMPoint)
        {
            TBD.TasPointClass TasPoint = new TBD.TasPointClass();
            TasPoint.x = (float)(BHoMPoint.X);
            TasPoint.y = (float)(BHoMPoint.Y);
            TasPoint.z = (float)(BHoMPoint.Z);
            return TasPoint;
        }

        /***************************************/

        public static TBD.PolygonClass ToTas(BHG.Polyline BHoMPolyline)
        {          
       
            TBD.PolygonClass TasPolygon = new TBD.PolygonClass();
            List<BHG.Point> CoordList = BHoMPolyline.ControlPoints;

            for (int i = 0; i < CoordList.Count; i++)
            {
                 TasPolygon.AddCoordinate((float)CoordList[i].X, (float)CoordList[i].Y, (float)CoordList[i].Z);
            }
               
            return TasPolygon;
        }

        /***************************************/

        //public static TBD.zoneSurfaceClass ToTas(BHE.Elements.Panel BHoMPanel)
        //{

        //}
               


        /***************************************/
        //Property converter
        /***************************************/

        /***************************************/
        //List Converter
        /***************************************/


    }
}
