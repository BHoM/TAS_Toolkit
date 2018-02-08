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
            m_TBDDocumentInstance.close();
     
            if (m_TBDDocumentInstance != null)
            {
                // issue with closing files and not closing 
                ClearCOMObject(m_TBDDocumentInstance);
                m_TBDDocumentInstance = null;
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

            TBD.room tasRoom = tasZone.AddRoom();

            List<BHE.Elements.BuildingElementPanel> bHoMPanels = bHoMSpace.BuildingElementPanel;
            for (int i = 0; i< bHoMPanels.Count;i++)
            {
                TBD.Polygon tasPolygon = tasRoom.AddSurface().CreatePerimeter().CreateFace();
                List<BHG.Point> bHoMPoints = BH.Engine.Geometry.Query.ControlPoints(bHoMPanels[i].PolyCurve);

                for (int j = 0; j < bHoMPoints.Count - 1; j++)
                {
                    int tasCoord = tasPolygon.AddCoordinate((float)bHoMPoints[j].X, (float)bHoMPoints[j].Y, (float)bHoMPoints[j].Z);
                    TBD.TasPoint tasPt = tasPolygon.AddPoint();
                    tasPt = Engine.TAS.Convert.ToTas(bHoMPoints[j], tasPt);
                }

                double test0 = tasZone.GetRoom(0).GetSurface(0).GetPerimeter().GetFace().GetPoint(0).x;
                double test1 = tasZone.GetRoom(0).GetSurface(0).GetPerimeter().GetFace().GetPoint(1).x;
 
                //We have to add exactly ALL of the properties to a zonesurface before we save the file. Otherwise we end up with a corruped file!
                var myZoneSrf = tasZone.AddSurface();
                myZoneSrf.number = i;
                myZoneSrf.orientation = 270;
                myZoneSrf.GUID = bHoMSpace.BHoM_Guid.ToString();
                myZoneSrf.area = 150;
                TBD.buildingElement be = m_TBDDocumentInstance.Building.AddBuildingElement();
                be.BEType = (int)TBD.BuildingElementType.INTERNALWALL;
                myZoneSrf.buildingElement =be;
                myZoneSrf.buildingElement.name = "External Wall";
                myZoneSrf.GetRoomSurface(i);


            }

            return true;
        }

        /***************************************************/

        private bool Create(List<BHG.Point> bHoMPoint)
        {

            TBD.zone tasZone = m_TBDDocumentInstance.Building.AddZone();
            TBD.room tasRoom = tasZone.AddRoom();

            TBD.Polygon tasPolygon = tasRoom.AddSurface().CreatePerimeter().GetFace();

            
            int pointIndex = 0;
            while (bHoMPoint[pointIndex] != null)
            {
                TBD.TasPoint tasPt = tasPolygon.AddPoint();
                tasPt = Engine.TAS.Convert.ToTas(bHoMPoint[pointIndex], tasPt);
            }


            return true;
        }





    }
}
