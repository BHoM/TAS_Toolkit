using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHS = BH.oM.Structural;
using BH.oM.Environmental.Elements;
using BH.oM.Environmental.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<BHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(BuildingElementPanel))
                return ReadPanels();
            else if (type == typeof(Building))
                return ReadBuilding();
            else if (type == typeof(Space))
                return ReadZones();
            else if (type == typeof(BuildingElementProperties))
                return ReadBuildingElementsProperties();
            else if (type == typeof(BHS.Elements.Storey))
                return ReadStorey();
            else
                return null;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Space> ReadZones(List<string> ids = null)
        {
            List<Space> bHoMSpace = new List<Space>();

            int zoneIndex = 0;
            while (m_TBDDocumentInstance.Building.GetZone(zoneIndex) != null)
            {
                TBD.zone zone = m_TBDDocumentInstance.Building.GetZone(zoneIndex);
                bHoMSpace.Add(Engine.TAS.Convert.ToBHoM(zone));
                zoneIndex++;
            }                
                       
            return bHoMSpace;
        }

        /***************************************************/

        private List<Building> ReadBuilding(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;
            List<Building> BHoMBuilding = new List<Building>();
            BHoMBuilding.Add(Engine.TAS.Convert.ToBHoM(building));

            return BHoMBuilding;
        }


        /***************************************************/

        private List<BuildingElementPanel> ReadPanels(List<string> ids = null)
        {
            
            List<BuildingElementPanel> bHoMPanels = new List<BuildingElementPanel>();

            int zoneIndex = 0;
            while (m_TBDDocumentInstance.Building.GetZone(zoneIndex) != null)
            {
                int panelIndex = 0;
                while (m_TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(panelIndex) != null)
                {
                    TBD.zoneSurface zonesurface = m_TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(panelIndex);
                  
                    try
                    {
                        //Get edges as polylines for the Tas Surfaces
                        TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(0);
                        TBD.Perimeter currPerimeter = currRoomSrf.GetPerimeter();
                        TBD.Polygon currPolygon = currPerimeter.GetFace();
                                                            
                        BHG.Polyline edges = Engine.TAS.Convert.ToBHoM(currPolygon);
                        BHG.PolyCurve crv_edges = BH.Engine.Geometry.Create.PolyCurve(new List<BHG.Polyline> { edges }); //Can I solve this ina better way??

                        bHoMPanels.Add(Engine.TAS.Convert.ToBHoM(zonesurface, crv_edges));
                    }

                    //If we have air walls we will get a NullReferenceException. Tas does not count air walls as surfaces 
                    catch (NullReferenceException e)
                    {
                        int error = panelIndex;
                        Console.WriteLine(e);
                        //throw e;
                                               
                    }                                                     
                    panelIndex++;
                }
                zoneIndex++;                
            }
            return bHoMPanels;
        }

        /***************************************************/

        public List<BuildingElementProperties> ReadBuildingElementsProperties(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;
            List<BuildingElementProperties> BHoMBuildingElementProperties = new List<BuildingElementProperties>();

            int BuildingElementIndex = 0;
            while (building.GetBuildingElement(BuildingElementIndex) != null)
            {

                BHoMBuildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(building, BuildingElementIndex));
                BuildingElementIndex++;

            }

            return BHoMBuildingElementProperties;
        }

        /***************************************************/

        private List<BHS.Elements.Storey> ReadStorey(List<string> ids = null)
        {
            TBD.BuildingStorey tasStorey = m_TBDDocumentInstance.Building.GetStorey(0);
            List<BHS.Elements.Storey> BHoMStorey = new List<BHS.Elements.Storey>();
            BHoMStorey.Add(Engine.TAS.Convert.ToBHoM(tasStorey));

            return BHoMStorey;
        }

    }
}
