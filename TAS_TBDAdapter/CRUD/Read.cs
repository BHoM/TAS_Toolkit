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

using BH.Engine.TAS;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(BuildingElement))
                return ReadBuildingElements();
            else if (type == typeof(Building))
                return ReadBuilding();
            else if (type == typeof(Space))
                return ReadSpaces();
            //else if (type == typeof(BuildingElement))
            //    return ReadPanels();
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

        public List<IBHoMObject> Read()
        {
            List<IBHoMObject> bhomObjects = new List<IBHoMObject>();

            bhomObjects.AddRange(ReadBuilding());
            bhomObjects.AddRange(ReadSpaces());
            bhomObjects.AddRange(ReadBuildingElements());
            bhomObjects.AddRange(ReadConstructionLayer());

            return bhomObjects;
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
            TBD.buildingElement buildingElement = null;
            List<BuildingElement> buildingElements = new List<BuildingElement>();

            /*int elementIndex = 0;

            while ((buildingElement = building.GetBuildingElement(elementIndex)) != null)
            {
                buildingElements.Add(buildingElement.ToBHoM()); //Convert element...
                elementIndex++;
            }*/

            int zoneIndex = 0;
            TBD.zone zone = null;

            while((zone = building.GetZone(zoneIndex)) != null)
            {
                int zoneSurfaceIndex = 0;
                TBD.zoneSurface zoneSrf = null;
                while((zoneSrf = zone.GetSurface(zoneSurfaceIndex)) != null)
                {
                    buildingElements.Add(zoneSrf.buildingElement.ToBHoM(zoneSrf));
                    //int roomSrfIndex = 0;
                    //TBD.RoomSurface roomSrf = null;
                    //while((roomSrf = zoneSrf.GetRoomSurface(roomSrfIndex)) != null)
                    //{
                    //    if(roomSrf.GetPerimeter() != null)
                    //    {
                    //        //Sometimes we can have a srf object in TAS without a geometry
                    //        buildingElements.Add(zoneSrf.buildingElement.ToBHoM(roomSrf));
                    //    }
                    //    roomSrfIndex++;
                    //}
                    zoneSurfaceIndex++;
                }
                zoneIndex++;
            }


            //Reading Zones
            //TBD.zone aZone = building.GetZone(aIndex);

            /*while(aZone != null)
            {
                //Reading ZoneSurfaces
                int zoneSurfaceIndex = 0;
                TBD.zoneSurface zoneSurface = new TBD.zoneSurface();
                while (aZone.GetSurface(zoneSurfaceIndex) != null)
                {
                    //Reading RoomSurfaces
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
            */
            return buildingElements;
        }

        //get external surfaces for filter query test roof
        public List<BuildingElement> ReadExternalBuildingElements(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;
            TBD.buildingElement buildingElement = null;
            List<BuildingElement> buildingElements = new List<BuildingElement>();

            /*int elementIndex = 0;

            while ((buildingElement = building.GetBuildingElement(elementIndex)) != null)
            {
                buildingElements.Add(buildingElement.ToBHoM()); //Convert element...
                elementIndex++;
            }*/

            int zoneIndex = 0;
            TBD.zone zone = null;

            while ((zone = building.GetZone(zoneIndex)) != null)
            {
                int zoneSurfaceIndex = 0;
                TBD.zoneSurface zoneSrf = null;
                while ((zoneSrf = zone.GetSurface(zoneSurfaceIndex)) != null)
                {
                    int roomSrfIndex = 0;
                    TBD.RoomSurface roomSrf = null;
                    while ((roomSrf = zoneSrf.GetRoomSurface(roomSrfIndex)) != null)
                    {
                        if (roomSrf.GetPerimeter() != null)
                        {
                            if (zoneSrf.buildingElement.BEType ==3)
                            {
                            //Sometimes we can have a srf object in TAS without a geometry
                            buildingElements.Add(zoneSrf.buildingElement.ToBHoM(roomSrf));

                            }
                        }
                        roomSrfIndex++;
                    }
                    zoneSurfaceIndex++;
                }
                zoneIndex++;
            }


            //Reading Zones
            //TBD.zone aZone = building.GetZone(aIndex);

            /*while(aZone != null)
            {
                //Reading ZoneSurfaces
                int zoneSurfaceIndex = 0;
                TBD.zoneSurface zoneSurface = new TBD.zoneSurface();
                while (aZone.GetSurface(zoneSurfaceIndex) != null)
                {
                    //Reading RoomSurfaces
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
            */
            return buildingElements;
        }

        /***************************************************/

        public List<BuildingElementProperties> ReadBuildingElementsProperties(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;

            List<BuildingElementProperties> buildingElementProperties = new List<BuildingElementProperties>();

            int buildingElementIndex = 0;
            TBD.buildingElement bElement = null;
            while((bElement = tbdDocument.Building.GetBuildingElement(buildingElementIndex)) != null)
            {
                BuildingElementType aBuildingElementType = Engine.TAS.Convert.ToBHoM((TBD.BuildingElementType)bElement.BEType);
                TBD.Construction construction = bElement.GetConstruction();
                buildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(construction, bElement.name, aBuildingElementType, bElement));
                buildingElementIndex++;
            }
            /*while (building.GetConstruction(buildingElementIndex) != null)
            {
                TBD.buildingElement buildingElement = tbdDocument.Building.GetBuildingElement(buildingElementIndex);
                //buildingElement.Get
                BuildingElementType aBuildingElementType = Engine.TAS.Convert.ToBHoM((TBD.BuildingElementType)buildingElement.BEType);
                TBD.Construction construction = tbdDocument.Building.GetConstruction(buildingElementIndex);
                buildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(construction, buildingElement.name, aBuildingElementType, buildingElement));
                buildingElementIndex++;

            }*/

            return buildingElementProperties;
        }

        /***************************************************/

        public List<ConstructionLayer> ReadConstructionLayer(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;

            List<ConstructionLayer> constructionLayer = new List<ConstructionLayer>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {
                TBD.Construction construction = tbdDocument.Building.GetConstruction(buildingElementIndex);
                TBD.material material = null;
                int materialIndex = 1; // TAS doesn't have any material at index 0
                while ((material = construction.materials(materialIndex)) != null)
                {
                    constructionLayer.Add(Engine.TAS.Convert.ToBHoM(construction, material));
                    materialIndex++;
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
