using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;

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

            if (typeof(IObject).IsAssignableFrom(typeof(T)))
            {

                foreach (T obj in objects)
                {
                    success &= Create(obj as dynamic);
                }
            }

            m_TBDDocumentInstance.save();

          
     
                if (m_TBDDocumentInstance != null)
                {
                    // issue with closing files and not closing 
                    ClearCOMObject(m_TBDDocumentInstance);
                    m_TBDDocumentInstance = null;
                }


            return success; 

        }


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

        private bool Create(BHE.Elements.Building bHoMBuilding)
        {
            TBD.Building tasBuilding = m_TBDDocumentInstance.Building;
            tasBuilding.latitude = (float)bHoMBuilding.Latitude;
            tasBuilding.longitude = (float)bHoMBuilding.Longitude;
            tasBuilding.name = bHoMBuilding.Name;

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement bHoMBuildingElement)
        {
            TBD.buildingElement tasBuildingElement = m_TBDDocumentInstance.Building.AddBuildingElement();
            tasBuildingElement.name = bHoMBuildingElement.Name;
            return true;
        }

        /***************************************************/

        private bool Create(BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties)
        {
            TBD.Construction tasConstruction = m_TBDDocumentInstance.Building.AddConstruction(null);
            tasConstruction.name = bHoMBuildingElementProperties.Name;
            tasConstruction.materialWidth[0] = (float)bHoMBuildingElementProperties.Thickness; //which value in the array shall we use??

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElementPanel bHoMBuildingElementPanel)
        {
            TBD.zone tasZone = m_TBDDocumentInstance.Building.GetZone(0); //We have to create spaces before we can create its panels
            //BD. tasRoom = tasZone.AddRoom();
            //TBD.room tasRoom = tasZone.AddRoom();
            //TBD.RoomSurface roomsrf = tasRoom.AddSurface();
            //TBD.zoneSurface zonesrf = tasZone.AddSurface();

            return true;
        }



        /***************************************************/

        private bool Create(BHE.Elements.InternalCondition bHoMInternalCondition)
        {
            TBD.InternalCondition tasInternalCondition = m_TBDDocumentInstance.Building.AddIC(null);
            tasInternalCondition.name = bHoMInternalCondition.Name;

            return true;
        }


        /***************************************************/

        private bool Create(BHE.Elements.Space bHoMSpace)
        {
            TBD.zone tasZone = m_TBDDocumentInstance.Building.AddZone();
            tasZone.name = bHoMSpace.Name;
            tasZone.volume = (float)bHoMSpace.Volume;
            tasZone.floorArea = (float)bHoMSpace.Area;

            TBD.TasPoint tasPt = tasZone.AddRoom().AddSurface().CreatePerimeter().CreateFace().AddPoint();
            

            return true;
        }

        /***************************************************/

        private bool Create(BHG.Point bHoMPoint)
        {

            TBD.zone tasZone = m_TBDDocumentInstance.Building.GetZone(0); //We have to create spaces before we can create its panels

            TBD.room tasRoom = tasZone.AddRoom();
            TBD.RoomSurface tasRoomSrf = tasRoom.AddSurface();
            TBD.Perimeter tasPerimeter = tasRoomSrf.CreatePerimeter();
            TBD.Polygon tasPolygon = tasPerimeter.CreateFace();
            TBD.TasPoint tasPt = tasPolygon.AddPoint();

            List<TBD.TasPoint> tasPoint = new List<TBD.TasPoint>();
            tasPoint.Add(Engine.TAS.Convert.ToTas(bHoMPoint, tasPt));

            return true;
        }





    }
}
