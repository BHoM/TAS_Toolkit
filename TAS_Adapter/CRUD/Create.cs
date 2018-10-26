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
        /*//TODO: Can not use Spaces from Building, (line 80 bHoMBuilding.Spaces)
        private bool Create(BH.oM.Environment.Elements.Building building)
        {
            TBD.Building tbdBuilding = m_TBDDocument.Building;
            tbdBuilding.latitude = (float)building.Latitude;
            tbdBuilding.longitude = (float)building.Longitude;
            tbdBuilding.name = building.Name;
            bool success = true;
            
            foreach (BH.oM.Environment.Elements.Space  aSpace in building.Spaces)
            {
                success &= Create(aSpace, building);
            }

            return success;
    } */

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement buildingElement)
        {
            TBD.buildingElement tbdBuildingElement = tbdDocument.Building.AddBuildingElement();
            tbdBuildingElement.name = buildingElement.Name;
            return true;
        }

        /***************************************************/

        private bool Create(BHE.Properties.BuildingElementProperties buildingElementProperties)
        {
            TBD.Construction tbdConstruction = tbdDocument.Building.AddConstruction(null);
            tbdConstruction.name = buildingElementProperties.Name;
            //TODO: BuildingElementProperties can not handle Thickness
            //tbdConstruction.materialWidth[0] = (float)buildingElementProperties.Thickness; //which value in the array shall we use??

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
            TBD.InternalCondition tbdInternalCondition = tbdDocument.Building.AddIC(null);
            tbdInternalCondition.name = internalCondition.Name;

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

