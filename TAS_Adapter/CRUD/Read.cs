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

            int index = 0;
            while (TBDDocumentInstance.Building.GetZone(index) != null)
            {
                TBD.zone zone = TBDDocumentInstance.Building.GetZone(index);
                BHoMSpace.Add(Convert.ToBHoM(zone));
                index++;
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
            TBD.zone zone = TBDDocumentInstance.Building.GetZone(0);
            TBD.zoneSurface zonesurface = zone.GetSurface(0);
            List<Panel> BHoMPanels = new List<Panel>();
            BHoMPanels.Add(Convert.ToBHoM(zonesurface));
            return BHoMPanels;
        }

        /***************************************************/

        
    }
}
