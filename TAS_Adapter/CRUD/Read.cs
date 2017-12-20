using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using BH.oM.Environmental.Elements;
using BHG = BH.oM.Geometry;
using BH.Adapter.TAS;
using TBD;
using TAS3D;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Adapter Methods                           ****/
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
        /**** Protected Methods                         ****/
        /***************************************************/

        public List<Space> ReadZones(List<string> ids = null)
        {
            List<Space> BHoMSpace = new List<Space>();

            int zoneIndex = 0;
            while (TBDDocumentInstance.Building.GetZone(zoneIndex) != null)
            {
                TBD.zone zone = TBDDocumentInstance.Building.GetZone(zoneIndex);
                BHoMSpace.Add(Convert.ToBHoM(zone));
                zoneIndex++;
            }                
                       
            return BHoMSpace;
        }
                
        /***************************************************/

        public List<BHE.Elements.Location> ReadLocation(List<string> ids = null)
        {
            TBD.Building building = TBDDocumentInstance.Building;
            List<BHE.Elements.Location> BHoMLocation = new List<BHE.Elements.Location>();
            BHoMLocation.Add(Convert.ToBHoM(building));
                      
            return BHoMLocation;
        }

        /***************************************************/
               
        public List<Panel> ReadPanels(List<string> ids = null)
        {
            
            List<Panel> BHoMPanels = new List<Panel>();

            int zoneIndex = 0;
            while (TBDDocumentInstance.Building.GetZone(zoneIndex) != null)
            {
                int panelIndex = 0;
                while (TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(panelIndex) != null)
                {
                    TBD.zoneSurface zonesurface = TBDDocumentInstance.Building.GetZone(zoneIndex).GetSurface(panelIndex);

                    //Get edges as polylines for the Tas Surfaces
                    TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(0);
                    TBD.Perimeter currPerimeter = currRoomSrf.GetPerimeter();
                    TBD.Polygon currPolygon = currPerimeter.GetFace();

                    int pointIndex = 0;
                    while (currPolygon.GetPoint(pointIndex) != null)
                    {
                        TBD.TasPoint currPoint = currPolygon.GetPoint(pointIndex);
                        BHG.Point controlPoint = Convert.ToBHoM(currPoint);
                        pointIndex++;

                        BHoMPanels.Add(Convert.ToBHoM(zonesurface, controlPoint)); //TODO: change input to polyline
                    }
             

                    panelIndex++;
                }

                zoneIndex++;                
            }

            return BHoMPanels;
        }

        /***************************************************/

        
    }
}
