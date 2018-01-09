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

        public static BHE.Elements.Location ToBHoM(float latitude, float longitude)
        {
            BHE.Elements.Location BHoMLocation = new BHE.Elements.Location();
            BHoMLocation.Latitude = latitude;
            BHoMLocation.Longitude = longitude;
            return BHoMLocation;
        }


        /***************************************/
        public static BHE.Elements.BuildingElement ToBHoM(TBD.Building ITasBuilding, int BuildingElementIndex)
        {
                BHE.Elements.BuildingElement BHoMBuildingElement = new BHE.Elements.BuildingElement();
                BHoMBuildingElement.BEType = ITasBuilding.GetBuildingElement(BuildingElementIndex).BEType;
                BHoMBuildingElement.Name = ITasBuilding.GetBuildingElement(BuildingElementIndex).name;
                BHoMBuildingElement.Ghost = ITasBuilding.GetBuildingElement(BuildingElementIndex).ghost;
                BHoMBuildingElement.Width = ITasBuilding.GetBuildingElement(BuildingElementIndex).width;
                BHoMBuildingElement.Ground = ITasBuilding.GetBuildingElement(BuildingElementIndex).ground;
               
                

            return BHoMBuildingElement;
        }

        public static BHE.Elements.BuildingElement ToBHoM(TBD.zoneSurface ITasSurface)
        {
            BHE.Elements.BuildingElement BHoMBuildingElement = new BHE.Elements.BuildingElement();
            BHoMBuildingElement.BEType = ITasSurface.buildingElement.BEType;
            BHoMBuildingElement.Name = ITasSurface.buildingElement.name;

            return BHoMBuildingElement;
        }


        /***************************************/

        public static BHE.Elements.Space ToBHoM(TBD.zone ITasZone)
        {
            BHE.Elements.Space BHoMSpace = new BHE.Elements.Space();
            //BHoMSpace.Name = ITasZone.name;
            return BHoMSpace;
        }

        /***************************************/

        public static BHE.Elements.Panel ToBHoM(TBD.zoneSurface ITasSurface, BHG.Polyline edges)
        {
            BHE.Elements.Panel BHoMPanel = new BHE.Elements.Panel();
            BHoMPanel.Edges = edges;
            BHE.Elements.BuildingElement BHoMBuildingElement= ToBHoM(ITasSurface);
            BHoMPanel.BuildingElements.Name = BHoMBuildingElement.Name;
                        
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
            //Returns a closed polyline
            List<BHG.Point> BHoMPointList = new List<BHG.Point>();

            int pointIndex = 0;
            while (ITasPolygon.GetPoint(pointIndex) != null)
            {
                TasPoint TasControlPoint = ITasPolygon.GetPoint(pointIndex);
                BHG.Point BHoMPoint = ToBHoM(TasControlPoint);
                                
                BHoMPointList.Add(BHoMPoint);
                pointIndex++;
                                
            }

            BHoMPointList.Add(BHoMPointList[0]);
            BHG.Polyline Edges = new BHG.Polyline(BHoMPointList);
                                   
            return Edges;
        }

        //***************************************/



    }
}
