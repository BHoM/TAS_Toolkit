using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;
using BH.Engine.Geometry;
using TBD;
using System;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        public static int ToTBDBEType(this BHE.Elements.BuildingElement bHoMBuildingElement)
        {
            //Air= 17
            int type = 17;
            if (bHoMBuildingElement == null)
                return type;
            else if (bHoMBuildingElement.AdjacentSpaces.Count == 0 && bHoMBuildingElement.BuildingElementProperties.BuildingElementType != BHE.Elements.BuildingElementType.Window && bHoMBuildingElement.BuildingElementProperties.BuildingElementType != BHE.Elements.BuildingElementType.Door)
                // shade = 5
                type = 5;

            if (bHoMBuildingElement.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
            {
                object aObject = bHoMBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"];
                if (aObject != null)
                    type = ToTBDSurfaceType(aObject.ToString()); //modifies the string
            }

            //else if (bHoMBuildingElement != null)
            //{
            //    type = ToTBDSurfaceType((bHoMBuildingElement.BuildingElementGeometry as BHE.Elements.BuildingElementPanel).ElementType);
            //}
            else
                type = 17;

            return type;
        }

        /***************************************************/
        //String modification for surface types
        public static int ToTBDSurfaceType(this string type)
        {
            switch (type)

            {
                //Strings from Revit
                case "No Type":
                    return 0;
                case "Internal Wall":
                    return 1;
                case "External Wall":
                    return 2;
                case "Roof":
                    return 3;
                case "Internal Floor":
                    return 4;
                case "Shade":
                    return 5;
                case "Underground Wall":
                    return 6;
                case "Underground Slab":
                    return 7;
                case "Internal Ceiling":
                    return 8;
                case "Underground Ceiling":
                    return 9;
                case "Raised Floor":
                    return 10;
                case "Slab on Grade":
                    return 11;
                case "Glazing":
                    return 12;
                case "Rooflight":
                    return 13;
                case "Door":
                    return 14;
                case "Frame":
                    return 15;
                case "Curtain Wall":
                    return 16;
                case "Air":
                    return 17;
                case "Solar / PV Panel":
                    return 18;
                case "Exposed Floor":
                    return 19;
                case "Vehicle Door":
                    return 20;

                default:
                    return 0; //Adiabatic
            }
        }

        ///***************************************************/
        ///
        public static String ToTBDSurfaceType(this int type)
        {
            switch (type)

            {
                //Strings from Revit
                case 0:
                    return "Adiabatic";
                case 1:
                    return "Internal Wall";
                case 2:
                    return "External Wall";
                case 3:
                    return "Roof";
                case 4:
                    return "Internal Floor";
                case 5:
                    return "Shade";
                case 6:
                    return "Underground Wall";
                case 7:
                    return "Underground Slab";
                case 8:
                    return "Internal Ceiling";
                case 9:
                    return "Underground Ceiling";
                case 10:
                    return "Raised Floor";
                case 11:
                    return "Slab on Grade";
                case 12:
                    return "Glazing";
                case 13:
                    return "Rooflight";
                case 14:
                    return "Door";
                case 15:
                    return "Frame";
                case 16:
                    return "Curtain Wall";
                case 17:
                    return "Adiabatic";
                case 18:
                    return "Solar / PV Panel";
                case 19:
                    return "Exposed Floor";
                case 20:
                    return "Vehicle Door";

                default:
                    return "Adiabatic"; //0
            }
        }

    }
}