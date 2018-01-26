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
            tasBuilding.name = "Test Name";

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement bHoMBuildingElement)
        {
            TBD.buildingElement tasBuildingElement = m_TBDDocumentInstance.Building.AddBuildingElement();
            tasBuildingElement.name = bHoMBuildingElement.Name;
            //tasBuildingElement.BEType = bHoMBuildingElement.BuildingElementProperties.BuildingElementType
            return true;
        }

        /***************************************************/

        //private bool Create(BHE.Elements.BuildingElementPanel bHoMBuildingElementPanel)
        //{
        //    TBD.zoneSurface tasZoneSurface = m_TBDDocumentInstance.Building.AddZone().AddSurface();
        //    tasZoneSurface.area = 45;
        //    return true;
        //}


        /***************************************************/

        private bool Create(BHE.Elements.Space bHoMSpace)
        {
            //TBD.zone tasZone = m_TBDDocumentInstance.Building.AddZone();
            //tasZone.name = bHoMSpace.Name;
            return true;
        }

      



    }
}
