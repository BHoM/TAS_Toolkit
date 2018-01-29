using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BHE = BH.oM.Environmental;

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
            m_TBDDocumentInstance.close();

            return success;
            
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

            TBD.zoneSurface tasZoneSurface = m_TBDDocumentInstance.Building.AddZone().AddSurface();
            tasZoneSurface.area = 455;

            //TBD.RoomSurface currRoomSrf = tasZoneSurface.GetRoomSurface(0);
            //TBD.Perimeter currPerimeter = currRoomSrf.GetPerimeter();
            //TBD.Polygon currPolygon = currPerimeter.GetFace();

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
            tasZone.output = 1;
            
            //TBD.zoneSurface tasZoneSurface = tasZone.AddSurface();
            //TBD.RoomSurface roomSrf = tasZone.AddRoom().AddSurface();
            //TBD.zoneSurface tasZoneSurface = (TBD.zoneSurface)roomSrf;
            //TBD.zone currZone = tasZoneSurface.zone;
            //roomSrf.area = 225;

            return true;
        }

      



    }
}
