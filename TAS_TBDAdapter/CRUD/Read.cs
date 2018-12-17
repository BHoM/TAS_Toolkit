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

            if (type == typeof(Building))
                return ReadBuilding();
            else if (type == typeof(BuildingElement))
                return ReadBuildingElements();
            else if (type == typeof(Space))
                return ReadSpaces();
            //else if (type == typeof(BuildingElement))
            //    return ReadPanels();
            else if (type == typeof(BuildingElementProperties))
                return ReadBuildingElementsProperties();
            else if (type == typeof(BH.oM.Environment.Materials.Material))
                return ReadMaterials();
            else if (type == typeof(BH.oM.Architecture.Elements.Level))
                return ReadLevels();
            else if (type == typeof(Construction))
                return ReadConstruction();
            else if (type == typeof(InternalCondition))
                return ReadInternalCondition();
            else
                return Read(); //Read everything
        }

        public List<IBHoMObject> Read()
        {
            List<IBHoMObject> bhomObjects = new List<IBHoMObject>();

            bhomObjects.AddRange(ReadBuilding());
            bhomObjects.AddRange(ReadSpaces());
            bhomObjects.AddRange(ReadBuildingElements());
            bhomObjects.AddRange(ReadConstruction());
            bhomObjects.AddRange(ReadLevels());

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

        private List<BH.oM.Architecture.Elements.Level> ReadLevels(List<string> ids = null)
        {
            TBD.Building tbdBuilding = tbdDocument.Building;
            List<BH.oM.Architecture.Elements.Level> levels = new List<BH.oM.Architecture.Elements.Level>();
            levels = Engine.TAS.Convert.ToBHoMLevels(tbdBuilding);

            return levels;
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

            int zoneIndex = 0;
            TBD.zone zone = null;

            while ((zone = building.GetZone(zoneIndex)) != null)
            {
                int zoneSurfaceIndex = 0;
                TBD.zoneSurface zoneSrf = null;
                while ((zoneSrf = zone.GetSurface(zoneSurfaceIndex)) != null)
                {
                    buildingElements.Add(zoneSrf.buildingElement.ToBHoM(zoneSrf));
                    zoneSurfaceIndex++;
                }
                zoneIndex++;
            }

            return buildingElements;
        }

        //get external surfaces for filter   
        public List<BuildingElement> ReadExternalBuildingElements(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;
            List<BuildingElement> buildingElements = new List<BuildingElement>();

            int zoneIndex = 0;
            TBD.zone zone = null;

            while ((zone = building.GetZone(zoneIndex)) != null)
            {
                int zoneSurfaceIndex = 0;
                TBD.zoneSurface zoneSrf = null;
                while ((zoneSrf = zone.GetSurface(zoneSurfaceIndex)) != null)
                {

                    if (zoneSrf.buildingElement.BEType == 3
                        || zoneSrf.buildingElement.BEType == 2
                        || zoneSrf.buildingElement.BEType == 6
                        || zoneSrf.buildingElement.BEType == 7
                        || zoneSrf.buildingElement.BEType == 11
                        || zoneSrf.buildingElement.BEType == 16
                        || zoneSrf.buildingElement.BEType == 19)
                    {
                        //Sometimes we can have a srf object in TAS without a geometry
                        buildingElements.Add(zoneSrf.buildingElement.ToBHoM(zoneSrf));
                    }

                    zoneSurfaceIndex++;
                }
                zoneIndex++;
            }


            return buildingElements;
        }

        /***************************************************/

        public List<BuildingElementProperties> ReadBuildingElementsProperties(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;

            List<BuildingElementProperties> buildingElementProperties = new List<BuildingElementProperties>();

            int buildingElementIndex = 0;
            TBD.buildingElement tbdBuildingElement = null;
            while ((tbdBuildingElement = tbdDocument.Building.GetBuildingElement(buildingElementIndex)) != null)
            {
                //BuildingElementType aBuildingElementType = Engine.TAS.Convert.ToBHoM((TBD.BuildingElementType)tbdBuildingElement.BEType);
                TBD.Construction construction = tbdBuildingElement.GetConstruction();
                BH.oM.Environment.Elements.BuildingElementType bHoMBuildingElementType = BH.Engine.TAS.Convert.ToBHoM((TBD.BuildingElementType)tbdBuildingElement.BEType);
                buildingElementProperties.Add(Engine.TAS.Convert.ToBHoM(construction, tbdBuildingElement.name, bHoMBuildingElementType, tbdBuildingElement));
                buildingElementIndex++;
            }

            return buildingElementProperties;
        }

        /***************************************************/

        public List<Construction> ReadConstruction(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;
            List<Construction> constructions = new List<Construction>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {
                TBD.Construction construction = tbdDocument.Building.GetConstruction(buildingElementIndex);
                constructions.Add(Engine.TAS.Convert.ToBHoMConstruction(construction));
                buildingElementIndex++;
            }

            return constructions;
        }

        /***************************************************/

        public List<InternalCondition> ReadInternalCondition(List<string> ids = null)
        {
            TBD.Building building = tbdDocument.Building;
            List<InternalCondition> internalConditions = new List<InternalCondition>();

            int internalConditionIndex = 0;
            while (building.GetIC(internalConditionIndex) != null)
            {
                TBD.InternalCondition internalCondition = tbdDocument.Building.GetIC(internalConditionIndex);
                internalConditions.Add(Engine.TAS.Convert.ToBHoM(internalCondition));
                internalConditionIndex++;
            }

            return internalConditions;
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
