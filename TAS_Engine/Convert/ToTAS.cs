using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHS = BH.oM.Structural;
using BH.oM.Environmental.Elements;
using BH.oM.Environmental.Properties;
using BH.oM.Environmental.Interface;
using BHG = BH.oM.Geometry;
using BH.Engine;
using TBD;
using BH.Adapter.TAS;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        //public static string ToTas(this Type type)
        //{
        //    if (type == typeof(Construction))
        //        return "Construction";
        //    return null;
        //}

        /***************************************************/


        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        //public static TBD.TasPoint ToTas(this BHG.Point bHoMPoint)
        //{
        //    TBD.TasPoint tasPoint = new TBD.TasPoint();
        //    tasPoint.x = (float)(bHoMPoint.X);
        //    tasPoint.y = (float)(bHoMPoint.Y);
        //    tasPoint.z = (float)(bHoMPoint.Z);
        //    return tasPoint;
        //}

        /***************************************************/

        //public static TBD.Polygon ToTas(this BHG.Polyline bHoMPolyline)
        //{

        //    TBD.Polygon tasPolygon = new TBD.Polygon();
        //    List<BHG.Point> coordList = bHoMPolyline.ControlPoints;

        //    for (int i = 0; i < coordList.Count; i++)
        //    {
        //        tasPolygon.AddCoordinate((float)coordList[i].X, (float)coordList[i].Y, (float)coordList[i].Z);
        //    }

        //    return tasPolygon;
        //}

        /***************************************************/

        //public static TBD.zoneSurface ToTas(this BHE.Elements.BuildingElementPanel bHoMPanel)
        //{
        //    TBD.zoneSurface tasSurface = new TBD.zoneSurface();
        //    //add points as properties to the surface
        //    //tasSurface.area = (float)bHoMPanel.Surface.IArea();
        //    return tasSurface;
        //}

        /***************************************************/


        /***************************************************/
        /**** Public Methods - Objects                  ****/
        /***************************************************/

        //public static TBD.material ToTas(this BHE.Elements.OpaqueMaterial bHoMOpaqueMaterial)
        //{
        //    TBD.material tasMaterial = new TBD.material
        //    {
        //        name = bHoMOpaqueMaterial.Name,
        //        description = bHoMOpaqueMaterial.Description,
        //        width = (float)bHoMOpaqueMaterial.Thickness,
        //        conductivity = (float)bHoMOpaqueMaterial.Conductivity,
        //        vapourDiffusionFactor = (float)bHoMOpaqueMaterial.VapourDiffusionFactor,
        //        externalSolarReflectance = (float)bHoMOpaqueMaterial.SolarReflectanceExternal,
        //        internalSolarReflectance = (float)bHoMOpaqueMaterial.SolarReflectanceInternal,
        //        externalLightReflectance = (float)bHoMOpaqueMaterial.LightReflectanceExternal,
        //        internalLightReflectance = (float)bHoMOpaqueMaterial.LightReflectanceInternal,
        //        externalEmissivity = (float)bHoMOpaqueMaterial.EmissivityExternal,
        //        internalEmissivity = (float)bHoMOpaqueMaterial.EmissivityInternal
        //    };

        //    return tasMaterial;
        //}

        /***************************************************/

        public static TBD.ConstructionClass ToTas(this BuildingElementProperties bHoMBuildingElementProperties)
        {

            
            TBD.ConstructionClass tasConstruction = new TBD.ConstructionClass
            {
                name = bHoMBuildingElementProperties.Name
            };


            return tasConstruction;
        }


       

        /***************************************************/


        /***************************************************/
        /**** Public Methods - Enums                    ****/
        /***************************************************/





    }
}
