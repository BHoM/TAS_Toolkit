using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHG = BH.oM.Geometry;
using TBD;

namespace BH.Adapter.TAS
{
    public static partial class Convert
    {

        ///***************************************/
        ////Object Converters
        ///***************************************/

        public static BHE.Elements.Location ToBHoM(TBD.Building ITasBuilding)
        {
            BHE.Elements.Location BHoMLocation = new BHE.Elements.Location();
            BHoMLocation.Latitude = ITasBuilding.latitude;
            BHoMLocation.Longitude = ITasBuilding.longitude;
            return BHoMLocation;
        }
               
        /***************************************/

        public static BHE.Elements.Space ToBHoM(TBD.zone ITasZone)
        {
            BHE.Elements.Space BHoMSpace = new BHE.Elements.Space();
            BHoMSpace.Name = ITasZone.name;
            return BHoMSpace;
        }

        /***************************************/

        public static BHE.Elements.Panel ToBHoM(TBD.zoneSurface ITasSurface, BHG.Polyline edges)
        {
            BHE.Elements.Panel BHoMPanel = new BHE.Elements.Panel();
            BHoMPanel.Area = ITasSurface.area;
            BHoMPanel.Type = ITasSurface.type.ToString();
            BHoMPanel.Edges = edges;
                        
            return BHoMPanel;
                       
        }

        
        /***************************************/
        //Geometry Converters
        /***************************************/

        public static BHG.Point ToBHoM(TBD.TasPoint TASPoint)
        {

            BHG.Point BHoMPoint = new BHG.Point();
            BHoMPoint.X = TASPoint.x;
            BHoMPoint.Y = TASPoint.y;
            BHoMPoint.Z = TASPoint.z;
            return BHoMPoint;
        }

        //***************************************/

        public static BHG.Polyline ToBHoM(TBD.Polygon ITasPolygon)
        {

            BHG.Polyline Edges = new BHG.Polyline();
            List<BHG.Point> BHoMPointList = new List<BHG.Point>();

            int pointIndex = 0;
            while (ITasPolygon.GetPoint(pointIndex) != null)
            {
                TasPoint TasControlPoint = ITasPolygon.GetPoint(pointIndex);
                BHG.Point BHoMPoint = ToBHoM(TasControlPoint);
                                
                BHoMPointList.Add(BHoMPoint);
                pointIndex++;
                                
            }

            Edges.ControlPoints = BHoMPointList;
            
            return Edges;
        }

        //***************************************/



    }
}
