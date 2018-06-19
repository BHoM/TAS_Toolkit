using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental.Elements;
using BHEE = BH.oM.Environmental.Elements;
using BHEI = BH.oM.Environmental.Interface;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<BHE.Space> GetAdjacentSpace(this BHE.BuildingElement bHoMBuildingElement, List<BHE.Space> spaces)
        {
            List<BHE.Space> adSpace = new List<BHE.Space>();

            foreach (Guid adjSpace in bHoMBuildingElement.AdjacentSpaces)
            {
                if (spaces.Select(x => x.BHoM_Guid).Contains(adjSpace))
                {
                    BHE.Space foundSpace = spaces.Find(x => x.BHoM_Guid == adjSpace);
                    if (foundSpace == null)
                        continue;
                    adSpace.Add(foundSpace);
                }
            }
            return adSpace;
        }

        /***************************************************/

        public static List<Guid> GetAdjacentSpace(this List<BHE.BuildingElement> bHoMBuildingElement)
        {
            List<Guid> adjSpace = new List<Guid>();

            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                adjSpace.Add(element.AdjacentSpaces[0]);
            }

            return adjSpace;

            /***************************************************/
        }

    }
}
