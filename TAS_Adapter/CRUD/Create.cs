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
 
                //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
                var myZoneSrf = tasZone.AddSurface();
                myZoneSrf = Engine.TAS.Convert.ToTas(bHoMPanels[i], myZoneSrf);

                myZoneSrf.number = i;
               
                TBD.buildingElement be = m_TBDDocumentInstance.Building.AddBuildingElement();
                be.BEType = (int)TBD.BuildingElementType.INTERNALWALL;
                myZoneSrf.buildingElement =be;

                if (myZoneSrf.orientation==0) // if floor
                {
                    tasZone.floorArea = (float)Engine.Geometry.Query.Area(bHoMPanels[i].PolyCurve);
                }
                

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
