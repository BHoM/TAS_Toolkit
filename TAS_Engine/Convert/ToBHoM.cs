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

        public static BHE.Properties.BuildingElementProperties ToBHoM(TBD.Building ITasBuilding, int BuildingElementIndex)
        {
            BHE.Properties.BuildingElementProperties BHoMBuildingElementProperties = new BHE.Properties.BuildingElementProperties()
            {
                Name = ITasBuilding.GetBuildingElement(BuildingElementIndex).name,
                Thickness = ITasBuilding.GetBuildingElement(BuildingElementIndex).width,
                //gValue = (double)ITasBuilding.GetBuildingElement(BuildingElementIndex).GetConstruction().GetGlazingValues(),
                //LtValue = ITasBuilding.GetBuildingElement(BuildingElementIndex).GetConstruction().lightTransmittance,
                //ThermalConductivity = ITasBuilding.GetBuildingElement(BuildingElementIndex).GetConstruction().conductance,
                //UValue = (double)ITasBuilding.GetBuildingElement(BuildingElementIndex).GetConstruction().GetUValue(),
            };
            return BHoMBuildingElementProperties;
        }

        /***************************************************/

        public static BHE.Properties.BuildingElementProperties ToBHoM(TBD.zoneSurface ITasSurface)
        {
            BHE.Properties.BuildingElementProperties BHoMBuildingElement = new BHE.Properties.BuildingElementProperties()
            {
                Thickness = ITasSurface.buildingElement.width,
                Name = ITasSurface.buildingElement.name
            };
            return BHoMBuildingElement;
        }

        /***************************************************/

        public static BHS.Elements.Storey ToBHoM(TBD.BuildingStorey TasStorey)
        {
            BHS.Elements.Storey BHoMStorey = new BHS.Elements.Storey()
            {

            };
            return BHoMStorey;
        }

        /***************************************************/

        public static BHE.Elements.Building ToBHoM(this TBD.Building tasBuilding)
        {
            BHE.Elements.Building bHoMBuilding = new BHE.Elements.Building()
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

        // A TAS object must correspond just to one BHoMObject. If you feel things don't work and a duplicate appears plausible, the problem is in the BHoM, not in the converter
        public static BHE.Elements.BuildingElementPanel ToBHoM(TBD.zoneSurface ITasSurface, BHG.PolyCurve edges)
        {
            BHE.Elements.BuildingElementPanel bHoMPanel = new BHE.Elements.BuildingElementPanel();
                   
            bHoMPanel.PolyCurve = edges;

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
