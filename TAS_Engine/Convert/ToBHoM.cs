using System;
using System.Collections.Generic;
using System.Linq;
using BHE = BH.oM.Environmental;
using BHS = BH.oM.Structural;
using BHG = BH.oM.Geometry;
using TBD;
using BHEE = BH.Engine.Environment;
using BH.oM.Environmental.Properties;
using BH.oM.Environmental.Elements;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/

        public static BHE.Elements.Building ToBHoM(this TBD.Building tasBuilding)
        {
            BHE.Elements.Building bHoMBuilding = new BHE.Elements.Building
            {
                Latitude = tasBuilding.latitude,
                Longitude = tasBuilding.longitude,
                Elevation = tasBuilding.maxBuildingAltitude,

                //TODO: location, equipment, spaces, storeys, profiles, IC, EquipmentProperties
            };
            return bHoMBuilding;
        }

        /***************************************************/

        public static BHE.Elements.Space ToBHoM(this TBD.zone tasZone)
        {
            List<BHE.Elements.BuildingElement> bHoMBuildingElements = new List<BHE.Elements.BuildingElement>();
            BHE.Elements.Space bHoMSpace = new BHE.Elements.Space();
            bHoMSpace.Name = tasZone.name;

            int buildingElementIndex = 1;
            while (tasZone.GetSurface(buildingElementIndex) != null)
            {
                buildingElement tasBuildingElement = tasZone.GetSurface(buildingElementIndex).buildingElement;
                bHoMSpace.BuildingElements.Add(tasBuildingElement.ToBHoM());
                buildingElementIndex++;
            }

            return bHoMSpace;
        }

        /***************************************************/

        public static BHE.Elements.BuildingElement ToBHoM(this TBD.buildingElement tasBuildingElement)
        {
            if (tasBuildingElement == null)
                return null;

            Construction tasConstruction = tasBuildingElement.GetConstruction();
            BuildingElementProperties bHoMBuildingElementProperties = null;
            
            if (tasConstruction != null)
            {
                bHoMBuildingElementProperties = tasConstruction.ToBHoM();
                bHoMBuildingElementProperties.BuildingElementType = ToBHoM((TBD.BuildingElementType)tasBuildingElement.BEType);// BEType on Construction
            }                          
                           
            BHE.Elements.BuildingElement bhomBuildingElement = new BHE.Elements.BuildingElement
            {
                Name = tasBuildingElement.name,
                BuildingElementProperties = bHoMBuildingElementProperties
            };

         return bhomBuildingElement;

            //TODO: add storey and BuildingElementGeometry
        }

        /***************************************************/
        
        public static BHE.Properties.BuildingElementProperties ToBHoM(this TBD.Construction tasConstruction)
        {
           
            //List<float> u= (tasConstruction.GetUValue() as IEnumerable<float>).ToList();
            BHE.Properties.BuildingElementProperties bhomBuildingElementProperties = new BHE.Properties.BuildingElementProperties()
            {
                Name = tasConstruction.name,
                Thickness = tasConstruction.materialWidth[0],
                LtValue = tasConstruction.lightTransmittance,
                ThermalConductivity = tasConstruction.conductance,
                //TODO: gValue = ?
                //TODO: UValue = ?
            };

            //Assign Construction Layer to the object
            List<BHE.Elements.ConstructionLayer> bHoMConstructionLayer = new List<BHE.Elements.ConstructionLayer>();

            int constructionLayerIndex = 1; //Cannot be 0 in TAS
            while (tasConstruction.materials(constructionLayerIndex) != null)
            {
                bhomBuildingElementProperties.ConstructionLayers.Add(ToBHoM(tasConstruction, tasConstruction.materials(constructionLayerIndex)));
                constructionLayerIndex++;
            }

            return bhomBuildingElementProperties;
        }

        /***************************************************/

        public static BHE.Elements.ConstructionLayer ToBHoM(this TBD.Construction tasConstructionLayer, material tasMaterial)
        {
            BHE.Elements.ConstructionLayer bhomConstructionLayer = new BHE.Elements.ConstructionLayer()
            {       
               Thickness = tasMaterial.width,
               Material = tasMaterial.ToBHoM()
              
            };
            return bhomConstructionLayer;
        }

        /***************************************************/

        public static BHS.Elements.Storey ToBHoM(this TBD.BuildingStorey tasStorey)
        {
            throw new NotImplementedException();
        }
              
        /***************************************************/
             
        public static BHE.Elements.BuildingElementPanel ToBHoM(this TBD.RoomSurface tasRoomSrf)
        {
            BHE.Elements.BuildingElementPanel bHoMPanel = new BHE.Elements.BuildingElementPanel();

            //TBD.RoomSurface currRoomSrf = tasZoneSurface.GetRoomSurface(0); // We need to use all of the room surfaces in the model (Sofia)
            TBD.Perimeter currPerimeter = tasRoomSrf.GetPerimeter();
            TBD.Polygon currPolygon = currPerimeter.GetFace();
                        
            BHG.Polyline edges = ToBHoM(currPolygon);
            BHG.PolyCurve crv_edges = Geometry.Create.PolyCurve(new List<BHG.Polyline> { edges }); //Can I solve this in a better way??

            bHoMPanel.PolyCurve = crv_edges;

            return bHoMPanel;

            //TODO: add surface data. (Not from the TBD namespace)
        }

        /***************************************************/

        public static BHE.Interface.IMaterial ToBHoM(this TBD.material tasMaterial)
        {
           BHE.Elements.MaterialType materialtype = ToBHoM((TBD.MaterialTypes)tasMaterial.type);

           switch (materialtype)
            {
                case MaterialType.Opaque:

                    BHE.Elements.OpaqueMaterial bhomOpaqeMaterial = new BHE.Elements.OpaqueMaterial
                    {
                        Name = tasMaterial.name,
                        Description = tasMaterial.description,
                        MaterialType = materialtype,
                        Thickness = tasMaterial.width,
                        Conductivity = tasMaterial.conductivity,
                        VapourDiffusionFactor = tasMaterial.vapourDiffusionFactor,
                        SolarReflectanceExternal = tasMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tasMaterial.internalSolarReflectance,
                        LightReflectanceExternal = tasMaterial.externalLightReflectance,
                        LightReflectanceInternal = tasMaterial.internalLightReflectance,
                        EmissivityExternal = tasMaterial.externalEmissivity,
                        EmissivityInternal = tasMaterial.internalEmissivity
                    };
                    return bhomOpaqeMaterial;

                case MaterialType.Transparent:
                    BHE.Elements.TransparentMaterial bhomTransparentMaterial = new BHE.Elements.TransparentMaterial
                    {
                        Name = tasMaterial.name,
                        Description = tasMaterial.description,
                        MaterialType = materialtype,
                        Thickness = tasMaterial.width,
                        Conductivity = tasMaterial.conductivity,
                        VapourDiffusionFactor = tasMaterial.vapourDiffusionFactor,
                        SolarTransmittance = tasMaterial.solarTransmittance,
                        SolarReflectanceExternal = tasMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tasMaterial.internalSolarReflectance,
                        LightReflectanceExternal = tasMaterial.externalLightReflectance,
                        LightReflectanceInternal = tasMaterial.internalLightReflectance,
                        EmissivityExternal = tasMaterial.externalEmissivity,
                        EmissivityInternal = tasMaterial.internalEmissivity

                    };
                    return bhomTransparentMaterial;

                case MaterialType.Gas:
                    BHE.Elements.GasMaterial bhomGasMaterial = new BHE.Elements.GasMaterial
                    {
                    Name = tasMaterial.name,
                    Thickness = tasMaterial.width,
                    ConvectionCoefficient = tasMaterial.convectionCoefficient,
                    VapourDiffusionFactor = tasMaterial.vapourDiffusionFactor
                    };

                    return bhomGasMaterial;
            }
            return null;
        }

        /***************************************************/

        public static BHE.Elements.InternalCondition ToBHoM(this TBD.InternalCondition tasInternalCondition)
        {
            BHE.Elements.InternalCondition bHoMInternalCondition = new BHE.Elements.InternalCondition
            {
                //EmitterProperties = tasInternalCondition.GetCoolingEmitter().ToBHoM(),
                //TODO: implement bHoMEmitterProperties and Profiles
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

        public static BHG.Point ToBHoM(this TBD.TasPoint tasPoint)
        {
            BHG.Point bHoMPoint = new BHG.Point()
            {
                X = tasPoint.x,
                Y = tasPoint.y,
                Z = tasPoint.z
            };
            return bHoMPoint;
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this TBD.Polygon tasPolygon)  // TODO : When BH.oM.Geometry.Contour is implemented, Polyline can be replaced with Contour
        {
            //
            //  Not sure how this is working but that's a very strange way of getting points for Tas. Are you sure it is the only way?
            //
            List<BHG.Point> bHoMPointList = new List<BHG.Point>();
            int pointIndex = 0;
            while (true)
            {
                TasPoint tasPt= tasPolygon.GetPoint(pointIndex);
                if (tasPt == null) { break; }
                bHoMPointList.Add(tasPt.ToBHoM());
                pointIndex++;
            }
            bHoMPointList.Add(bHoMPointList[0]);
            return new BHG.Polyline { ControlPoints = bHoMPointList };
        }

        /***************************************************/



        /***************************************************/
        /**** Enums                                     ****/
        /***************************************************/
               
        public static BHE.Elements.MaterialType ToBHoM(this TBD.MaterialTypes tasMaterialType)
        {
            switch (tasMaterialType)
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

        public static BHE.Elements.BuidingElementType ToBHoM(this TBD.BuildingElementType tasBuildingElementType)
        {
            switch (tasBuildingElementType)
            {
                case BuildingElementType.CURTAINWALL:
                case BuildingElementType.EXTERNALWALL:
                case BuildingElementType.INTERNALWALL:
                    return BHE.Elements.BuidingElementType.Wall;
                case BuildingElementType.ROOFELEMENT:
                    return BHE.Elements.BuidingElementType.Roof;
                case BuildingElementType.CEILING:
                case BuildingElementType.UNDERGROUNDCEILING:
                    return BHE.Elements.BuidingElementType.Ceiling;
                case BuildingElementType.EXPOSEDFLOOR:
                case BuildingElementType.INTERNALFLOOR:
                case BuildingElementType.RAISEDFLOOR:
                    return BHE.Elements.BuidingElementType.Floor;
                default:
                    return BHE.Elements.BuidingElementType.Wall;
            }
        }

    }
}
