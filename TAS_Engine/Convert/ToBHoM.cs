using System;
using System.Collections.Generic;
using System.Linq;
using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using TBD;
using TAS3D;
using BHEE = BH.Engine.Environment;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Elements;
using System.Collections;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/

        public static BHE.Elements.Building ToBHoM(this TBD.Building tbdBuilding)
        {

            // by MD 2018-05-21 get BuildingElementsProperties  - in BHoM Building element it contrail geomety and building element property
            // in TAS BuildingElement is just an object with BuildingElement propoerties so please avoid confusion
            List<BHE.Properties.BuildingElementProperties> buildingElementPropertiesList = new List<BHE.Properties.BuildingElementProperties>();

            int buildingElementIndex = 0;
            TBD.buildingElement aBuildingElement = null;
            while ((aBuildingElement = tbdBuilding.GetBuildingElement(buildingElementIndex)) != null)
            {
                buildingElementPropertiesList.Add(ToBHoM(aBuildingElement));
                buildingElementIndex++;
            }

            // get Spaces from TAS TBD in TAS TBD Spaces index start from 0 
            List<BHE.Elements.Space> spaceList = new List<BHE.Elements.Space>();

            int spaceIndex = 0;
            TBD.zone aSpace = null;
            while ((aSpace = tbdBuilding.GetZone(spaceIndex)) != null)
            {
                spaceList.Add(ToBHoM(aSpace));
                spaceIndex++;
            }

            // here we outputing Building data 
            BHE.Elements.Building bHoMBuilding = new BHE.Elements.Building
            {
                Latitude = tbdBuilding.latitude,
                Longitude = tbdBuilding.longitude,
                Elevation = tbdBuilding.maxBuildingAltitude,
                //BuildingElementProperties = buildingElementPropertiesList,
                //Spaces = spaceList,


                //TODO: location, equipment, spaces, storeys, profiles, IC, EquipmentProperties
            };
          return bHoMBuilding;
        }

        /***************************************************/

        public static BHE.Elements.Space ToBHoM(this TBD.zone tbdZone, out double minElevation)
        {
            BHE.Elements.Space bHoMSpace = new BHE.Elements.Space();

            //Space Data
            bHoMSpace.Name = tbdZone.name;
            /*int internalConditionIndex = 0;
            while (tasZone.GetIC(internalConditionIndex) != null)
                {
                    bHoMSpace.InternalConditions.Add(ToBHoM(tasZone.GetIC(0)));
                    internalConditionIndex++;
                }*/

            //Geometry
            int tbdZoneSurfaceIndex = 0;
            minElevation = double.MaxValue;
            zoneSurface tbdZoneSurface = null;
            while ((tbdZoneSurface = tbdZone.GetSurface(tbdZoneSurfaceIndex)) != null)
            {
                int tbdRoomSurfaceIndex = 0;
                RoomSurface tbdRoomSurface = null;
                while ((tbdRoomSurface = tbdZoneSurface.GetRoomSurface(tbdRoomSurfaceIndex)) != null)
                {
                    if (tbdRoomSurface.GetPerimeter() != null)
                    {
                        BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties = ToBHoM(tbdZoneSurface.buildingElement);
                        BHE.Elements.BuildingElement bHoMBuildingElement = new BuildingElement()
                        {
                            Name = bHoMBuildingElementProperties.Name,
                            //BuildingElementGeometry = tasRoomSrf.ToBHoM(),
                            BuildingElementProperties = bHoMBuildingElementProperties
                        };

                        minElevation = Math.Min(minElevation, BH.Engine.TAS.Query.MinElevation(tbdRoomSurface.GetPerimeter()));
                        //bHoMSpace.BuildingElements.Add(bHoMBuildingElement);
                    }
                    tbdRoomSurfaceIndex++;
                }
                tbdZoneSurfaceIndex++;
            }

            //Space Custom Data
            System.Drawing.Color spaceRGB = Query.GetRGB(tbdZone.colour);
            bHoMSpace.CustomData.Add("Colour", spaceRGB);

            return bHoMSpace;
        }

        public static BHE.Elements.Space ToBHoM(this TBD.zone tasZone)
        {
            double aMinElevation;
            return ToBHoM(tasZone, out aMinElevation);
        }

        /***************************************************/

        public static BuildingElementProperties ToBHoM(this TBD.buildingElement tbdBuildingElement)
        {
        //  by MD 2018-05-21 IN TAS Building Element is type with property and does not have geometry. 
        // IN BHoM Building element  is instance including geometry and property
        // BuildingProperty is Type with all data for this type
            if (tbdBuildingElement == null)
                return null;

            BuildingElementProperties bHoMBuildingElementProperties = null;

            BHE.Elements.BuildingElementType aBuildingElementType = ToBHoM((TBD.BuildingElementType)tbdBuildingElement.BEType);
            string aName = tbdBuildingElement.name;

            Construction tbdConstruction = tbdBuildingElement.GetConstruction();
            if (tbdConstruction != null)
                bHoMBuildingElementProperties = tbdConstruction.ToBHoM(aName, aBuildingElementType);

            if (bHoMBuildingElementProperties == null)
                bHoMBuildingElementProperties = new BuildingElementProperties();

            bHoMBuildingElementProperties.BuildingElementType = aBuildingElementType;
            bHoMBuildingElementProperties.Name = aName;
            //BuildingElementProperties do not handle Thickness.
            //bHoMBuildingElementProperties.Thickness = tasBuildingElement.width;

            //BuildingElement Custom Data
            System.Drawing.Color buildingElementRGB = Query.GetRGB(tbdBuildingElement.colour);
            bHoMBuildingElementProperties.CustomData.Add("Colour", buildingElementRGB);

            return bHoMBuildingElementProperties;

        }

        /***************************************************/




        /***************************************************/

        public static BuildingElementProperties ToBHoM(this TBD.Construction tbdConstruction, string name, BHE.Elements.BuildingElementType buildingElementType) //double thickness = 0, bool Internal = true, BHE.Elements.BuildingElementType buildingElementType = BHE.Elements.BuildingElementType.Wall)
        {
            //  by MD 2018-05-21 - there 6 values in TBDTas for Uvalue, we have BuildingElement BE that have construction and then material layers
            // form BE we can get geometrical thickness that is used for Volume calculations, in tas there are 6 Uvalues:
            //0.Uexternalhorizontal 1.Uexternalupwards  2.Uexternaldownwards
            //3.Uinternalhorizontal 4.Uinternaupwards  5.Uinternadownwards
            //Here we use aUvalue as this is special case where we want to see caputal U to be clear
            List<float> aUValues = (tbdConstruction.GetUValue() as List<float>);

            double aUvalue = 0;
            double agValue = 0;
            double aLtValue = 0;

            // here we checking if Building Element is transparent to get correct Uvalue and properties, there is different source for Uvalue
            if (tbdConstruction.type == ConstructionTypes.tcdTransparentConstruction)
            {
                //tas exposes tranparent building element all value as list  
                //1. LtValuegValue,  7. Uvalue,  6. gValue
                List<float> aGlazingValueList = (tbdConstruction.GetGlazingValues() as List<float>);
                agValue = aGlazingValueList[6];
                aLtValue = aGlazingValueList[1];
                aUvalue = aGlazingValueList[7];
            }
            else
            {   //  by MD 2018-05-21 here we selecting Opaque surfaces
                // currently we need to recognized if is innternal or not - BE by name in this case SAM model are following specific naming,
                // _EXT or _INT in code below we filter Internal element to be able to get correct Uvalue
                // in TAS all six value are presented becuase are not used for calculation just for display
                // here we try to 
                bool Internal = name.ToUpper().Contains("_INT");

                switch (buildingElementType)
                {
                    case BHE.Elements.BuildingElementType.Ceiling:
                        aUvalue = aUValues[4];
                        break;
                    case BHE.Elements.BuildingElementType.Door:
                    case BHE.Elements.BuildingElementType.Wall:
                    case BHE.Elements.BuildingElementType.Window:
                        if (Internal)
                            aUvalue = aUValues[3];
                        else
                            aUvalue = aUValues[0];
                        break;
                    case BHE.Elements.BuildingElementType.Floor:
                        if (Internal == true)
                            aUvalue = aUValues[5];
                        else
                            aUvalue = aUValues[2];
                        break;
                    case BHE.Elements.BuildingElementType.Roof:
                        aUvalue = aUValues[1];
                        break;
                    case BHE.Elements.BuildingElementType.Undefined:
                        break;
                }
            }

            // we pulling BEProperties
            BuildingElementProperties bhomBuildingElementProperties = new BHE.Properties.BuildingElementProperties()
            {
                //UValue = aUvalue,
                BuildingElementType = buildingElementType,
           };
            GlazingMaterialProperties bhomGlazingMaterialProperties = new BHE.Properties.GlazingMaterialProperties()
            {
                LtValue = aLtValue,
                gValue = agValue,
                ThermalConductivity = tbdConstruction.conductance,
            };

            //Assign Construction Layer to the object
            List<BHE.Elements.ConstructionLayer> bHoMConstructionLayer = new List<BHE.Elements.ConstructionLayer>();

            int constructionLayerIndex = 1; //Cannot be 0 in TAS
            material tasMat = null;
            while ((tasMat = tbdConstruction.materials(constructionLayerIndex)) != null)
            {
                bhomBuildingElementProperties.ConstructionLayers.Add(ToBHoM(tbdConstruction, tasMat));
                constructionLayerIndex++;
            }

            double aThicknessAnalytical = 0;
            int aIndex = 0;
            material aMaterial = null;
            while ((aMaterial = tbdConstruction.materials(aIndex)) != null)
            {
                aThicknessAnalytical += tbdConstruction.materialWidth[aIndex];
                aIndex++;
            }

            bhomBuildingElementProperties.CustomData.Add("Thickness Analytical", aThicknessAnalytical);

            return bhomBuildingElementProperties;
        }

        /***************************************************/

        public static BHE.Elements.ConstructionLayer ToBHoM(this TBD.Construction tbdConstructionLayer, material tbdMaterial)
        {
            BHE.Elements.ConstructionLayer bhomConstructionLayer = new BHE.Elements.ConstructionLayer()
            {
                Thickness = tbdMaterial.width,
                Material = tbdMaterial.ToBHoM(),
                Name = tbdConstructionLayer.name,
                BHoM_Guid = new Guid(tbdConstructionLayer.GUID),
                UValues = tbdConstructionLayer.GetUValue() as List<double>,
                ConstructionType = tbdConstructionLayer.type.ToBHoM(),
                AdditionalHeatTransfer = tbdConstructionLayer.additionalHeatTransfer,
                FFactor = tbdConstructionLayer.FFactor,
            };
            bhomConstructionLayer.CustomData.Add("TAS_Description", tbdConstructionLayer.description);
            return bhomConstructionLayer;
        }

        /***************************************************/

        public static BHE.Elements.ConstructionType ToBHoM(this TBD.ConstructionTypes type)
        {
            switch(type)
            {
                case ConstructionTypes.tcdOpaqueConstruction:
                    return ConstructionType.Opaque;
                case ConstructionTypes.tcdTransparentConstruction:
                    return ConstructionType.Transparent;
                default:
                    return ConstructionType.Undefined;
            }
        }

        /***************************************************/

        public static BHA.Elements.Level ToBHoM(this TAS3D.Floor t3dFloor)
        {
            return new BHA.Elements.Level()
            {
                Elevation = t3dFloor.level,
                Name = t3dFloor.name,                 
             };
        }

    /***************************************************/

    public static BHE.Elements.Panel ToBHoM(this TBD.RoomSurface tbdRoomSurface)

        {
            BHE.Elements.Panel bHoMPanel = new BHE.Elements.Panel();

            TBD.Perimeter tbdPerimeter = tbdRoomSurface.GetPerimeter();   
            TBD.Polygon tbdPolygon = tbdPerimeter.GetFace();

            BHG.ICurve curve = ToBHoM(tbdPolygon);
            BHG.PolyCurve polyCurve = Geometry.Create.PolyCurve(new List<BHG.ICurve> { curve });

            bHoMPanel.PanelCurve = polyCurve;
            //bHoMPanel.ElementType = ((TBD.BuildingElementType)tasRoomSrf.zoneSurface.buildingElement.BEType).ToString();

            return bHoMPanel;

        }

        /***************************************************/

        public static BHE.Interface.IMaterial ToBHoM(this TBD.material tbdMaterial)
        {
           BHE.Elements.MaterialType materialtype = ToBHoM((TBD.MaterialTypes)tbdMaterial.type);

           switch (materialtype)
            {
                case MaterialType.Opaque:

                    BHE.Materials.OpaqueMaterial bhomOpaqeMaterial = new BHE.Materials.OpaqueMaterial
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        Conductivity = tbdMaterial.conductivity,
                        SpecificHeat = tbdMaterial.specificHeat,
                        Density = tbdMaterial.density,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                        SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                        LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                        LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                        EmissivityExternal = tbdMaterial.externalEmissivity,
                        EmissivityInternal = tbdMaterial.internalEmissivity,
                    };
                    bhomOpaqeMaterial.MaterialProperties.Thickness = tbdMaterial.width;
                    return bhomOpaqeMaterial;

                case MaterialType.Transparent:
                    BHE.Materials.TransparentMaterial bhomTransparentMaterial = new BHE.Materials.TransparentMaterial
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        Conductivity = tbdMaterial.conductivity,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                        SolarTransmittance = tbdMaterial.solarTransmittance,
                        SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                        LightTransmittance = tbdMaterial.lightTransmittance,
                        LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                        LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                        EmissivityExternal = tbdMaterial.externalEmissivity,
                        EmissivityInternal = tbdMaterial.internalEmissivity
                    };
                    bhomTransparentMaterial.MaterialProperties.Thickness = tbdMaterial.width;
                    if (bhomTransparentMaterial.MaterialProperties is GlazingMaterialProperties)
                        (bhomTransparentMaterial.MaterialProperties as GlazingMaterialProperties).IsBlind = (tbdMaterial.isBlind == 1);
                    return bhomTransparentMaterial;

                case MaterialType.Gas:
                    BHE.Materials.GasMaterial bhomGasMaterial = new BHE.Materials.GasMaterial
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        ConvectionCoefficient = tbdMaterial.convectionCoefficient,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor
                    };
                    bhomGasMaterial.MaterialProperties.Thickness = tbdMaterial.width;
                    if (bhomGasMaterial.MaterialProperties is GlazingMaterialProperties)
                        (bhomGasMaterial.MaterialProperties as GlazingMaterialProperties).IsBlind = (tbdMaterial.isBlind == 1);

                    return bhomGasMaterial;
            }
            return null;
        }

        /***************************************************/

        public static BHE.Elements.InternalCondition ToBHoM(this TBD.InternalCondition tbdInternalCondition)
        {
            if (tbdInternalCondition == null)
                return null;
    
            BHE.Elements.InternalCondition bHoMInternalCondition = new BHE.Elements.InternalCondition
            {
                Name = tbdInternalCondition.name
            };

            return bHoMInternalCondition;
        }

        /***************************************************/

        public static BHE.Elements.Emitter ToBHoM(this TBD.Emitter tasEmitterProperties)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHE.Elements.Profile ToBHoM(this TBD.profile tasProfile) //Has no properties in BHoM yet...
        {
            throw new NotImplementedException();
        }

        /***************************************************/




        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this TBD.TasPoint tbdPoint)
        {
            BHG.Point bHoMPoint = new BHG.Point()
            {
                X = tbdPoint.x,
                Y = tbdPoint.y,
                Z = tbdPoint.z
            };
            return bHoMPoint;
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this TBD.Polygon tbdPolygon)  // TODO : When BH.oM.Geometry.Contour is implemented, Polyline can be replaced with Contour
        {
            //
            //  Not sure how this is working but that's a very strange way of getting points for Tas. Are you sure it is the only way?
            //
            List<BHG.Point> bHoMPointList = new List<BHG.Point>();
            int pointIndex = 0;
            while (true)
            {
                TasPoint tasPoint= tbdPolygon.GetPoint(pointIndex);
                if (tasPoint == null) { break; }
                bHoMPointList.Add(tasPoint.ToBHoM());
                pointIndex++;
            }
            bHoMPointList.Add(bHoMPointList[0]);
            return new BHG.Polyline { ControlPoints = bHoMPointList };
        }

        /***************************************************/



        /***************************************************/
        /**** Enums                                     ****/
        /***************************************************/
               
        public static BHE.Elements.MaterialType ToBHoM(this TBD.MaterialTypes tbdMaterialType)
        {
            switch (tbdMaterialType)
            {
                case MaterialTypes.tcdOpaqueLayer:
                case MaterialTypes.tcdOpaqueMaterial:
                    return BHE.Elements.MaterialType.Opaque;
                case MaterialTypes.tcdTransparentLayer:
                    return BHE.Elements.MaterialType.Transparent;
                case MaterialTypes.tcdGasLayer:
                    return BHE.Elements.MaterialType.Gas;
                default:
                    return BHE.Elements.MaterialType.Opaque;
            }
        }

        /***************************************************/

        public static BHE.Elements.BuildingElementType ToBHoM(this TBD.BuildingElementType tbdBuildingElementType)
        {
            switch (tbdBuildingElementType)
            {
                case TBD.BuildingElementType.CURTAINWALL:
                case TBD.BuildingElementType.EXTERNALWALL:
                case TBD.BuildingElementType.INTERNALWALL:
                    return BHE.Elements.BuildingElementType.Wall;
                case TBD.BuildingElementType.ROOFELEMENT:
                    return BHE.Elements.BuildingElementType.Roof;
                case TBD.BuildingElementType.CEILING:
                case TBD.BuildingElementType.UNDERGROUNDCEILING:
                    return BHE.Elements.BuildingElementType.Ceiling;
                case TBD.BuildingElementType.EXPOSEDFLOOR:
                case TBD.BuildingElementType.INTERNALFLOOR:
                case TBD.BuildingElementType.RAISEDFLOOR:
                    return BHE.Elements.BuildingElementType.Floor;
                default:
                    return BHE.Elements.BuildingElementType.Wall;
            }
        }

    }
}
