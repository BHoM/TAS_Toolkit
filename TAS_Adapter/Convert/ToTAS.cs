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

        ///***************************************/
        ////Object Converters
        ///***************************************/
        
        public static TAS3D.BuildingClass ToTas(BHE.Elements.Location BHoMLocation)
        {
            //TBD.BuildingClass TasLocation = new TBD.BuildingClass();
            TAS3D.BuildingClass TasLocation = new TAS3D.BuildingClass();
            TasLocation.latitude = (float)BHoMLocation.Latitude;
            TasLocation.longitude = (float)BHoMLocation.Longitude;
            //TasLocation.maxBuildingAltitude = (float)BHoMLocation.Elevation;
            return TasLocation;
            
        }

        
        ///***************************************/
        ////Geometry Converters
        ///***************************************/

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

        public static TBD.zoneSurfaceClass ToTas(BHE.Elements.Panel BHoMPanel)
        {
            TBD.zoneSurfaceClass TasSurface = new TBD.zoneSurfaceClass();
            //add points as properties to the surface
            TasSurface.area = (float)BHoMPanel.Area;
            return TasSurface;
        }
               


        /***************************************/
        //Property converter
        /***************************************/

        /***************************************/
        //List Converter
        /***************************************/


    }
}
