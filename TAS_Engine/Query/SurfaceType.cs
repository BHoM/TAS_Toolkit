using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;
using BHEI = BH.oM.Environmental.Interface;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static TBD.SurfaceType SurfaceType (BHEE.BuildingElement buildingElement)
        {
            object aValue;
            if(buildingElement.BuildingElementProperties.CustomData.TryGetValue("SAM_BuildingElementType", out aValue))
            {
                if(aValue is string)
                {
                    string aSurfaceTypeString = (string)aValue;

                    if (buildingElement.AdjacentSpaces.Count == 1)
                    {
                        if (aSurfaceTypeString == "Shade")
                            return TBD.SurfaceType.tbdNullLink;

                        if (aSurfaceTypeString == "External Wall")
                            return TBD.SurfaceType.tbdExposed;
                    }

                    if (buildingElement.AdjacentSpaces.Count > 1)
                    {
                        if (aSurfaceTypeString == "Air")
                            return TBD.SurfaceType.tbdNullLink;

                        if (aSurfaceTypeString == "Internal Floor")
                            return TBD.SurfaceType.tbdLink;
                    }
                }
            }

            return TBD.SurfaceType.tbdNullLink;
        }

        /***************************************************/
    }
}
