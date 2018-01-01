using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BH.oM.Environmental.Elements;
using BHG = BH.oM.Geometry;
using TBD;
using TAS3D;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override IEnumerable<BHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(Panel))
                return ReadPanels();
            else if (type == typeof(BHE.Elements.Location))
                return ReadLocation();
            else if (type == typeof(Space))
                return ReadZones();
            else
                return null;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Space> ReadZones(List<string> ids = null)
        {
            List<Space> bHoMSpace = new List<Space>();

            int zoneIndex = 0;
            while (m_TBDDocumentInstance.Building.GetZone(zoneIndex) != null)
            {
                TBD.zone zone = m_TBDDocumentInstance.Building.GetZone(zoneIndex);
                bHoMSpace.Add(Engine.TAS.Convert.ToBHoM(zone));
                zoneIndex++;
            }                
                       
            return bHoMSpace;
        }
                
        /***************************************************/

        private List<BHE.Elements.Location> ReadLocation(List<string> ids = null)
        {
            TBD.Building building = m_TBDDocumentInstance.Building;
            List<BHE.Elements.Location> BHoMLocation = new List<BHE.Elements.Location>();
            BHoMLocation.Add(Engine.TAS.Convert.ToBHoM(building));
                      
            return BHoMLocation;
        }

        /***************************************************/
               
        private List<Panel> ReadPanels(List<string> ids = null)
        {
            
            List<Panel> bHoMPanels = new List<Panel>();

            int zoneIndex = 0;
            while (m_TBDDocumentInstance.Building.GetZone(zoneIndex) != null)
            {
                int panelIndex = 0;
                while (m_TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(panelIndex) != null)
                {
                    TBD.zoneSurface zonesurface = m_TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(panelIndex);

                    try
                    {
                        //Get edges as polylines for the Tas Surfaces
                        TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(0);
                        TBD.Perimeter currPerimeter = currRoomSrf.GetPerimeter();
                        TBD.Polygon currPolygon = currPerimeter.GetFace();
                                                            
                        BHG.Polyline edges = Engine.TAS.Convert.ToBHoM(currPolygon);
                        bHoMPanels.Add(Engine.TAS.Convert.ToBHoM(zonesurface, edges));
                    }

                    //If we have air walls we will get a NullReferenceException. Tas does not count air walls as surfaces 
                    catch (NullReferenceException e)
                    {
                        int error = panelIndex;
                        Console.WriteLine(e);
                        //throw e;
                                               
                    }                                                     
                    panelIndex++;
                }
                zoneIndex++;                
            }
            return bHoMPanels;
        }

        /***************************************************/
    }
}
