using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;
using BH.Engine.Environment;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
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
                //success &= Create(space, spaces);
            }
            return success;
        }

        /***************************************************/
        /* //TODO: Can not use Spaces from Building, (line 80 bHoMBuilding.Spaces)
        private bool Create(BH.oM.Environment.Elements.Building bHoMBuilding)
        {
            TBD.Building tasBuilding = m_TBDDocument.Building;
            tasBuilding.latitude = (float)bHoMBuilding.Latitude;
            tasBuilding.longitude = (float)bHoMBuilding.Longitude;
            tasBuilding.name = bHoMBuilding.Name;
            bool success = true;
            
            foreach (BH.oM.Environment.Elements.Space  aSpace in bHoMBuilding.Spaces)
            {
                success &= Create(aSpace, bHoMBuilding);
            }

            return success;
    } */

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement bHoMBuildingElement)
        {
            TBD.buildingElement tasBuildingElement = m_TBDDocument.Building.AddBuildingElement();
            tasBuildingElement.name = bHoMBuildingElement.Name;
            return true;
        }

        /***************************************************/

        private bool Create(BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties)
        {
            TBD.Construction tasConstruction = m_TBDDocument.Building.AddConstruction(null);
            tasConstruction.name = bHoMBuildingElementProperties.Name;
            //TODO: BuildingElementProperties can not handle Thickness
            //tasConstruction.materialWidth[0] = (float)bHoMBuildingElementProperties.Thickness; //which value in the array shall we use??

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.Panel bHoMBuildingElementPanel)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        private bool Create(BHE.Elements.Opening bHoMBuildingElementOpening)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        private bool Create(BHE.Elements.InternalCondition bHoMInternalCondition)
        {
            TBD.InternalCondition tasInternalCondition = m_TBDDocument.Building.AddIC(null);
            tasInternalCondition.name = bHoMInternalCondition.Name;

            return true;
        }

        /***************************************************/
        /*            
        private bool Create(BHE.Elements.Space bHoMSpace, IEnumerable<BHE.Elements.Space> spaces)
        {

            TBD.zone tasZone = m_TBDDocument.Building.AddZone();
            TBD.room tasRoom = tasZone.AddRoom();
            tasZone = Engine.TAS.Convert.ToTas(bHoMSpace, tasZone);

            //TODO: Can not use BuildingElement from Spaces, (line 80 bHoMSpace.BuildingElements)
            //foreach (BHE.Elements.BuildingElement element in bHoMSpace.BuildingElements)
            {
                //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
                TBD.buildingElement be = m_TBDDocument.Building.AddBuildingElement();

                //Add zoneSrf and convert it
                TBD.zoneSurface tasZoneSrf = tasZone.AddSurface();
                tasZoneSrf = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry, tasZoneSrf);

                //Add roomSrf, create face, get its controlpoints and convert to TAS
                TBD.Polygon tasPolygon = tasRoom.AddSurface().CreatePerimeter().CreateFace();
                tasPolygon = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry.ICurve(), tasPolygon);

                //Set the building Element
                tasZoneSrf.buildingElement = Engine.TAS.Convert.ToTas(element, be, m_TBDDocument.Building);

                //tasZoneSrf.type = BH.Engine.TAS.Query.GetSurfaceType(element, spaces);
                tasZoneSrf.orientation = (float)BH.Engine.Environment.Query.Azimuth(element.BuildingElementGeometry, new BHG.Vector());
                //tasZoneSrf.orientation = BH.Engine.TAS.Query.GetOrientation(element.BuildingElementGeometry, bHoMSpace);
                tasZoneSrf.inclination = (float)BH.Engine.Environment.Query.Tilt(element.BuildingElementGeometry);
                //tasZoneSrf.inclination = BH.Engine.TAS.Query.GetInclination(element.BuildingElementGeometry, bHoMSpace);
            }

            return true;
        }
        */
        /*
        private bool Create(BHE.Elements.Space bHoMSpace, BHE.Elements.Building building)
        {
            
            TBD.zone tasZone = m_TBDDocument.Building.AddZone();
            TBD.room tasRoom = tasZone.AddRoom();
            tasZone = Engine.TAS.Convert.ToTas(bHoMSpace, tasZone);
            //TODO: Change BuildingElements to depend on a list of objects
            foreach (BHE.Elements.BuildingElement element in Query.BuildingElements(building, bHoMSpace))
            {
                //Add zoneSrf and convert it
                TBD.zoneSurface tasZoneSrf = tasZone.AddSurface();
                //BuildingElementGeometry is removed from element.
                tasZoneSrf = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry, tasZoneSrf);
                //MD assign type to be fixed!
                tasZoneSrf.type = BH.Engine.TAS.Query.SurfaceType(element); 

                //Add roomSrf, create face, get its controlpoints and convert to TAS
                TBD.Polygon tasPolygon = tasRoom.AddSurface().CreatePerimeter().CreateFace();
                tasPolygon = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry.ICurve(), tasPolygon);

                //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
                TBD.buildingElement be = BH.Engine.TAS.Query.BuildingElement(m_TBDDocument.Building, element.Name);
                if (be == null)
                {
                    be = m_TBDDocument.Building.AddBuildingElement();
                    //Set the building Element
                     Engine.TAS.Convert.ToTas(element, be, m_TBDDocument.Building);
                }
                tasZoneSrf.buildingElement = be;

                //tasZoneSrf.type = BH.Engine.TAS.Query.GetSurfaceType(element, spaces);
                tasZoneSrf.orientation = (float)BH.Engine.Environment.Query.Azimuth(element.BuildingElementGeometry, new BHG.Vector());
                //tasZoneSrf.orientation = BH.Engine.TAS.Query.GetOrientation(element.BuildingElementGeometry, bHoMSpace);
                tasZoneSrf.inclination = (float)BH.Engine.Environment.Query.Tilt(element.BuildingElementGeometry);
                //tasZoneSrf.inclination = BH.Engine.TAS.Query.GetInclination(element.BuildingElementGeometry, bHoMSpace);
            }

            return true;
        */
    }

    /***************************************************/

}

