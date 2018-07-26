using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BHS = BH.oM.Structural;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Interface;
using BHG = BH.oM.Geometry;
using BH.Engine;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(BuildingElementPanel))
                return ReadPanels();
            else if (type == typeof(Building))
                return ReadBuilding();
            else if (type == typeof(Space))
                return ReadSpaces();
            else if (type == typeof(BuildingElement))
                return ReadBuildingElements();
            else if (type == typeof(BuildingElementProperties))
                return ReadBuildingElementsProperties();
            //else if (typeof(IMaterial).IsAssignableFrom(type))
            //    return ReadMaterials();
            else if (type == typeof(OpaqueMaterial) || type == typeof(TransparentMaterial) || type == typeof(GasMaterial))
                return ReadMaterials();
            //else if (type == typeof(BHS.Elements.Storey))
            //    return ReadStorey();
            else if (type == typeof(ConstructionLayer))
                return ReadConstructionLayer();
            else
                return null;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Space> ReadSpaces(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocument.Building;

            List<Space> bHoMSpace = new List<Space>();

            int zoneIndex = 0;
            while (building.GetZone(zoneIndex) != null)
            {
                TBD.zone zone = m_TBDDocument.Building.GetZone(zoneIndex);
                bHoMSpace.Add(Engine.TAS.Convert.ToBHoM(zone));
                zoneIndex++;
            }                
                       
            return bHoMSpace;
        }

        /***************************************************/

        private List<Building> ReadBuilding(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocument.Building;
            List<Building> bHoMBuilding = new List<Building>();
            bHoMBuilding.Add(Engine.TAS.Convert.ToBHoM(building));
  
            return bHoMBuilding;
        }

        /***************************************************/

        private List<BuildingElementPanel> ReadPanels(List<string> ids = null)
        {

            List<BuildingElementPanel> bHoMPanels = new List<BuildingElementPanel>();

            int zoneIndex = 0;
            while (m_TBDDocument.Building.GetZone(zoneIndex) != null)
            {
                int panelIndex = 0;
                while (m_TBDDocument.Building.GetZone(zoneIndex).GetSurface(panelIndex) != null)
                {
                    TBD.zoneSurface zonesurface = m_TBDDocument.Building.GetZone(zoneIndex).GetSurface(panelIndex);

                    int roomSurfaceIndex = 0;
                    while (zonesurface.GetRoomSurface(roomSurfaceIndex) != null)
                    {
                        TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(roomSurfaceIndex);

                        if (currRoomSrf.GetPerimeter() != null)
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
            TBD.Building building = m_TBDDocument.Building;

            List<BuildingElement> BHoMBuildingElements = new List<BuildingElement>();

            int aIndex = 0;
            TBD.zone aZone = building.GetZone(aIndex);
            while(aZone != null)
            {
                int zoneSurfaceIndex = 0;
                while (aZone.GetSurface(zoneSurfaceIndex) != null)
                {
                    int roomSrfIndex = 0;
                    while (aZone.GetSurface(zoneSurfaceIndex).GetRoomSurface(roomSrfIndex) != null)
                    {
                        TBD.RoomSurface tasRoomSrf = aZone.GetSurface(zoneSurfaceIndex).GetRoomSurface(roomSrfIndex);
                        if (tasRoomSrf.GetPerimeter() != null) //sometimes we can have a srf object in tas without a geometry
                        {
                            //BHE.Elements.BuildingElement bHoMBuildingElement = ToBHoM(tasZone.GetSurface(zoneSurfaceIndex).buildingElement);
                            BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties = Engine.TAS.Convert.ToBHoM(aZone.GetSurface(zoneSurfaceIndex).buildingElement);
                            BHE.Elements.BuildingElement bHoMBuildingElement = new BuildingElement()
                            // tasZone.GetSurface(zoneSurfaceIndex).

                            {

                                Name = bHoMBuildingElementProperties.Name,
                                BuildingElementGeometry = Engine.TAS.Convert.ToBHoM(tasRoomSrf),
                                BuildingElementProperties = bHoMBuildingElementProperties
                            };

                            BHoMBuildingElements.Add(bHoMBuildingElement);
                        }
                        roomSrfIndex++;
                    }
                    zoneSurfaceIndex++;
                }

                aIndex++;
                aZone = building.GetZone(aIndex);
            }

            return BHoMBuildingElements;
        }
        
        /***************************************************/

        public List<BuildingElementProperties> ReadBuildingElementsProperties(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocument.Building;

            List<BuildingElementProperties> bHoMBuildingElementProperties = new List<BuildingElementProperties>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {
                TBD.buildingElement buildingElement = m_TBDDocument.Building.GetBuildingElement(buildingElementIndex);
                BuildingElementType aBuildingElementType = Engine.TAS.Convert.ToBHoM((TBD.BuildingElementType)buildingElement.BEType);
                TBD.Construction construction = m_TBDDocument.Building.GetConstruction(buildingElementIndex);
                bHoMBuildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(construction, buildingElement.name, aBuildingElementType));
                buildingElementIndex++;

            }

            return bHoMBuildingElementProperties;
        }

        /***************************************************/

        public List<ConstructionLayer> ReadConstructionLayer(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocument.Building;

            List<BuildingElementProperties> bHoMBuildingElementProperties = new List<BuildingElementProperties>();
            List<ConstructionLayer> bHoMConstructionLayer = new List<ConstructionLayer>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {

                TBD.Construction construction = m_TBDDocument.Building.GetConstruction(buildingElementIndex);

                int MaterialIndex = 1; // TAS doesn't have any material at index 0
                while (construction.materials(MaterialIndex) != null)
                {
                    TBD.material tasMaterial = m_TBDDocument.Building.GetConstruction(buildingElementIndex).materials(MaterialIndex);
                    bHoMConstructionLayer.Add(Engine.TAS.Convert.ToBHoM(construction, tasMaterial));
                    MaterialIndex++;
                }

                buildingElementIndex++;

            }

            return bHoMConstructionLayer;
        }

        /***************************************************/

        //private List<BHS.Elements.Storey> ReadStorey(List<string> ids = null)
        //{
        //    TBD.BuildingStorey tasStorey = m_TBDDocumentInstance.Building.GetStorey(0);
        //    List<BHS.Elements.Storey> bHoMStorey = new List<BHS.Elements.Storey>();
        //    bHoMStorey.Add(Engine.TAS.Convert.ToBHoM(tasStorey));

        //    return bHoMStorey;
        //}

        /***************************************************/

        private List<BHE.Interface.IMaterial> ReadMaterials(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocument.Building;
           
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
