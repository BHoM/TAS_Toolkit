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

                    if (buildingElement.AdjacentSpaces.Count == 0)
                    {
                        if (aSurfaceTypeString == "Shade")
                            return TBD.SurfaceType.tbdExposed;

                        //if (aSurfaceTypeString == "Rooflight")
                        //    return TBD.SurfaceType.; Window

                        //if (aSurfaceTypeString == "Glazing")
                        //    return TBD.SurfaceType.; Window

                        //if (aSurfaceTypeString == "Door")
                        //    return TBD.SurfaceType.; Window _offset zero

                        // // we take window from revit and frame is transform in tas
                        //if (aSurfaceTypeString == "Frame")
                        //    return TBD.SurfaceType.; 

                        //if (aSurfaceTypeString == "Solar / PV Panel")
                        //    return TBD.SurfaceType.;
                    }

                    if (buildingElement.AdjacentSpaces.Count == 1)
                    {

                        if (aSurfaceTypeString == "No Type")
                            return TBD.SurfaceType.tbdNullLink;

                        if (aSurfaceTypeString == "Exposed Floor")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Raised Floor")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Curtain Wall")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Roof")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "External Wall")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Slab on Grade")
                            return TBD.SurfaceType.tbdGround;

                        if (aSurfaceTypeString == "Underground Wall")
                            return TBD.SurfaceType.tbdGround;
                    }

                    if (buildingElement.AdjacentSpaces.Count > 1)
                    {
                        if (aSurfaceTypeString == "No Type")
                            return TBD.SurfaceType.tbdNullLink;

                        if (aSurfaceTypeString == "Internal Wall")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Internal Floor")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Exposed Floor")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Underground Ceiling")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Internal Ceiling")
                            return TBD.SurfaceType.tbdLink;
                    }
                }
            }

            return TBD.SurfaceType.tbdNullLink;
        }

        /***************************************************/
    }
}
