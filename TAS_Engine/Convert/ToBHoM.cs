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

        public static BHE.Elements.BuildingElement ToBHoM(this TBD.buildingElement tasBuildingElement)
        {
            if (tasBuildingElement == null)
                return null;

            Construction tasConstruction = tasBuildingElement.GetConstruction();
            BuildingElementProperties bHoMBuildingElementProperties = null;
            

            if (tasConstruction != null) // The construction needs to exsist. 
            {
                bHoMBuildingElementProperties = tasConstruction.ToBHoM();
                bHoMBuildingElementProperties.BuildingElementType = Enums.Enums.GetBuildingElementType(tasBuildingElement); // BEType on Construction
                
            }                          
                           
            BHE.Elements.BuildingElement BHoMBuildingElement = new BHE.Elements.BuildingElement
            {
                Name = tasBuildingElement.name,
                BuildingElementProperties = bHoMBuildingElementProperties
            };

         return BHoMBuildingElement;
        }

        /***************************************************/
        
        public static BHE.Properties.BuildingElementProperties ToBHoM(this TBD.Construction tasConstruction)
        {
           
            //List<float> u= (tasConstruction.GetUValue() as IEnumerable<float>).ToList();
            BHE.Properties.BuildingElementProperties BHoMBuildingElementProperties = new BHE.Properties.BuildingElementProperties()
            {
                Name = tasConstruction.name,
                Thickness = tasConstruction.materialWidth[0],
                LtValue = tasConstruction.lightTransmittance,
                ThermalConductivity = tasConstruction.conductance,
                //gValue = ?
                //UValue = ?
            };

            return BHoMBuildingElementProperties;
        }

        /***************************************************/
        public static BHE.Elements.ConstructionLayer ToBHoM(this TBD.Construction tasConstructionLayer, material tasMaterial)
        {
            BHE.Elements.ConstructionLayer bHoMConstructionLayer = new BHE.Elements.ConstructionLayer()
            {       
               Thickness = tasMaterial.width,
               Material = tasMaterial.ToBHoM()
              
            };
            return bHoMConstructionLayer;
        }

        /***************************************************/

        public static BHS.Elements.Storey ToBHoM(this TBD.BuildingStorey tasStorey)
        {
            BHS.Elements.Storey BHoMStorey = new BHS.Elements.Storey
            {
                //TODO
            };
            return BHoMStorey;
        }

        /***************************************************/

        public static BHE.Elements.Building ToBHoM(this TBD.Building tasBuilding)
        {
            BHE.Elements.Building bHoMBuilding = new BHE.Elements.Building
            {
                Latitude = tasBuilding.latitude,
                Longitude = tasBuilding.longitude,
                Elevation = tasBuilding.maxBuildingAltitude,
            };
            return bHoMBuilding;
        }

        /***************************************************/
        
        public static BHE.Elements.Space ToBHoM(this TBD.zone tasZone)
        {
            List<BHE.Elements.BuildingElement> bHoMBuildingElements = new List<BHE.Elements.BuildingElement>();
            BHE.Elements.Space bHoMSpace = new BHE.Elements.Space();
            bHoMSpace.Name = tasZone.name;
           
            int buildingElementIndex = 0;
            while (tasZone.GetSurface(buildingElementIndex) != null)
            {
                buildingElement tasBuildingElement = tasZone.GetSurface(buildingElementIndex).buildingElement;
                bHoMSpace.BuildingElements.Add(tasBuildingElement.ToBHoM());
                buildingElementIndex++;
            }    
                        
            return bHoMSpace;
        }

        /***************************************************/
               
        public static BHE.Elements.BuildingElementPanel ToBHoM(this TBD.zoneSurface zonesurface)
        {
            BHE.Elements.BuildingElementPanel bHoMPanel = new BHE.Elements.BuildingElementPanel();

            TBD.RoomSurface currRoomSrf = zonesurface.GetRoomSurface(0);
            TBD.Perimeter currPerimeter = currRoomSrf.GetPerimeter();
            TBD.Polygon currPolygon = currPerimeter.GetFace();
                        
            BHG.Polyline edges = ToBHoM(currPolygon);
            BHG.PolyCurve crv_edges = Geometry.Create.PolyCurve(new List<BHG.Polyline> { edges }); //Can I solve this in a better way??

            bHoMPanel.PolyCurve = crv_edges;

            return bHoMPanel;
        }

        /***************************************************/

        public static BHE.Interface.IMaterial ToBHoM(this TBD.material tasMaterial)
        {
           BHE.Elements.MaterialType materialtype = Enums.Enums.GetMaterialType(tasMaterial);

           switch (materialtype)
            {
                case MaterialType.Opaque:

                    BHE.Elements.OpaqueMaterial BHoMOpaqeMaterial = new BHE.Elements.OpaqueMaterial
                    {
                        Name = tasMaterial.name,
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
                    return BHoMOpaqeMaterial;

                case MaterialType.Transparent:
                    BHE.Elements.TransparentMaterial BHoMTransparentMaterial = new BHE.Elements.TransparentMaterial
                    {
                        Name = tasMaterial.name,
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
                    return BHoMTransparentMaterial;

                case MaterialType.Gas:
                    BHE.Elements.GasMaterial BHoMGasMaterial = new BHE.Elements.GasMaterial
                    {
                    Name = tasMaterial.name,
                    Thickness = tasMaterial.width,
                    ConvectionCoefficient = tasMaterial.convectionCoefficient,
                    VapourDiffusionFactor = tasMaterial.vapourDiffusionFactor
                    };

                    return BHoMGasMaterial;
            }
            return null;
        }

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
    }
}
