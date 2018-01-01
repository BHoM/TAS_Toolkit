using System.Collections.Generic;
using BHE = BH.oM.Environmental;
using BHG = BH.oM.Geometry;
using TBD;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/

        public static BHE.Elements.Location ToBHoM(this TBD.Building tasBuilding)
        {
            BHE.Elements.Location bHoMLocation = new BHE.Elements.Location();
            bHoMLocation.Latitude = tasBuilding.latitude;
            bHoMLocation.Longitude = tasBuilding.longitude;
            return bHoMLocation;
        }

        /***************************************************/

        public static BHE.Elements.Space ToBHoM(this TBD.zone tasZone)
        {
            BHE.Elements.Space bHoMSpace = new BHE.Elements.Space();
            bHoMSpace.Name = tasZone.name;
            return bHoMSpace;
        }

        /***************************************************/

        public static BHE.Elements.Panel ToBHoM(this TBD.zoneSurface tasSurface, BHG.Polyline edges)
        {
            BHE.Elements.Panel bHoMPanel = new BHE.Elements.Panel();
           // bHoMPanel.Area = tasSurface.area;         //TODO: This doesn't make sense. The surface of the panel needs to be defined instead
            //BHoMPanel.Type = ITasSurface.type.ToString();
            //bHoMPanel.Edges = edges;
                        
            return bHoMPanel;
                       
        }


        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this TBD.TasPoint tASPoint)
        {
            BHG.Point bHoMPoint = new BHG.Point();
            bHoMPoint.X = tASPoint.x;
            bHoMPoint.Y = tASPoint.y;
            bHoMPoint.Z = tASPoint.z;
            return bHoMPoint;
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this TBD.Polygon tasPolygon)
        {
            //Returns a closed polyline


            List<BHG.Point> bHoMPointList = new List<BHG.Point>();

            int pointIndex = 0;
            while (tasPolygon.GetPoint(pointIndex) != null)
            {
                TasPoint TasControlPoint = tasPolygon.GetPoint(pointIndex);
                BHG.Point BHoMPoint = ToBHoM(TasControlPoint);

                bHoMPointList.Add(BHoMPoint);
                pointIndex++;

            }

            bHoMPointList.Add(bHoMPointList[0]);
            BHG.Polyline edges = new BHG.Polyline { ControlPoints = bHoMPointList };
                                   
            return edges;
        }

        /***************************************************/
    }
}
