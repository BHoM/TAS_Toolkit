using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environment;
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
            if (type == typeof(BuildingElement))
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
            else if (type == typeof(BH.oM.Environment.Materials.OpaqueMaterial) || type == typeof(BH.oM.Environment.Materials.TransparentMaterial) || type == typeof(BH.oM.Environment.Materials.GasMaterial))
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
            TBD.Building building = tbdDocument.Building;

            List<Space> spaces = new List<Space>();

            int zoneIndex = 0;
            while (building.GetZone(zoneIndex) != null)
            {
                TBD.zone zone = tbdDocument.Building.GetZone(zoneIndex);
                spaces.Add(Engine.TAS.Convert.ToBHoM(zone));
                zoneIndex++;
            }                
                       
            return spaces;
        }

        /***************************************************/

        private List<Building> ReadBuilding(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;
            List<Building> buildings = new List<Building>();
            buildings.Add(Engine.TAS.Convert.ToBHoM(building));
  
            return buildings;
        }

        /***************************************************/

        private List<BuildingElement> ReadPanels(List<string> ids = null)
        {

            List<BuildingElement> panels = new List<BuildingElement>();

            int zoneIndex = 0;
            while (tbdDocument.Building.GetZone(zoneIndex) != null)
            {
                int panelIndex = 0;
                while (tbdDocument.Building.GetZone(zoneIndex).GetSurface(panelIndex) != null)
                {
                    TBD.zoneSurface zonesurface = tbdDocument.Building.GetZone(zoneIndex).GetSurface(panelIndex);

                    int roomSurfaceIndex = 0;
                    while (zonesurface.GetRoomSurface(roomSurfaceIndex) != null)
                    {
                        TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(roomSurfaceIndex);

                        if (currRoomSrf.GetPerimeter() != null)
                            //bHoMPanels.Add(Engine.TAS.Convert.ToBHoM(currRoomSrf));

                        roomSurfaceIndex++;
                    }

                   panelIndex++;
                }
                zoneIndex++;
            }
            return panels;
        }

        /***************************************************/

        public List<BuildingElement> ReadBuildingElements(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;

            List<BuildingElement> buildingElements = new List<BuildingElement>();

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
                            BHE.Properties.BuildingElementProperties buildingElementProperties = Engine.TAS.Convert.ToBHoM(aZone.GetSurface(zoneSurfaceIndex).buildingElement);
                            BHE.Elements.BuildingElement buildingElement = new BuildingElement()
                            // tasZone.GetSurface(zoneSurfaceIndex).

                            {

                                Name = buildingElementProperties.Name,
                                //BuildingElementGeometry = Engine.TAS.Convert.ToBHoM(tasRoomSrf),
                                BuildingElementProperties = buildingElementProperties
                            };

                            buildingElements.Add(buildingElement);
                        }
                        roomSrfIndex++;
                    }
                    zoneSurfaceIndex++;
                }

                aIndex++;
                aZone = building.GetZone(aIndex);
            }

            return buildingElements;
        }
        
        /***************************************************/

        public List<BuildingElementProperties> ReadBuildingElementsProperties(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;

            List<BuildingElementProperties> buildingElementProperties = new List<BuildingElementProperties>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {
                TBD.buildingElement buildingElement = tbdDocument.Building.GetBuildingElement(buildingElementIndex);
                BuildingElementType aBuildingElementType = Engine.TAS.Convert.ToBHoM((TBD.BuildingElementType)buildingElement.BEType);
                TBD.Construction construction = tbdDocument.Building.GetConstruction(buildingElementIndex);
                buildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(construction, buildingElement.name, aBuildingElementType));
                buildingElementIndex++;

            }

            return buildingElementProperties;
        }

        /***************************************************/

        public List<ConstructionLayer> ReadConstructionLayer(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;

            List<BuildingElementProperties> buildingElementProperties = new List<BuildingElementProperties>();
            List<ConstructionLayer> constructionLayer = new List<ConstructionLayer>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {

                TBD.Construction construction = tbdDocument.Building.GetConstruction(buildingElementIndex);

                int MaterialIndex = 1; // TAS doesn't have any material at index 0
                while (construction.materials(MaterialIndex) != null)
                {
                    TBD.material material = tbdDocument.Building.GetConstruction(buildingElementIndex).materials(MaterialIndex);
                    constructionLayer.Add(Engine.TAS.Convert.ToBHoM(construction, material));
                    MaterialIndex++;
                }

                buildingElementIndex++;

            }

            return constructionLayer;
        }

        /***************************************************/
        /*
        private List<BHS.Elements.Storey> readStorey(List<string> ids = null)
        {
            TBD.BuildingStorey tbdStorey = m_TBDDocumentInstance.Building.GetStorey(0);
            List<BHS.Elements.Storey> storey = new List<BHS.Elements.Storey>();
            storey.Add(Engine.TAS.Convert.ToBHoM(tbdStorey));

            return storey;
        }

        /***************************************************/

        private List<BHE.Interface.IMaterial> ReadMaterials(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;
           
            List<BHE.Interface.IMaterial> material = new List<BHE.Interface.IMaterial>();

            int constructionIndex = 0;
            while (building.GetConstruction(constructionIndex) != null)
            {
                              
                TBD.Construction currConstruction = building.GetConstruction(constructionIndex);
                               
                int materialIndex = 1; //TAS does not have any material at index 0
                while (building.GetConstruction(constructionIndex).materials(materialIndex) != null)
                {
                    TBD.material tbdMaterial = building.GetConstruction(constructionIndex).materials(materialIndex);

                    material.Add(Engine.TAS.Convert.ToBHoM(tbdMaterial));
                    materialIndex++;
                }       
              
                constructionIndex++;
            }
            return material;
        }

        /***************************************************/
    }
}
