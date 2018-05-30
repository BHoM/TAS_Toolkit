using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BH.oM.Environmental.Elements;
using BH.oM.Environmental.Properties;
using BH.oM.Environmental.Interface;
using BHG = BH.oM.Geometry;
using TBD;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {

        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static TBD.TasPoint ToTas(this BHG.Point bHoMPoint, TasPoint tasPoint)
        {
            tasPoint.x = (float)(bHoMPoint.X);
            tasPoint.y = (float)(bHoMPoint.Y);
            tasPoint.z = (float)(bHoMPoint.Z);
            return tasPoint;
        }

        /***************************************************/

        public static TBD.Polygon ToTas(this BHG.ICurve bHoMPolyCurve, Polygon tasPolygon)
        {

            List<BHG.Point> bHoMPoints = Engine.Geometry.Query.IControlPoints(bHoMPolyCurve);

            for (int j = 0; j < bHoMPoints.Count - 1; j++)
            {
                TBD.TasPoint tasPt = tasPolygon.AddPoint();
                tasPt = Engine.TAS.Convert.ToTas(bHoMPoints[j], tasPt);
            }

            return tasPolygon;
        }

        /***************************************************/

        public static TBD.zoneSurface ToTas(this IBuildingElementGeometry bHoMPanel, zoneSurface tasZoneSrf)
        {

            //tasZoneSrf.orientation = Query.GetOrientation(bHoMPanel);
            //tasZoneSrf.inclination = Query.GetInclination(bHoMPanel);

            tasZoneSrf.altitude = (float)BH.Engine.Environment.Query.Altitude(bHoMPanel);
            tasZoneSrf.altitudeRange = (float)BH.Engine.Environment.Query.AltitudeRange(bHoMPanel);
            tasZoneSrf.GUID = bHoMPanel.BHoM_Guid.ToString();
            tasZoneSrf.area = (float)Geometry.Query.IArea((bHoMPanel.ICurve()));

            //tasZoneSrf.type = IToTas(bHoMPanel);
            //tasZoneSrf.type = SurfaceType.tbdLink;
            
            return tasZoneSrf;
        }

        /***************************************************/

        public static TBD.zone ToTas(this Space bHoMSpace, zone tasZone)
        {
            tasZone.name = bHoMSpace.Number + " " + bHoMSpace.Name;
            tasZone.floorArea = Query.GetFloorArea(bHoMSpace);
            tasZone.GUID = bHoMSpace.BHoM_Guid.ToString();
            tasZone.volume = Query.GetVolume(bHoMSpace);

            return tasZone;
        }

        /***************************************************/

        public static TBD.InternalCondition ToTas(this BHE.Elements.InternalCondition bHoMIC, TBD.InternalCondition tasIC)
        {
            tasIC.name = bHoMIC.Name;
            return tasIC;
        }

        /***************************************************/

        public static TBD.buildingElement ToTas(this BHE.Elements.BuildingElement bHoMBuildingElement, TBD.buildingElement tasBuildingElement, TBD.Building building)
        {
    
            tasBuildingElement.name = bHoMBuildingElement.Name;
            //TAS.Adapter
            tasBuildingElement.BEType = BH.Engine.TAS.Convert.ToTBDBEType(bHoMBuildingElement);
            //tasBuildingElement.colour = bHoMBuildingElement
            tasBuildingElement.AssignConstruction(BH.Engine.TAS.Convert.ToTBDBEConstruction(bHoMBuildingElement, building));
            return tasBuildingElement;

        }


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

        //public static TBD.ConstructionClass ToTas(this BuildingElementProperties bHoMBuildingElementProperties)
        //{

        //    TBD.ConstructionClass tasConstruction = new TBD.ConstructionClass
        //    {
        //        name = bHoMBuildingElementProperties.Name
        //    };

        //    return tasConstruction;
        //}

        // if we use TBD template assign construction
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

        public static TBD.MaterialTypes ToTas(this BHE.Elements.MaterialType bHoMMaterialType)
        {
            switch (bHoMMaterialType)
            {
                case BHE.Elements.MaterialType.Opaque:
                    return MaterialTypes.tcdOpaqueMaterial;
                case BHE.Elements.MaterialType.Transparent:
                    return MaterialTypes.tcdTransparentLayer;
                case BHE.Elements.MaterialType.Gas:
                    return MaterialTypes.tcdGasLayer;
                default:
                    return MaterialTypes.tcdOpaqueMaterial;
            }
        }

        /***************************************************/

        public static TBD.BuildingElementType ToTas(this BHE.Elements.BuildingElementType bHoMBuildingElementType)
        {
            switch (bHoMBuildingElementType) // This is just a test, it doeas not match. We have more BETypes in Tas than in BHoM
            // here we will need to have two levels or recognision ASHRAEBuilidingElementType as per new idraw graph
            {
                case BHE.Elements.BuildingElementType.Wall:
                    return TBD.BuildingElementType.EXTERNALWALL; //What about the other TBD Wall types??
                case BHE.Elements.BuildingElementType.Roof:
                    return TBD.BuildingElementType.ROOFELEMENT;
                case BHE.Elements.BuildingElementType.Ceiling:
                    return TBD.BuildingElementType.UNDERGROUNDCEILING;
                case BHE.Elements.BuildingElementType.Floor:
                    return TBD.BuildingElementType.INTERNALFLOOR;
                default:
                    return TBD.BuildingElementType.EXTERNALWALL;
            }
        }

        /***************************************************/

        public static TBD.SurfaceType ToTas(this BHE.Elements.BuildingElementPanel bHoMSurface)
        {
            //Should we implement an enum for surface types in BHoM?? 

            if (bHoMSurface.ElementType == "Ground")
                return SurfaceType.tbdGround;
            else if (bHoMSurface.ElementType == "Exposed")
                return SurfaceType.tbdExposed;
            if (bHoMSurface.ElementType == "Internal")
                return SurfaceType.tbdInternal;
            else if (bHoMSurface.ElementType == "Link")
                return SurfaceType.tbdLink;
            if (bHoMSurface.ElementType == "Ground")
                return SurfaceType.tbdGround;
            else
                return SurfaceType.tbdNullLink; //Adiabatic

        }


        /***************************************************/

        public static TBD.SurfaceType ToTas(this BHE.Elements.BuildingElementCurve bHoMSurface)
        {
            return SurfaceType.tbdNullLink;
        }

        /***************************************************/

        public static TBD.SurfaceType IToTas(this BHE.Interface.IBuildingElementGeometry bHoMSurface)
        {
            return ToTas(bHoMSurface as dynamic);
        }

        /***************************************************/
    }
}
