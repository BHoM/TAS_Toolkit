/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Environment.SpaceCriteria;
using BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;
using BH.Engine.Environment;
using BH.Engine.Adapters.TAS;
using BHP = BH.oM.Environment.Fragments;
using BH.oM.Physical.Constructions;
using BH.oM.Physical.Materials;

using BH.oM.Adapter;

using BH.Engine.Base;
using BH.oM.Adapters.TAS.Fragments;
using BH.oM.Adapters.TAS;
using BH.Engine.Adapter;
using BH.oM.Data.Requests;
using System.IO;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList ids, ActionConfig actionConfig = null)
        {
            List<IBHoMObject> objects = new List<IBHoMObject>();
            if (actionConfig == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return objects;
            }

            TASTBDConfig config = (TASTBDConfig)actionConfig;
            if (config == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TASTBDConfig ActionConfig to use this adapter.");
                return objects;
            }

            if (config.TBDFile == null)
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid TBDFile FileSettings object to use this adapter.");
                return objects;
            }
            else if (!File.Exists(config.TBDFile.GetFullFileName()))
            {
                BH.Engine.Base.Compute.RecordError("You must provide a valid existing TBD file to read from.");
                return objects;
            }

            TBDDocument document = new TBDDocument().OpenTASDocument(config.TBDFile); //Open the TBD Document for pulling data from

            try
            {
                if (type == typeof(Building))
                    objects.AddRange(ReadBuilding(document));
                else if (type == typeof(Panel))
                    objects.AddRange(ReadBuildingElements(document, config));
                else if (type == typeof(Space))
                    objects.AddRange(ReadSpaces(document));
                //else if (type == typeof(BuildingElement))
                //    return ReadPanels();
                //else if (type == typeof(ElementProperties))
                //  return ReadElementsProperties();
                else if (type == typeof(Layer))
                    objects.AddRange(ReadMaterials(document));
                else if (type == typeof(BH.oM.Spatial.SettingOut.Level))
                    objects.AddRange(ReadLevels(document));
                else if (type == typeof(Construction))
                    objects.AddRange(ReadConstruction(document));
                else if (type == null)
                    objects.AddRange(Read(document, config)); //Read everything
                else
                    BH.Engine.Base.Compute.RecordError($"Reading elements of type: {type} is not currently supported.");

                Compute.ICloseTASDocument(document, true);

                return objects;
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordError($"An error occurred when reading or saving the TAS file: {e}.");
                Compute.ICloseTASDocument(document, false);
                return objects;
            }

        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<IBHoMObject> Read(TBDDocument document, TASTBDConfig actionConfig)
        {
            List<IBHoMObject> bhomObjects = new List<IBHoMObject>();

            bhomObjects.AddRange(ReadBuilding(document));
            bhomObjects.AddRange(ReadSpaces(document));
            bhomObjects.AddRange(ReadBuildingElements(document, actionConfig));
            bhomObjects.AddRange(ReadConstruction(document));
            bhomObjects.AddRange(ReadLevels(document));

            return bhomObjects;
        }

        /***************************************************/

        private List<Space> ReadSpaces(TBDDocument document)
        {
            TBD.Building building = document.Document.Building;

            List<Space> spaces = new List<Space>();

            int zoneIndex = 0;
            while (building.GetZone(zoneIndex) != null)
            {
                TBD.zone zone = document.Document.Building.GetZone(zoneIndex);
                spaces.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(zone, document.Document));
                zoneIndex++;
            }

            return spaces;
        }

        /***************************************************/

        private List<Building> ReadBuilding(TBDDocument document)
        {
            TBD.Building building = document.Document.Building;
            List<Building> buildings = new List<Building>();
            buildings.Add(BH.Engine.Adapters.TAS.Convert.FromTAS(building));

            return buildings;
        }

        /***************************************************/

        private List<BH.oM.Spatial.SettingOut.Level> ReadLevels(TBDDocument document)
        {
            TBD.Building tbdBuilding = document.Document.Building;
            List<BH.oM.Spatial.SettingOut.Level> levels = new List<BH.oM.Spatial.SettingOut.Level>();
            levels = BH.Engine.Adapters.TAS.Convert.FromTASLevels(tbdBuilding);

            return levels;
        }

        /***************************************************/

        private List<Panel> ReadPanels(TBDDocument document)
        {

            List<Panel> panels = new List<Panel>();

            int zoneIndex = 0;
            while (document.Document.Building.GetZone(zoneIndex) != null)
            {
                int panelIndex = 0;
                while (document.Document.Building.GetZone(zoneIndex).GetSurface(panelIndex) != null)
                {
                    TBD.zoneSurface zonesurface = document.Document.Building.GetZone(zoneIndex).GetSurface(panelIndex);

                    int roomSurfaceIndex = 0;
                    while (zonesurface.GetRoomSurface(roomSurfaceIndex) != null)
                    {
                        TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(roomSurfaceIndex);

                        //if (currRoomSrf.GetPerimeter() != null)
                            //panels.Add(BH.Engine.Adapters.TAS.Convert.(currRoomSrf));

                        roomSurfaceIndex++;
                    }

                    panelIndex++;
                }
                zoneIndex++;
            }
            return panels;
        }

        /***************************************************/

        private List<Panel> ReadBuildingElements(TBDDocument document, TASTBDConfig actionConfig)
        {
            TBD.Building building = document.Document.Building;
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
                        buildingElements.Add(zoneSrf.buildingElement.FromTAS(zoneSrf, actionConfig));
                    zoneSurfaceIndex++;
                }
                zoneIndex++;
            }

            //Clean up building elements with openings and constructions
            List<Panel> nonOpeningElements = buildingElements.Where(x =>
            {
                TASPanelData fragment = x.FindFragment<TASPanelData>(typeof(TASPanelData));
                return fragment.PanelIsOpening;
            }).ToList();

            List<Panel> frameElements = buildingElements.Where(x =>
            {
                TASPanelData fragment = x.FindFragment<TASPanelData>(typeof(TASPanelData));
                return fragment.PanelIsOpening && fragment.OpeningIsFrame;
            }).ToList();

            List<Panel> panes = buildingElements.Where(x =>
            {
                TASPanelData fragment = x.FindFragment<TASPanelData>(typeof(TASPanelData));
                return fragment.PanelIsOpening && !fragment.OpeningIsFrame;
            }).ToList();

            foreach (Panel element in nonOpeningElements)
            {
                //Sort out opening construction
                OriginContextFragment originContext = element.FindFragment<OriginContextFragment>(typeof(OriginContextFragment));
                string elementID = (originContext != null ? originContext.ElementID : "");

                element.Openings = new List<Opening>();

                List<Panel> frames = frameElements.Where(x =>
                {
                    return x.Openings.Where(y =>
                    {
                        TASOpeningData fragment = y.FindFragment<TASOpeningData>(typeof(TASOpeningData));
                        return fragment.ParentGUID == elementID;
                    }).Count() > 0;
                }).ToList();

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

        private List<Construction> ReadConstruction(TBDDocument document)
        {
            TBD.Building building = document.Document.Building;
            List<Construction> constructions = new List<Construction>();

            int buildingElementIndex = 0;
            while (building.GetConstruction(buildingElementIndex) != null)
            {
                TBD.Construction construction = document.Document.Building.GetConstruction(buildingElementIndex);
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

        private List<Layer> ReadMaterials(TBDDocument document)
        {
            TBD.Building building = document.Document.Building;

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
    }
}
