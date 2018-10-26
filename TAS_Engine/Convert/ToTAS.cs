using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Interface;
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
        // first three letters inparameters refers to type of TAS files: 
        // .t3d TAS 3D Modeler
        // .tbd TAS Building Designer
        // .tsd TAS Results Viewer
        // .tpd TAS HVAC System
        // .tcd TAS Construction
        // ie. tbdIC - TAS Building Designer Internal Condition

        public static TBD.TasPoint ToTas(this BHG.Point bHoMPoint, TasPoint tbdPoint)
        {
            tbdPoint.x = (float)(bHoMPoint.X);
            tbdPoint.y = (float)(bHoMPoint.Y);
            tbdPoint.z = (float)(bHoMPoint.Z);
            return tbdPoint;
        }

        /***************************************************/

        public static TBD.Polygon ToTas(this BHG.ICurve bHoMPolyCurve, Polygon tbdPolygon)
        {

            List<BHG.Point> bHoMPoints = Engine.Geometry.Query.IControlPoints(bHoMPolyCurve);

            for (int j = 0; j < bHoMPoints.Count - 1; j++)
            {
                TBD.TasPoint tasPt = tbdPolygon.AddPoint();
                tasPt = Engine.TAS.Convert.ToTas(bHoMPoints[j], tasPt);
            }

            return tbdPolygon;
        }

        /***************************************************/

        public static TBD.zoneSurface ToTas(this IBuildingObject bHoMPanel, zoneSurface tbdZoneSurface)
        {

            //tasZoneSrf.orientation = Query.GetOrientation(bHoMPanel);
            //tasZoneSrf.inclination = Query.GetInclination(bHoMPanel);

            tbdZoneSurface.altitude = (float)BH.Engine.Environment.Query.Altitude(bHoMPanel);
            tbdZoneSurface.altitudeRange = (float)BH.Engine.Environment.Query.AltitudeRange(bHoMPanel);
            tbdZoneSurface.GUID = bHoMPanel.BHoM_Guid.ToString();
            tbdZoneSurface.area = (float)Geometry.Query.IArea((bHoMPanel.ICurve()));

            //tasZoneSrf.type = IToTas(bHoMPanel);
            //tasZoneSrf.type = SurfaceType.tbdLink;
            
            return tbdZoneSurface;
        }

        /***************************************************/

        public static TBD.zone ToTas(this Space bHoMSpace, zone tbdZone)
        {
            tbdZone.name = bHoMSpace.Number + " " + bHoMSpace.Name;
            tbdZone.floorArea = (float)BH.Engine.Environment.Query.FloorArea(bHoMSpace);
            tbdZone.GUID = bHoMSpace.BHoM_Guid.ToString();
            tbdZone.volume = (float)BH.Engine.Environment.Query.Volume(bHoMSpace);

            return tbdZone;
        }

        /***************************************************/

        public static TBD.InternalCondition ToTas(this BHE.Elements.InternalCondition bHoMIC, TBD.InternalCondition tbdIC)
        {
            tbdIC.name = bHoMIC.Name;
            return tbdIC;
        }

        /***************************************************/

        public static TBD.buildingElement ToTas(this BHE.Elements.BuildingElement bHoMBuildingElement, TBD.buildingElement tbdBuildingElement, TBD.Building tbdBuilding)
        {
    
            tbdBuildingElement.name = bHoMBuildingElement.Name;
            //TAS.Adapter
            tbdBuildingElement.BEType = BH.Engine.TAS.Convert.ToTBDBEType(bHoMBuildingElement);
            //tasBuildingElement.colour = bHoMBuildingElement
            tbdBuildingElement.AssignConstruction(BH.Engine.TAS.Convert.ToTBDBEConstruction(bHoMBuildingElement, tbdBuilding));
            return tbdBuildingElement;

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

        public static TBD.SurfaceType ToTas(this BHE.Elements.BuildingElement bHoMSurface)
        {
            //TODO: Fix the Object Model and link the right kind of Elementtypes 

            //Should we implement an enum for surface types in BHoM?? 
            /*
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
            */
            return SurfaceType.tbdNullLink;

        }


        /***************************************************/

        public static TBD.SurfaceType ToTas(this BHE.Elements.Panel bHoMSurface)
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
