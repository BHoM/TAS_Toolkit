using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHS = BH.oM.Structural;
using BH.oM.Environmental.Elements;
using BH.oM.Environmental.Properties;
using BH.oM.Environmental.Interface;
using BHG = BH.oM.Geometry;
using BH.Engine;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IObject> Read(Type type, IList indices = null)
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
            //else if (typeof(IMaterial).IsAssignableFrom(type))
            //    return ReadMaterials();
            else if (type == typeof(OpaqueMaterial) || type == typeof(TransparentMaterial) || type == typeof(GasMaterial))
                return ReadMaterials();
            else if (type == typeof(BHS.Elements.Storey))
                return ReadStorey();
            else if (type == typeof(ConstructionLayer))
                return ReadConstructionLayer();
            else
                return null;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Space> ReadZones(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;

            List<Space> bHoMSpace = new List<Space>();

            int zoneIndex = 0;
            while (building.GetZone(zoneIndex) != null)
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
            List<Building> bHoMBuilding = new List<Building>();
            bHoMBuilding.Add(Engine.TAS.Convert.ToBHoM(building));
  
            return bHoMBuilding;
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

                    int roomSurfaceIndex = 0;
                    while (zonesurface.GetRoomSurface(roomSurfaceIndex) != null)
                    {
                        TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(roomSurfaceIndex);

                        if (currRoomSrf.GetPerimeter() != null) //sometimes we can have a srf object in tas without a geometry
                            bHoMPanels.Add(Engine.TAS.Convert.ToBHoM(currRoomSrf));

                        roomSurfaceIndex++;
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

            int buildingElementIndex = 0;
            while (building.GetBuildingElement(buildingElementIndex) != null)
            {
                TBD.buildingElement tasBuildingElement = m_TBDDocumentInstance.Building.GetBuildingElement(buildingElementIndex);
                BHoMBuildingElement.Add(Engine.TAS.Convert.ToBHoM(tasBuildingElement));
                buildingElementIndex++;
            }

            return BHoMBuildingElement;
        }
        
        /***************************************************/

        public List<BuildingElementProperties> ReadBuildingElementsProperties(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;

            List<BuildingElementProperties> bHoMBuildingElementProperties = new List<BuildingElementProperties>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {
               
                TBD.Construction construction = m_TBDDocumentInstance.Building.GetConstruction(buildingElementIndex);
                bHoMBuildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(construction));
                buildingElementIndex++;
                
            }

            return bHoMBuildingElementProperties;
        }

        /***************************************************/

        public List<ConstructionLayer> ReadConstructionLayer(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;

            List<BuildingElementProperties> bHoMBuildingElementProperties = new List<BuildingElementProperties>();
            List<ConstructionLayer> bHoMConstructionLayer = new List<ConstructionLayer>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {

                TBD.Construction construction = m_TBDDocumentInstance.Building.GetConstruction(buildingElementIndex);

                int MaterialIndex = 1; // TAS doesn't have any material at index 0
                while (construction.materials(MaterialIndex) != null)
                {
                    TBD.material tasMaterial = m_TBDDocumentInstance.Building.GetConstruction(buildingElementIndex).materials(MaterialIndex);
                    bHoMConstructionLayer.Add(Engine.TAS.Convert.ToBHoM(construction, tasMaterial));
                    MaterialIndex++;
                }

                buildingElementIndex++;

            }

            return bHoMConstructionLayer;
        }

        /***************************************************/

        private List<BHS.Elements.Storey> ReadStorey(List<string> ids = null)
        {
            TBD.BuildingStorey tasStorey = m_TBDDocumentInstance.Building.GetStorey(0);
            List<BHS.Elements.Storey> bHoMStorey = new List<BHS.Elements.Storey>();
            bHoMStorey.Add(Engine.TAS.Convert.ToBHoM(tasStorey));

            return bHoMStorey;
        }

        /***************************************************/

        private List<BHE.Interface.IMaterial> ReadMaterials(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;
           
            List<BHE.Interface.IMaterial> bHoMMaterial = new List<BHE.Interface.IMaterial>();

            int constructionIndex = 0;
            while (building.GetConstruction(constructionIndex) != null)
            {
                              
                TBD.Construction currConstruction = building.GetConstruction(constructionIndex);
                               
                int materialIndex = 1; //TAS does not have any material at index 0
                while (building.GetConstruction(constructionIndex).materials(materialIndex) != null)
                {
                    TBD.material tasMaterial = building.GetConstruction(constructionIndex).materials(materialIndex);

                    bHoMMaterial.Add(Engine.TAS.Convert.ToBHoM(tasMaterial));
                    materialIndex++;
                }       
              
                constructionIndex++;
            }
            return bHoMMaterial;
        }

        /***************************************************/
    }
}
