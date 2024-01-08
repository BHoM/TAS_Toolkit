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
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BHM = BH.oM.Environment.MaterialFragments;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;
using BH.Engine.Environment;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.oM.Architecture;
using BH.oM.Base.Attributes;
using System.ComponentModel;
using BHP = BH.oM.Environment.Fragments;
using BH.Engine.Adapters.TAS;

using BH.oM.Adapter;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Interface methods                  ****/
        /***************************************************/

        protected bool ICreate<T>(IEnumerable<T> objects, TBDDocument document, ActionConfig actionConfig = null)
        {
            return Create(objects as dynamic, document);
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private bool Create(IEnumerable<BHE.Elements.Space> spaces, TBDDocument document)
        {
            foreach (BHE.Elements.Space space in spaces)
            {
                space.ToTAS(document.Document.Building.AddZone());
            }
            return true;
        }

        /***************************************************/

        private bool Create(IEnumerable<BH.oM.Environment.Elements.Building> buildings, TBDDocument document)
        {
            foreach(BH.oM.Environment.Elements.Building building in buildings)
                building.ToTAS(document.Document.Building);
            return true;
        }

        /***************************************************/

        private bool Create(IEnumerable<BHE.Elements.Panel> buildingElements, TBDDocument document)
        {
            TBD.Construction tbdConstruction;

            List<BHE.Elements.Panel> elements = buildingElements.ToList();

            Dictionary<string, TBD.Construction> tbdConstructions = new Dictionary<string, TBD.Construction>();
            foreach (BH.oM.Physical.Constructions.Construction construction in elements.UniqueConstructions())
            {
                tbdConstruction = document.Document.Building.AddConstruction(null);
                tbdConstructions.Add(construction.UniqueConstructionName(), construction.ToTAS(tbdConstruction));
            }

            foreach (BHE.Elements.Panel buildingElement in buildingElements)
            {
                TBD.Construction tasCon = null;
                /*BHP.ElementProperties elementProperties = buildingElement.ElementProperties() as BHP.ElementProperties;
                if (elementProperties == null || elementProperties.Construction == null)
                    tasCon = tbdDocument.Building.AddConstruction(null);
                else
                    tasCon = tbdConstructions.Where(x => x.Key == elementProperties.Construction.UniqueConstructionName()).FirstOrDefault().Value;*/
                buildingElement.ToTAS(document.Document.Building.AddBuildingElement(), tasCon);

                foreach(BHE.Elements.Opening opening in buildingElement.Openings)
                {
                    /*elementProperties = opening.ElementProperties() as BHP.ElementProperties;
                    if (elementProperties == null || elementProperties.Construction == null)
                        tasCon = tbdDocument.Building.AddConstruction(null);
                    else
                        tasCon = tbdConstructions.Where(x => x.Key == elementProperties.Construction.UniqueConstructionName()).FirstOrDefault().Value;
                        */
                    opening.ToTAS(document.Document.Building.AddBuildingElement(), tasCon);
                }
            }
            return true;
        }
      
        /***************************************************/

        private bool Create(IEnumerable<BH.oM.Physical.Constructions.Construction> constructions, TBDDocument document)
        {
            foreach(BH.oM.Physical.Constructions.Construction construction in constructions)
                construction.ToTAS(document.Document.Building.AddConstruction(null));

            return true;
        }
        
        /***************************************************/

        private bool Create(IEnumerable<BH.oM.Physical.Constructions.Layer> layers, TBDDocument document, TBD.Construction tbdConstruction = null)
        {
            if (tbdConstruction == null)
                tbdConstruction = document.Document.Building.AddConstruction(null);

            foreach(BH.oM.Physical.Constructions.Layer layer in layers)
                layer.ToTAS(tbdConstruction.AddMaterial());
            
            return true;
        }

        /***************************************************/
        /**** Fallback methods                          ****/
        /***************************************************/

        private bool Create(IEnumerable<object> objects, TBDDocument document)
        {
            BH.Engine.Base.Compute.RecordError($"The type: {objects.GetType()} cannot be pushed to a TBD file.");
            return false;
        }

    }

    /***************************************************/

}
