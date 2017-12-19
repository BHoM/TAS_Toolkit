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
               
        /***************************************/

        public static BHE.Elements.Space ToBHoM(TBD.zone ITasZone)
        {
            BHE.Elements.Space BHoMSpace = new BHE.Elements.Space();
            BHoMSpace.Name = ITasZone.name;
            return BHoMSpace;
        }

        /***************************************/

        public static BHE.Elements.Panel ToBHoM(TBD.zoneSurface ITasSurface)
        {
            BHE.Elements.Panel BHoMPanel = new BHE.Elements.Panel();
            BHoMPanel.Area = ITasSurface.area;
            BHoMPanel.Type = ITasSurface.type.ToString();
            return BHoMPanel;
                       
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

       
                
    }
}
