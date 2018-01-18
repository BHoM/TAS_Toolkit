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
            else if (type == typeof(BuildingElement))
                return ReadBuildingElements();
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
                         bHoMPanels.Add(Engine.TAS.Convert.ToBHoM(zonesurface));
                    }

                    catch (NullReferenceException e) //If we have air walls we will get a NullReferenceException. Tas does not count air walls as surfaces
                    {
                        bHoMPanels.Add(null);
                        Console.WriteLine(e);
                    }
                    panelIndex++;
                }
                zoneIndex++;
            }
            return bHoMPanels;
        }
        /***************************************************/

        public List<BuildingElement> ReadBuildingElements(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;

            List<BuildingElement> BHoMBuildingElement = new List<BuildingElement>();

            int BuildingElementIndex = 0;
            while (building.GetBuildingElement(BuildingElementIndex) != null)
            {
                TBD.buildingElement tasBuildingElement = m_TBDDocumentInstance.Building.GetBuildingElement(BuildingElementIndex);
                BHoMBuildingElement.Add(Engine.TAS.Convert.ToBHoM(tasBuildingElement));
                BuildingElementIndex++;
            }

            return BHoMBuildingElement;
        }

        /***************************************************/

        //private List<BuildingElement> ReadBuildingElements(List<string> ids = null)
        //{
        //    List<BuildingElement> bHoMBuildingElementList = new List<BuildingElement>();

        //    int zoneIndex = 0;
        //    while (m_TBDDocumentInstance.Building.GetZone(zoneIndex) != null)
        //    {
        //        int buildingElementIndex = 0;
        //        while (m_TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(buildingElementIndex) != null)
        //        {
        //            TBD.buildingElement buildingelement = m_TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(buildingElementIndex).buildingElement;
        //            bHoMBuildingElementList.Add(Engine.TAS.Convert.ToBHoM(buildingelement));
        //            buildingElementIndex++;
        //        }
        //        zoneIndex++;
        //    }
        //    return bHoMBuildingElementList;
        //}



        /***************************************************/

        public List<BuildingElementProperties> ReadBuildingElementsProperties(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;

            List<BuildingElementProperties> BHoMBuildingElementProperties = new List<BuildingElementProperties>();

            int BuildingElementIndex = 0;
            while (building.GetConstruction(BuildingElementIndex) != null)
            {
                //TBD.Construction construction = m_TBDDocumentInstance.Building.GetBuildingElement(BuildingElementIndex).GetConstruction();
                TBD.Construction construction = m_TBDDocumentInstance.Building.GetConstruction(BuildingElementIndex);
                BHoMBuildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(construction));
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
