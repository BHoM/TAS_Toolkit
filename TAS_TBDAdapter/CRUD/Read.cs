/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BH.oM.Base;
using BHE = BH.oM.Environment;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Gains;
using BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;
using BH.Engine.Environment;
using BH.Engine.Adapters.TAS;
using BHP = BH.oM.Environment.Fragments;
using BH.oM.Physical.Constructions;
using BH.oM.Physical.Materials;

using BH.oM.Adapter;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList indices = null, ActionConfig actionConfig = null)
        {

            if (type == typeof(Building))
                return ReadBuilding();
            else if (type == typeof(Panel))
                return ReadBuildingElements();
            else if (type == typeof(Space))
                return ReadSpaces();
            //else if (type == typeof(BuildingElement))
            //    return ReadPanels();
            //else if (type == typeof(ElementProperties))
            //  return ReadElementsProperties();
            else if (type == typeof(Layer))
                return ReadMaterials();
            else if (type == typeof(BH.oM.Architecture.Elements.Level))
                return ReadLevels();
            else if (type == typeof(Construction))
                return ReadConstruction();
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
            TBD.Building building = m_tbdDocument.Building;

            List<Space> spaces = new List<Space>();

            int zoneIndex = 0;
            while (building.GetZone(zoneIndex) != null)
            {
                TBD.zone zone = m_tbdDocument.Building.GetZone(zoneIndex);
                spaces.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(zone, m_tbdDocument));
                zoneIndex++;
            }

            return spaces;
        }

        /***************************************************/

        private List<Building> ReadBuilding(List<string> ids = null)
        {
            TBD.Building building = m_tbdDocument.Building;
            List<Building> buildings = new List<Building>();
            buildings.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(building));

            return buildings;
        }

        /***************************************************/

        private List<BH.oM.Architecture.Elements.Level> ReadLevels(List<string> ids = null)
        {
            TBD.Building tbdBuilding = m_tbdDocument.Building;
            List<BH.oM.Architecture.Elements.Level> levels = new List<BH.oM.Architecture.Elements.Level>();
            levels = BH.Engine.Adapters.TAS.Convert.FromTASLevels(tbdBuilding);

            return levels;
        }

        /***************************************************/

        private List<Panel> ReadPanels(List<string> ids = null)
        {

            List<Panel> panels = new List<Panel>();

            int zoneIndex = 0;
            while (m_tbdDocument.Building.GetZone(zoneIndex) != null)
            {
                int panelIndex = 0;
                while (m_tbdDocument.Building.GetZone(zoneIndex).GetSurface(panelIndex) != null)
                {
                    TBD.zoneSurface zonesurface = m_tbdDocument.Building.GetZone(zoneIndex).GetSurface(panelIndex);

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

        public List<Panel> ReadBuildingElements(List<string> ids = null)
        {
            TBD.Building building = m_tbdDocument.Building;
            List<Panel> buildingElements = new List<Panel>();

            int zoneIndex = 0;
            TBD.zone zone = null;

            while ((zone = building.GetZone(zoneIndex)) != null)
            {
                int zoneSurfaceIndex = 0;
                TBD.zoneSurface zoneSrf = null;
                while ((zoneSrf = zone.GetSurface(zoneSurfaceIndex)) != null)
                {
                    //check to exlude tine area
                    if (zoneSrf.internalArea > 0 || zoneSrf.area > 0.2)
                        buildingElements.Add(zoneSrf.buildingElement.FromTAS(zoneSrf, _tasSettings));
                    zoneSurfaceIndex++;
                }
                zoneIndex++;
            }

            //Clean up building elements with openings and constructions
            List<Panel> nonOpeningElements = buildingElements.Where(x => !((bool)x.CustomData["ElementIsOpening"])).ToList();

            List<Panel> frameElements = buildingElements.Where(x => ((bool)x.CustomData["ElementIsOpening"]) && ((bool)x.CustomData["OpeningIsFrame"])).ToList();

            List<Panel> panes = buildingElements.Where(x => ((bool)x.CustomData["ElementIsOpening"]) && !((bool)x.CustomData["OpeningIsFrame"])).ToList();

            foreach (Panel element in nonOpeningElements)
            {
                //Sort out opening construction
                OriginContextFragment originContext = element.FindFragment<OriginContextFragment>(typeof(OriginContextFragment));
                string elementID = (originContext != null ? originContext.ElementID : "");

                element.Openings = new List<Opening>();

                List<Panel> frames = frameElements.Where(x => x.Openings.Where(y => y.CustomData.ContainsKey("TAS_ParentBuildingElementGUID") && y.CustomData["TAS_ParentBuildingElementGUID"].ToString() == elementID).Count() > 0).ToList();

                foreach (Panel frame in frames)
                {
                    Panel pane = panes.Where(x => (x.FindFragment<OriginContextFragment>(typeof(OriginContextFragment))).TypeName == frame.Name.Replace("frame", "pane")).FirstOrDefault();

                    if (pane != null)
                    {
                        Opening newOpening = new Opening();
                        newOpening.Edges = frame.ExternalEdges;
                        newOpening.Fragments = new FragmentSet(pane.Fragments);

                        string oldname = (newOpening.FindFragment<OriginContextFragment>(typeof(OriginContextFragment))).TypeName;
                        (newOpening.FindFragment<OriginContextFragment>(typeof(OriginContextFragment))).TypeName = oldname.RemoveStringPart(" -pane");
                        newOpening.Name = oldname.RemoveStringPart(" -pane");

                        element.Openings.Add(newOpening);
                    }
                }
            }

            return nonOpeningElements;
        }


        /***************************************************/

        public List<Construction> ReadConstruction(List<string> ids = null)
        {
            TBD.Building building = m_tbdDocument.Building;
            List<Construction> constructions = new List<Construction>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {
                TBD.Construction construction = m_tbdDocument.Building.GetConstruction(buildingElementIndex);
                constructions.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(construction)); //ToDo: FIX THIS
                buildingElementIndex++;
            }

            return constructions;
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

        private List<Layer> ReadMaterials(List<string> ids = null)
        {
            TBD.Building building = m_tbdDocument.Building;

            List<Layer> material = new List<Layer>();

            int constructionIndex = 0;
            while (building.GetConstruction(constructionIndex) != null)
            {

                TBD.Construction currConstruction = building.GetConstruction(constructionIndex);

                int materialIndex = 1; //TAS does not have any material at index 0
                while (building.GetConstruction(constructionIndex).materials(materialIndex) != null)
                {
                    TBD.material tbdMaterial = building.GetConstruction(constructionIndex).materials(materialIndex);

                    material.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(tbdMaterial, currConstruction));
                    materialIndex++;
                }

                constructionIndex++;
            }
            return material;
        }

        /***************************************************/
    }
}

