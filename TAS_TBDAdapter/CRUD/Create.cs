   
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
            bool success = true;
            foreach (BHE.Elements.Space space in spaces)
            {
                TBD.zone tbdSpace = tbdDocument.Building.AddZone();
                tbdSpace.name = space.Name;
                tbdSpace.maxHeatingLoad = (float)space.HeatingLoad;
                tbdSpace.maxCoolingLoad = (float)space.CoolingLoad;

                Dictionary<string, object> tasData = space.CustomData;
                if (tasData != null)
                {
                    tbdSpace.colour = (tasData.ContainsKey("SpaceColour") ? System.Convert.ToUInt32(tasData["SpaceColour"]) : 0);
                    tbdSpace.daylightFactor = (tasData.ContainsKey("DaylightFactor") ? (float)System.Convert.ToDouble(tasData["DaylightFactor"]) : 0);
                    tbdSpace.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
                    tbdSpace.exposedPerimeter = (tasData.ContainsKey("ExposedPerimeter") ? (float)System.Convert.ToDouble(tasData["ExposedPerimeter"]) : 0);
                    tbdSpace.external = (tasData.ContainsKey("External") ? System.Convert.ToInt32(tasData["External"]) : 0);
                    tbdSpace.facadeLength = (tasData.ContainsKey("FacadeLength") ? (float)System.Convert.ToDouble(tasData["FacadeLength"]) : 0);
                    tbdSpace.fixedConvectionCoefficient = (tasData.ContainsKey("FixedConvectionCoefficient") ? (float)System.Convert.ToDouble(tasData["FixedConvectionCoefficient"]) : 0);
                    tbdSpace.floorArea = (tasData.ContainsKey("FloorArea") ? (float)System.Convert.ToDouble(tasData["FloorArea"]) : 0);
                    tbdSpace.GUID = (tasData.ContainsKey("GUID") ? tasData["GUID"].ToString() : "");
                    tbdSpace.length = (tasData.ContainsKey("Length") ? (float)System.Convert.ToDouble(tasData["Length"]) : 0);
                    tbdSpace.sizeCooling = (tasData.ContainsKey("SizeCooling") ? System.Convert.ToInt32(tasData["SizeCooling"]) : 0);
                    tbdSpace.sizeHeating = (tasData.ContainsKey("SizeHeating") ? System.Convert.ToInt32(tasData["SizeHeating"]) : 0);
                    tbdSpace.volume = (tasData.ContainsKey("Volume") ? (float)System.Convert.ToDouble(tasData["Volume"]) : 0);
                    tbdSpace.wallFloorAreaRatio = (tasData.ContainsKey("WallFloorAreaRatio") ? (float)System.Convert.ToDouble(tasData["WallFloorAreaRatio"]) : 0);
                }
            }
            return success;

        }
        /***************************************************/
        private bool Create(BH.oM.Environment.Elements.Building building)
        {
            bool success = true;
            TBD.Building tbdBuilding = tbdDocument.Building;
            tbdBuilding.name = building.Name;
            //tbdBuilding.latitude = (float)building.Latitude;
            tbdBuilding.longitude = (float)building.Longitude;
            tbdBuilding.maxBuildingAltitude = (float)building.Elevation;
            Dictionary<string, object> tasData = building.CustomData;

            if (tasData != null)
            {
                tbdBuilding.description = (tasData.ContainsKey("BuildingDescription") ? tasData["BuildingDescription"].ToString() : "");
                tbdBuilding.northAngle = (tasData.ContainsKey("BuildingNorthAngle") ? (float)System.Convert.ToDouble(tasData["BuildingNorthAngle"]) : 0);
                tbdBuilding.path3DFile = (tasData.ContainsKey("BuildingPath3DFile") ? tasData["BuildingPath3DFile"].ToString() : "");
                tbdBuilding.peakCooling = (tasData.ContainsKey("BuildingPeakCooling") ? (float)System.Convert.ToDouble(tasData["BuildingPeakCooling"]) : 0);
                tbdBuilding.peakHeating = (tasData.ContainsKey("BuildingPeakHeating") ? (float)System.Convert.ToDouble(tasData["BuildingPeakHeating"]) : 0);
                tbdBuilding.GUID = (tasData.ContainsKey("BuildingTBDGUID") ? tasData["BuildingTBDGUID"].ToString() : "");
                tbdBuilding.timeZone = (tasData.ContainsKey("BuildingTimeZone") ? (float)System.Convert.ToDouble(tasData["BuildingTimeZone"]) : 0);
                if (tasData.ContainsKey("BuildingYear"))
                {
                    short year = System.Convert.ToInt16(tasData["BuildingYear"]);
                    tbdBuilding.year = year;
                    //TODO:Make Longitude, Latitude, NorthAngle work for PushTBD
                    //TODO:Add Facadelength, Length, VariableConvectionCoefficient for PushTBD
                }
            }

                    return success;
        }
        
        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement buildingElement)
        {
            TBD.buildingElement tbdBuildingElement = tbdDocument.Building.AddBuildingElement();
            tbdBuildingElement.name = buildingElement.Name;

            BHP.EnvironmentContextProperties envContextProperties = buildingElement.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;
            if (envContextProperties != null)
                tbdBuildingElement.GUID = envContextProperties.ElementID;
                tbdBuildingElement.description = envContextProperties.Description;

            BHP.ElementProperties elementProperties = buildingElement.ElementProperties() as BHP.ElementProperties;
            if (elementProperties != null)
                tbdBuildingElement.BEType = (int)elementProperties.BuildingElementType.ToTAS();
                //TBD.Construction construction = elementProperties.Construction.ToTAS();
                //tbdBuildingElement.AssignConstruction(construction);

            //TODO:Make Colour, Construction, GUID work for PushTBD

            //BHP.BuildingElementContextProperties BEContextProperties = buildingElement.ContextProperties() as BHP.BuildingElementContextProperties;
            //if (BEContextProperties != null)
            //    tbdBuildingElement.colour = System.Convert.ToUInt32(BEContextProperties.Colour);

            //Dictionary<string, object> tasData = buildingElement.CustomData;
            //if (tasData != null)
            //{
            //        tbdBuildingElement.colour = (tasData.ContainsKey("BuildingElementColour") ? System.Convert.ToUInt32(tasData["BuildingElementColour"]) : 0);
            //        tbdBuildingElement.colour = (tasData.ContainsKey("ElementColour") ? System.Convert.ToUInt32(tasData["ElementColour"]) : 0);
            //    tbdBuildingElement.description = (tasData.ContainsKey("ElementDescription") ? tasData["ElementDescription"].ToString() : "");
            //    tbdBuildingElement.ghost = (tasData.ContainsKey("ElementIsAir") ? (((bool)tasData["ElementIsAir"]) ? 1 : 0) : 0);
            //    tbdBuildingElement.ground = (tasData.ContainsKey("ElementIsGround") ? (((bool)tasData["ElementIsGround"]) ? 1 : 0) : 0);
            //    tbdBuildingElement.GUID = (tasData.ContainsKey("ElementGUID") ? tasData["ElementGUID"].ToString() : "");
            //    tbdBuildingElement.width = (tasData.ContainsKey("ElementWidth") ? (float)System.Convert.ToDouble(tasData["ElementWidth"]) : 0);
            //}
            return true;
        }

        /***************************************************/

        private bool Create(BHE.Properties.ElementProperties elementProperties)
        {
            TBD.Construction tbdConstruction = tbdDocument.Building.AddConstruction(null);
            tbdConstruction.name = elementProperties.Construction.Name;


            tbdConstruction.materialWidth[0] = (float)elementProperties.Construction.Thickness;
            
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
            TBD.InternalCondition tbdCondition = tbdDocument.Building.AddIC(null);
            tbdCondition.name = internalCondition.Name;
            tbdCondition.includeSolarInMRT = (internalCondition.IncludeSolarInMeanRadiantTemp ? 1 : 0);

            Dictionary<string, object> tasData = internalCondition.CustomData;
            if (tasData != null)
            {
                tbdCondition.description=(tasData.ContainsKey("InternalConditionDescription")?tasData["InternalConditionDescription"].ToString():"");
            }

            TBD.Emitter heatingEmitter = tbdCondition.GetHeatingEmitter();
            heatingEmitter = internalCondition.Emitters.Where(x => x.EmitterType == BHE.Elements.EmitterType.Heating).First().ToTAS(heatingEmitter);

            TBD.Emitter coolingEmitter = tbdCondition.GetCoolingEmitter();
            coolingEmitter = internalCondition.Emitters.Where(x => x.EmitterType == BHE.Elements.EmitterType.Cooling).First().ToTAS(coolingEmitter);

            //TODO: Add Daytypes, Internalgain, Thermostat

            //foreach (BHE.Elements.SimulationDayType dayType in internalCondition.DayTypes)
            //    tbdCondition.SetDayType(dayType.ToTAS(), true);
            //TBD.InternalGain internalGain = tbdCondition.GetInternalGain();
            //internalGain = internalCondition.Gains.ToTAS();
            //TBD.Thermostat thermostat = tbdCondition.GetThermostat();

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


