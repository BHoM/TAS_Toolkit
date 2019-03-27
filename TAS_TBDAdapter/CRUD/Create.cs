   
/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using BHM = BH.oM.Environment.Materials;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;
using BH.Engine.Environment;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.oM.Architecture;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BHP = BH.oM.Environment.Properties;
using BH.Engine.TAS;

namespace BH.Adapter.TAS
{
    public partial class TasTBDAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;

            if (typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                success = CreateCollection(objects as dynamic);
            }

            return success;
        }

        /***************************************************/

        public static void ClearCOMObject(object Object)
        {
            if (Object == null) return;
            int intrefcount = 0;
            do
            {
                intrefcount = Marshal.FinalReleaseComObject(Object);
            } while (intrefcount > 0);
            Object = null;
        }


        /***************************************************/
        /**** Create methods                            ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<IBHoMObject> objects)
        {
            bool success = true;
            foreach (IBHoMObject obj in objects)
            {
                success &= Create(obj as dynamic);
            }
            return success;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<BHE.Elements.Space> spaces)
        {
            foreach (BHE.Elements.Space space in spaces)
            {
                space.ToTAS(tbdDocument.Building.AddZone());
            }
            return true;

        }
        /***************************************************/

        private bool Create(BH.oM.Environment.Elements.Building building)
        {
            building.ToTAS(tbdDocument.Building);
            return true;
        }
        
        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement buildingElement)
        {
            buildingElement.ToTAS(tbdDocument.Building.AddBuildingElement());
            return true;
        }

        /***************************************************/

            //TODO: Remove ElementProperties?

        //private bool Create(BHE.Properties.ElementProperties elementProperties)
        //{
        //    TBD.Construction tbdConstruction = tbdDocument.Building.AddConstruction(null);
        //    tbdConstruction.name = elementProperties.Construction.Name;

        //    tbdConstruction.materialWidth[0] = (float)elementProperties.Construction.Thickness;
        //    return true;
        //}
        /***************************************************/
        
            //TODO: Pull Constructions vid Building Elements

        //private bool Create(BHE.Elements.Construction construction, TBD.buildingElement tbdBuildingElement=null)
        //{
        //    if (tbdBuildingElement == null)
        //        tbdBuildingElement = tbdDocument.Building.AddBuildingElement();
        //    construction.ToTAS(tbdBuildingElement.AssignConstruction(construction.ToTAS(tbdDocument.Building.AddConstruction(null)));

        //    return true;
        //}

        private bool Create(BHE.Elements.Construction construction)
        {
            construction.ToTAS(tbdDocument.Building.AddConstruction(null));

            return true;
        }
        
        /***************************************************/

        private bool Create(BHE.Materials.Material material, TBD.Construction tbdConstruction = null)
        {
            if (tbdConstruction == null)
                tbdConstruction = tbdDocument.Building.AddConstruction(null);
            material.ToTAS(tbdConstruction.AddMaterial());
            
            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.Panel buildingElementPanel)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        private bool Create(BHE.Elements.Opening buildingElementOpening)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        private bool Create(BHE.Elements.InternalCondition internalCondition)
        {
            internalCondition.ToTAS(tbdDocument.Building.AddIC(null));
            return true;
        }

        /***************************************************/
        /*           
       private bool Create(BHE.Elements.Space space, IEnumerable<BHE.Elements.Space> spaces)
       {
           TBD.zone tbdZone = m_TBDDocument.Building.AddZone();
           TBD.room tbdRoom = tbdZone.AddRoom();
           tbdZone = Engine.TAS.Convert.ToTas(space, tbdZone);
           //TODO: Can not use BuildingElement from Spaces, (line 80 bHoMSpace.BuildingElements)
           //foreach (BHE.Elements.BuildingElement element in bHoMSpace.BuildingElements)
           {
               //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
               TBD.buildingElement be = m_TBDDocument.Building.AddBuildingElement();
               //Add zoneSrf and convert it
               TBD.zoneSurface tbdZoneSrf = tbdZone.AddSurface();
               tbdZoneSrf = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry, tbdZoneSrf);
               //Add roomSrf, create face, get its controlpoints and convert to TAS
               TBD.Polygon tbdPolygon = tbdRoom.AddSurface().CreatePerimeter().CreateFace();
               tbdPolygon = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry.ICurve(), tbdPolygon);
               //Set the building Element
               tbdZoneSrf.buildingElement = Engine.TAS.Convert.ToTas(element, be, m_TBDDocument.Building);
               //tasZoneSrf.type = BH.Engine.TAS.Query.GetSurfaceType(element, spaces);
               tbdZoneSrf.orientation = (float)BH.Engine.Environment.Query.Azimuth(element.BuildingElementGeometry, new BHG.Vector());
               //tasZoneSrf.orientation = BH.Engine.TAS.Query.GetOrientation(element.BuildingElementGeometry, bHoMSpace);
               tbdZoneSrf.inclination = (float)BH.Engine.Environment.Query.Tilt(element.BuildingElementGeometry);
               //tasZoneSrf.inclination = BH.Engine.TAS.Query.GetInclination(element.BuildingElementGeometry, bHoMSpace);
           }
           return true;
       }
       */
        /*
        private bool Create(BHE.Elements.Space space, BHE.Elements.Building building)
        { 
            TBD.zone tbdZone = m_TBDDocument.Building.AddZone();
            TBD.room tbdRoom = tbdZone.AddRoom();
            tbdZone = Engine.TAS.Convert.ToTas(space, tbdZone);
            //TODO: Change BuildingElements to depend on a list of objects
            foreach (BHE.Elements.BuildingElement element in Query.BuildingElements(building, space))
            {
                //Add zoneSrf and convert it
                TBD.zoneSurface tbdZoneSrf = tbdZone.AddSurface();
                //BuildingElementGeometry is removed from element.
                tbdZoneSrf = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry, tbdZoneSrf);
                //MD assign type to be fixed!
                tbdZoneSrf.type = BH.Engine.TAS.Query.SurfaceType(element); 
                //Add roomSrf, create face, get its controlpoints and convert to TAS
                TBD.Polygon tbdPolygon = tbdRoom.AddSurface().CreatePerimeter().CreateFace();
                tbdPolygon = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry.ICurve(), tbdPolygon);
                //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
                TBD.buildingElement be = BH.Engine.TAS.Query.BuildingElement(m_TBDDocument.Building, element.Name);
                if (be == null)
                {
                    be = m_TBDDocument.Building.AddBuildingElement();
                    //Set the building Element
                     Engine.TAS.Convert.ToTas(element, be, m_TBDDocument.Building);
                }
                tbdZoneSrf.buildingElement = be;
                //tasZoneSrf.type = BH.Engine.TAS.Query.GetSurfaceType(element, spaces);
                tbdZoneSrf.orientation = (float)BH.Engine.Environment.Query.Azimuth(element.BuildingElementGeometry, new BHG.Vector());
                //tasZoneSrf.orientation = BH.Engine.TAS.Query.GetOrientation(element.BuildingElementGeometry, bHoMSpace);
                tbdZoneSrf.inclination = (float)BH.Engine.Environment.Query.Tilt(element.BuildingElementGeometry);
                //tasZoneSrf.inclination = BH.Engine.TAS.Query.GetInclination(element.BuildingElementGeometry, bHoMSpace);
            }
            return true;
        */
    }

    /***************************************************/

}


