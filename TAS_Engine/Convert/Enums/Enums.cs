using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Environmental;

namespace BH.Engine.TAS.Enums
{
    public static class Enums
    {
        /***************************************************/
        /****    Enum converter.Material Types          ****/
        /***************************************************/

        public static BHE.Elements.MaterialType GetMaterialType(this TBD.material tasMaterial)
        {
           switch (tasMaterial.type)
            {
                case 1:
                case 2:
                    return BHE.Elements.MaterialType.Opaque;
                case 3:
                    return BHE.Elements.MaterialType.Transparent;
                case 4:
                    return BHE.Elements.MaterialType.Gas;
                default:
                    return BHE.Elements.MaterialType.Opaque;
            }
        }

        /***************************************************/
        /****   Enum converter. Building Element Types  ****/
        /***************************************************/

        public static BHE.Elements.BuidingElementType GetBuildingElementType(this TBD.buildingElement tasBuildingElement)
        {
            switch (tasBuildingElement.BEType)
            {
                case 1:
                case 2:
                case 6:
                    return BHE.Elements.BuidingElementType.Wall;
                case 3:
                    return BHE.Elements.BuidingElementType.Roof;
                case 7:
                case 8:
                    return BHE.Elements.BuidingElementType.Ceiling;
                case 4:
                case 10:
                case 19:
                    return BHE.Elements.BuidingElementType.Floor;
                default:
                    return BHE.Elements.BuidingElementType.Wall;
            }
        }


        //Other building elements (not implemented yet):
        //SHADEELEMENT = 5,
        //SLABONGRADE = 11,
        //GLAZING = 12,
        //ROOFLIGHT = 13,
        //DOORELEMENT = 14,
        //FRAMEELEMENT = 15,
        //NULLELEMENT = 17,
        //SOLARPANEL = 18,
        //VEHICLEDOOR = 20

        /***************************************************/
        /****    Enum converter.Profile Types           ****/
        /***************************************************/

    
    }
}
