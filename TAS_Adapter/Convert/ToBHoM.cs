using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHG = BH.oM.Geometry;
using TBD;

namespace BH.Adapter.TAS
{
    public static partial class Convert
    {

        ///***************************************/
        ////Object Converters
        ///***************************************/

        public static BHE.Elements.Location ToBHoM(TBD.Building ITasBuilding)
        {
            BHE.Elements.Location BHoMLocation = new BHE.Elements.Location();
            BHoMLocation.Latitude = ITasBuilding.latitude;
            BHoMLocation.Longitude = ITasBuilding.longitude;
            return BHoMLocation;
        }


        public static BHE.Elements.Location ToBHoM(this TBD.BuildingClass TasBuilding)
        {
            BHE.Elements.Location BHoMLocation = new BHE.Elements.Location();
            BHoMLocation.Latitude = TasBuilding.latitude;
            BHoMLocation.Longitude = TasBuilding.longitude;
            return BHoMLocation;
        }
              
        
        /***************************************/
        //Geometry Converters
        /***************************************/

        public static BHG.Point ToBHoMGeometry(TBD.TasPointClass TASPoint)
        {

            BHG.Point BHoMPoint = new BHG.Point();
            BHoMPoint.X = (TASPoint.x);
            BHoMPoint.Y = (TASPoint.y);
            BHoMPoint.Z = (TASPoint.z);
            return BHoMPoint;
        }

        //***************************************/

        public static BHE.Elements.Panel ToBHoM(TBD.zoneSurfaceClass TASSurface)
        {
            BHE.Elements.Panel BHoMPanel = new BHE.Elements.Panel();
            BHoMPanel.Area = TASSurface.area;
            return BHoMPanel;
                       
        }
                
    }
}
