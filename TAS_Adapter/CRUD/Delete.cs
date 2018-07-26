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
        /**** Public Methods                            ****/
        /***************************************************/

        //private bool Delete(BHE.Elements.Space bHoMSpace, IEnumerable<BHE.Elements.Space> spaces)
        //{

        //    TBD.zone tasZone = m_TBDDocument.Building.AddZone();
        //    TBD.room tasRoom = tasZone.AddRoom();
        //    tasZone = Engine.TAS.Convert.ToTas(bHoMSpace, tasZone);


        //    foreach (BHE.Elements.BuildingElement element in bHoMSpace.BuildingElements)
        //    {
        //        //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
        //        TBD.buildingElement be = m_TBDDocument.Building.AddBuildingElement();

        //        //Add zoneSrf and convert it
        //        TBD.zoneSurface tasZoneSrf = tasZone.AddSurface();
        //        tasZoneSrf = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry, tasZoneSrf);

        //        //Add roomSrf, create face, get its controlpoints and convert to TAS
        //        TBD.Polygon tasPolygon = tasRoom.AddSurface().CreatePerimeter().CreateFace();
        //        tasPolygon = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry.ICurve(), tasPolygon);

        //        //Set the building Element
        //        tasZoneSrf.buildingElement = Engine.TAS.Convert.ToTas(element, be);

        //        //tasZoneSrf.type = BH.Engine.TAS.Query.GetSurfaceType(element, spaces);
        //        tasZoneSrf.orientation = (float)BH.Engine.Environment.Query.Azimuth(element.BuildingElementGeometry, new BHG.Vector());
        //        //tasZoneSrf.orientation = BH.Engine.TAS.Query.GetOrientation(element.BuildingElementGeometry, bHoMSpace);
        //        tasZoneSrf.inclination = (float)BH.Engine.Environment.Query.Tilt(element.BuildingElementGeometry);
        //        //tasZoneSrf.inclination = BH.Engine.TAS.Query.GetInclination(element.BuildingElementGeometry, bHoMSpace);
        //    }

        //    return true;
        //}



        /***************************************************/


    }
}
