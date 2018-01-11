using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using BH.Engine.Geometry;
using TAS3D;
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

        public static TAS3D.BuildingClass ToTas(BHE.Elements.Location bHoMLocation) // Are you sure it is not better to return the Building interface? It seems like Tas works with interfaces. It for sure need a deeper look 
        {
            TAS3D.BuildingClass tasLocation = new TAS3D.BuildingClass();
            tasLocation.latitude = (float)bHoMLocation.Latitude;
            tasLocation.longitude = (float)bHoMLocation.Longitude;
            return tasLocation;
        }


        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static TBD.TasPointClass ToTas(this BHG.Point bHoMPoint)
        {
            TBD.TasPointClass tasPoint = new TBD.TasPointClass();
            tasPoint.x = (float)(bHoMPoint.X);
            tasPoint.y = (float)(bHoMPoint.Y);
            tasPoint.z = (float)(bHoMPoint.Z);
            return tasPoint;
        }

        /***************************************************/

        public static TBD.PolygonClass ToTas(BHG.Polyline bHoMPolyline)
        {

            TBD.PolygonClass tasPolygon = new TBD.PolygonClass();
            List<BHG.Point> coordList = bHoMPolyline.ControlPoints;

            for (int i = 0; i < coordList.Count; i++)
            {
                tasPolygon.AddCoordinate((float)coordList[i].X, (float)coordList[i].Y, (float)coordList[i].Z);
            }

            return tasPolygon;
        }

        /***************************************************/

        public static TBD.zoneSurfaceClass ToTas(BHE.Elements.Panel bHoMPanel)
        {
            TBD.zoneSurfaceClass tasSurface = new TBD.zoneSurfaceClass();
            //add points as properties to the surface
            tasSurface.area = (float)bHoMPanel.Surface.IArea();
            return tasSurface;
        }


        /***************************************************/
    }
}
