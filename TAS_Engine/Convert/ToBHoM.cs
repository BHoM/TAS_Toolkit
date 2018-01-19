using System.Collections.Generic;
using BHE = BH.oM.Environmental;
using BHS = BH.oM.Structural;
using BHG = BH.oM.Geometry;
using TBD;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/
        
        public static BHE.Properties.BuildingElementProperties ToBHoM(TBD.buildingElement tasBuildingElement)
        {
            BHE.Properties.BuildingElementProperties BHoMBuildingElementProperties = new BHE.Properties.BuildingElementProperties()
            {
                Name = tasBuildingElement.name,
                Thickness = tasBuildingElement.width
            };
            return BHoMBuildingElementProperties;
        }

        /***************************************************/

        public static BHS.Elements.Storey ToBHoM(TBD.BuildingStorey tasStorey)
        {
            BHS.Elements.Storey BHoMStorey = new BHS.Elements.Storey
            {

            };
            return BHoMStorey;
        }

        /***************************************************/

        public static BHE.Elements.Building ToBHoM(this TBD.Building tasBuilding)
        {
            BHE.Elements.Building bHoMBuilding = new BHE.Elements.Building
            {
                Latitude = tasBuilding.latitude,
                Longitude = tasBuilding.longitude,
                Elevation = tasBuilding.maxBuildingAltitude,
                
            };
            return bHoMBuilding;
        }

        /***************************************************/


        public static BHE.Elements.Space ToBHoM(this TBD.zone tasZone)
        {
            BHE.Elements.Space bHoMSpace = new BHE.Elements.Space();
            bHoMSpace.Name = tasZone.name;
            return bHoMSpace;
        }

        /***************************************************/

       
        public static BHE.Elements.BuildingElementPanel ToBHoM(this TBD.zoneSurface zonesurface)
        {
            BHE.Elements.BuildingElementPanel bHoMPanel = new BHE.Elements.BuildingElementPanel();

            TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(0);
            TBD.Perimeter currPerimeter = currRoomSrf.GetPerimeter();
            TBD.Polygon currPolygon = currPerimeter.GetFace();

            BHG.Polyline edges = ToBHoM(currPolygon);
            BHG.PolyCurve crv_edges = Geometry.Create.PolyCurve(new List<BHG.Polyline> { edges }); //Can I solve this in a better way??

            bHoMPanel.PolyCurve = crv_edges;

            return bHoMPanel;
        }


        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this TBD.TasPoint tasPoint)
        {
            BHG.Point bHoMPoint = new BHG.Point()
            {
                X = tasPoint.x,
                Y = tasPoint.y,
                Z = tasPoint.z
            };
            return bHoMPoint;
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this TBD.Polygon tasPolygon)  // TODO : When BH.oM.Geometry.Contour is implemented, Polyline can be replaced with Contour
        {
            //
            //  Not sure how this is working but that's a very strange way of getting points for Tas. Are you sure it is the only way?
            //
            List<BHG.Point> bHoMPointList = new List<BHG.Point>();
            int pointIndex = 0;
            while (true)
            {
                TasPoint tasPt= tasPolygon.GetPoint(pointIndex);
                if (tasPt == null) { break; }
                bHoMPointList.Add(tasPt.ToBHoM());
                pointIndex++;
            }
            bHoMPointList.Add(bHoMPointList[0]);
            return new BHG.Polyline { ControlPoints = bHoMPointList };
        }

        /***************************************************/
    }
}
