using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using BH.Engine.Geometry;
using TBD;
using TSD;
using TPD;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/

        //public static TAS3D.Building ToTas(BHE.Elements_Legacy.Location bHoMLocation) // Are you sure it is not better to return the Building interface? It seems like Tas works with interfaces. It for sure need a deeper look 
        //{
        //    TAS3D.Building tasLocation = new TAS3D.Building();
        //    tasLocation.latitude = (float)bHoMLocation.Latitude;
        //    tasLocation.longitude = (float)bHoMLocation.Longitude;
        //    return tasLocation;
        //}


        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static TBD.TasPoint ToTas(this BHG.Point bHoMPoint)
        {
            TBD.TasPoint tasPoint = new TBD.TasPoint();
            tasPoint.x = (float)(bHoMPoint.X);
            tasPoint.y = (float)(bHoMPoint.Y);
            tasPoint.z = (float)(bHoMPoint.Z);
            return tasPoint;
        }

        /***************************************************/

        public static TBD.Polygon ToTas(BHG.Polyline bHoMPolyline)
        {

            TBD.Polygon tasPolygon = new TBD.Polygon();
            List<BHG.Point> coordList = bHoMPolyline.ControlPoints;

            for (int i = 0; i < coordList.Count; i++)
            {
                tasPolygon.AddCoordinate((float)coordList[i].X, (float)coordList[i].Y, (float)coordList[i].Z);
            }

            return tasPolygon;
        }

        /***************************************************/

        public static TBD.zoneSurface ToTas(BHE.Elements.BuildingElementPanel bHoMPanel)
        {
            TBD.zoneSurface tasSurface = new TBD.zoneSurface();
            //add points as properties to the surface
            //tasSurface.area = (float)bHoMPanel.Surface.IArea();
            return tasSurface;
        }


        /***************************************************/
    }
}
