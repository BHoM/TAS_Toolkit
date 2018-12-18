using System.ComponentModel;
using System.Collections.Generic;

using BH.oM.Base;
using BH.oM.Reflection.Attributes;
using BH.oM.Environment.Elements;

namespace BH.Engine.TAS
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Sets Tag for BHoMObject.")]
        [Input("bHoMObject", "BHoMObject")]
        [Input("tag", "tag to be set")]
        [Output("IBHoMObject")]
        public static BuildingElementType FixBuilidingElementType(this TBD.buildingElement tbdBuildingElement, TBD.zoneSurface tbdZoneSurface, BuildingElementType bHoMBuildingElementType)
        {
            if (bHoMBuildingElementType == oM.Environment.Elements.BuildingElementType.Undefined)
            {
                if (tbdBuildingElement.name.Contains("-frame"))
                {
                    if (tbdZoneSurface.inclination == 0)
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.RooflightWithFrame;
                    else
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.WindowWithFrame;

                }
                else if (tbdBuildingElement.name.Contains("Floor"))
                {
                    bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Floor;
                }
                else if (tbdBuildingElement.name.Contains("Wall"))
                {
                    bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Wall;
                }

                else if ((tbdBuildingElement.name == "Air") || (tbdBuildingElement.name == "Air-internal"))
                {
                    if (tbdZoneSurface.inclination == 0 || tbdZoneSurface.inclination == 180)
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Floor;
                    else
                        bHoMBuildingElementType = oM.Environment.Elements.BuildingElementType.Wall;
                }
            }


            return bHoMBuildingElementType;
        }

        /***************************************************/
    }
}
