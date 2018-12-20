/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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
            //Check were we are refering this to
            tbdPoint.x = (float)(bHoMPoint.X);
            tbdPoint.y = (float)(bHoMPoint.Y);
            tbdPoint.z = (float)(bHoMPoint.Z);
            return tbdPoint;
        }

        /***************************************************/

        public static TBD.Polygon ToTas(this BHG.ICurve bHoMPolyCurve, Polygon tbdPolygon)
        {
            //Check were we are refering this to
            List<BHG.Point> bHoMPoints = Engine.Geometry.Query.IControlPoints(bHoMPolyCurve);

            for (int j = 0; j < bHoMPoints.Count - 1; j++)
            {
                TBD.TasPoint tasPoint = tbdPolygon.AddPoint();
                tasPoint = Engine.TAS.Convert.ToTas(bHoMPoints[j], tasPoint);
            }

            return tbdPolygon;
        }

        /***************************************************/

        public static TBD.zoneSurface ToTas(this IBuildingObject bHoMPanel, zoneSurface tbdZoneSurface)
        {
            //Check were we are refering this to
            tbdZoneSurface.altitude = (float)BH.Engine.Environment.Query.Altitude(bHoMPanel);
            tbdZoneSurface.altitudeRange = (float)BH.Engine.Environment.Query.AltitudeRange(bHoMPanel);
            tbdZoneSurface.area = (float)Geometry.Query.IArea((bHoMPanel.ICurve()));
            //tbdZoneSurface.buildingElement=
            tbdZoneSurface.GUID = bHoMPanel.BHoM_Guid.ToString();
            //tasZoneSrf.inclination = Query.GetInclination(bHoMPanel);
            //tbdZoneSurface.internalArea
            //tbdZoneSurface.linkSurface
            //tbdZoneSurface.number
            //tasZoneSrf.orientation = Query.GetOrientation(bHoMPanel);
            //tbdZoneSurface.planHydraulicDiameter
            //tbdZoneSurface.reversed
            //tasZoneSrf.type = IToTas(bHoMPanel); 
            //tasZoneSrf.type = SurfaceType.tbdLink;
            //tbdZoneSurface.zone

            return tbdZoneSurface;
        }

        /***************************************************/

        public static TBD.zone ToTas(this Space bHoMSpace, zone tbdZone)
        {
            //Check were we are refering this to
            //tbdZone.colour=
            //tbdZone.daylightFactor=
            //tbdZone.description=
            //tbdZone.exposedPerimeter=
            //tbdZone.external=
            //tbdZone.facadeLength=
            //tbdZone.fixedConvectionCoefficient=
            tbdZone.floorArea = (float)BH.Engine.Environment.Query.FloorArea(bHoMSpace);
            tbdZone.GUID = bHoMSpace.BHoM_Guid.ToString();
            //tbdZone.length=
            //tbdZone.markDelete=
            //tbdZone.maxCoolingLoad=
            //tbdZone.maxHeatingLoad=
            tbdZone.name = bHoMSpace.Number + " " + bHoMSpace.Name;
            //tbdZone.number=
            //tbdZone.output=
            //tbdZone.peakFlowCool=
            //tbdZone.peakFlowHeat=
            //tbdZone.sizeCooling=
            //tbdZone.sizeHeating=
            //tbdZone.variableConvectionCoefficient=
            tbdZone.volume = (float)BH.Engine.Environment.Query.Volume(bHoMSpace);
            //tbdZone.wallFloorAreaRatio=

            return tbdZone;
        }

        /***************************************************/

        public static TBD.InternalCondition ToTas(this BHE.Elements.InternalCondition bHoMIC, TBD.InternalCondition tbdIC)
        {
            //Check were we are refering this to
            //tbdIC.description=
            //tbdIC.includeSolarInMRT =
            tbdIC.name = bHoMIC.Name;
            return tbdIC;
        }

        /***************************************************/

        public static TBD.buildingElement ToTas(this BHE.Elements.BuildingElement bHoMBuildingElement, TBD.buildingElement tbdBuildingElement, TBD.Building tbdBuilding)
        {
            //Check were we are refering this to
            //tbdBuildingElement.AssignConstruction(BH.Engine.TAS.Convert.ToTBDBEConstruction(bHoMBuildingElement, tbdBuilding)); Are we keeping this one?
            tbdBuildingElement.BEType = BH.Engine.TAS.Convert.ToTBDBEType(bHoMBuildingElement);
            //tbdBuildingElement.colour=
            //tbdBuildingElement.description=
            //tbdBuildingElement.ghost=
            //tbdBuildingElement.ground=
            //tbdBuildingElement.GUID=
            //tbdBuildingElement.markDelete=
            //TAS.Adapter
            tbdBuildingElement.name = bHoMBuildingElement.Name;
            //tbdBuildingElement.width=
            
            return tbdBuildingElement;
        }


        /***************************************************/
        /**** Public Methods - Objects                  ****/
        /***************************************************/

        public static TBD.material ToTas(this BHE.Materials.Material mat)
        {
            //TODO: Fix this method to work with all types of materials - switch statement between type and build accordingly
            BHE.Properties.MaterialPropertiesOpaque bHoMOpaqueMaterial = mat.MaterialProperties as BHE.Properties.MaterialPropertiesOpaque;
            TBD.material tasMaterial = new TBD.material

            {
                //Check were we are refering this to
                conductivity = (float)bHoMOpaqueMaterial.Conductivity,
                //convectionCoefficient = (float)bHoMOpaqueMaterial.convectionCoefficient,
                density=(float)bHoMOpaqueMaterial.Density,
                description = bHoMOpaqueMaterial.Description,
                externalEmissivity = (float)bHoMOpaqueMaterial.EmissivityExternal,
                externalLightReflectance = (float)bHoMOpaqueMaterial.LightReflectanceExternal,
                externalSolarReflectance = (float)bHoMOpaqueMaterial.SolarReflectanceExternal,
                internalEmissivity = (float)bHoMOpaqueMaterial.EmissivityInternal,
                //internalLightReflectance = (float)bHoMOpaqueMaterial.LightReflectanceInternal,
                //internalSolarReflectance = (float)bHoMOpaqueMaterial.SolarReflectanceInternal,
                //isBlind=(float)bHoMOpaqueMaterial.IsBlind,
                //lightTransmittance=(float)bHoMOpaqueMaterial.LightTransmittance,
                name = bHoMOpaqueMaterial.Name,
                //solarTransmittance=bHoMOpaqueMaterial.SolarTransmittance,
                //specificHeat=bHoMOpaqueMaterial.SpecificHeat,
                //type=bHoMOpaqueMaterial.Type,
                vapourDiffusionFactor = (float)bHoMOpaqueMaterial.VapourDiffusionFactor,
                //width = (float)bHoMOpaqueMaterial.Thickness,
            };

            return tasMaterial;
        }

        /***************************************************/
        /*
        public static TBD.ConstructionClass ToTas(this BuildingElementProperties bHoMBuildingElementProperties)
        {

            TBD.ConstructionClass tasConstruction = new TBD.ConstructionClass
            {
                //Check were we are refering this to
                additionalHeatTransfer=bHoMBuildingElementProperties.additionalHeatTransfer,
                conductance=bHoMBuildingElementProperties.Conductance,
                description=bHoMBuildingElementProperties.Description,
                externalBlind=bHoMBuildingElementProperties.ExternalBlind,
                externalEmissivity=bHoMBuildingElementProperties.ExternalEmissivity,
                externalSolarAbsorptanceExtSurf=bHoMBuildingElementProperties.ExternalSolarAbsorptanceExtSurf,
                externalSolarAbsorptanceIntSurf = bHoMBuildingElementProperties.ExternalSolarAbsorptanceIntSurf,
                FFactor=bHoMBuildingElementProperties.FFactor,
                GUID=bHoMBuildingElementProperties.GUID,
                internalBlind=bHoMBuildingElementProperties.InternalBlind,
                internalEmissivity = bHoMBuildingElementProperties.InternalEmissivity,
                internalSolarAbsorptanceExtSurf = bHoMBuildingElementProperties.InternalSolarAbsorptanceExtSurf,
                internalSolarAbsorptanceIntSurf = bHoMBuildingElementProperties.InternalSolarAbsorptanceIntSurf,
                lightReflectance=bHoMBuildingElementProperties.LightReflectance,
                lightTransmittance=bHoMBuildingElementProperties.LightTransmittance,
                //materialWidth=bHoMBuildingElementProperties.MaterialWidth,
                name = bHoMBuildingElementProperties.Name,
                solarTransmittance=bHoMBuildingElementProperties.SolarTransmittance,
                timeConstant=bHoMBuildingElementProperties.TimeConstant,
                type=bHoMBuildingElementProperties.Type,
                ;
            }

            return tasConstruction;
    }*/

        /***************************************************/


        /***************************************************/
        /**** Public Methods - Enums                    ****/
        /***************************************************/

        public static TBD.MaterialTypes ToTas(this BHE.Elements.MaterialType bHoMMaterialType)
        {
            switch (bHoMMaterialType)
            {
                //Check were we are refering this to
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
                //Check were we are refering this to
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
             * //Check were we are refering this to
             * 
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
            //Check were we are refering this to
            return SurfaceType.tbdNullLink;
        }

        /***************************************************/

        public static TBD.SurfaceType IToTas(this BHE.Interface.IBuildingElementGeometry bHoMSurface)
        {
            //Check were we are refering this to
            return ToTas(bHoMSurface as dynamic);
        }

        /***************************************************/
    }
}
