using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Environmental;

namespace BH.Engine.TAS.Helpers
{
    public static class Helper
    {
        /***************************************************/
        /****    Enum converter.Material Types          ****/
        /***************************************************/
       
        public static BHE.Elements.MaterialType GetMaterialType(this TBD.material tasMaterial)
        {
            if (tasMaterial.type == 1) // 1 = Opaque
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Opaque;
                return materialtype;
            }
            else if (tasMaterial.type == 2) // 2 = Opaque
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Opaque;
                return materialtype;
            }
            else if (tasMaterial.type == 3) // 3 = Transparent
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Transparent;
                return materialtype;
            }
            else if (tasMaterial.type == 4) // 4 = Gas
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Gas;
                return materialtype;
            }
            else
            {
                BHE.Elements.MaterialType materialtype = BHE.Elements.MaterialType.Opaque;
                return materialtype;
            }
          
        }

        /***************************************************/
        /****   Enum converter. Building Element Types  ****/
        /***************************************************/

        public static BHE.Elements.BuidingElementType GetBuildingElementType(this TBD.buildingElement tasBuildingElement)
        {
            if ((tasBuildingElement.BEType == 1)||(tasBuildingElement.BEType ==2)|| (tasBuildingElement.BEType == 6) || (tasBuildingElement.BEType == 16)) // 1 = Internal Wall, 2 = External Wall, 6 = undergroundwall, 16 = curtain wall
            {
                BHE.Elements.BuidingElementType buildingelementtype = BHE.Elements.BuidingElementType.Wall;
                return buildingelementtype;
            }

            else if ((tasBuildingElement.BEType == 8) || (tasBuildingElement.BEType == 7)) // 8 = ceiling, 7 = undergroundceiling
            {
                BHE.Elements.BuidingElementType buildingelementtype = BHE.Elements.BuidingElementType.Ceiling;
                return buildingelementtype;
            }

            else if (tasBuildingElement.BEType == 3) // 3 = RoofElement
            {
                BHE.Elements.BuidingElementType buildingelementtype = BHE.Elements.BuidingElementType.Roof;
                return buildingelementtype;
            }

            else if ((tasBuildingElement.BEType == 4) || (tasBuildingElement.BEType == 10)) // 4 = InternalFloor, 10 = RaisedFloor, 19 = ExposedFloor
            {
                BHE.Elements.BuidingElementType buildingelementtype = BHE.Elements.BuidingElementType.Floor;
                return buildingelementtype;
            }

            else
            {
                BHE.Elements.BuidingElementType buildingelementtype = BHE.Elements.BuidingElementType.Wall;
                return buildingelementtype;
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


    }
}
