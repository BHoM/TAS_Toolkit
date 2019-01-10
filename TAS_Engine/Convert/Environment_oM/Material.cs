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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHM = BH.oM.Environment.Materials;
using BHP = BH.oM.Environment.Properties;
using BHI = BH.oM.Environment.Interface;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Material from a TAS TBD Material")]
        [Input("tbdMaterial", "TAS TBD Material")]
        [Output("BHoM Environmental Material")]
        public static BHM.Material ToBHoM(this TBD.material tbdMaterial)
        {
            if (tbdMaterial == null) return null;

            BHM.Material material = new BHM.Material();
            material.Name = tbdMaterial.name;
            material.Thickness = tbdMaterial.width;
            material.MaterialType = ((TBD.MaterialTypes)tbdMaterial.type).ToBHoM();
            material.MaterialProperties = tbdMaterial.ToBHoMProperties();

            return material;
        }

        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental MaterialType from a TAS TBD MaterialType")]
        [Input("tbdType", "TAS TBD MaterialType")]
        [Output("BHoM Environmental MaterialType")]
        public static BHE.MaterialType ToBHoM(this TBD.MaterialTypes tbdType)
        {
            switch (tbdType)
            {
                case TBD.MaterialTypes.tcdOpaqueLayer:
                case TBD.MaterialTypes.tcdOpaqueMaterial:
                    return BHE.MaterialType.Opaque;
                case TBD.MaterialTypes.tcdTransparentLayer:
                    return BHE.MaterialType.Transparent;
                case TBD.MaterialTypes.tcdGasLayer:
                    return BHE.MaterialType.Gas;
                default:
                    return BHE.MaterialType.Opaque;
            }
        }
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental MaterialProperties from a TAS TBD Material")]
        [Input("tbdMaterial", "TAS TBD Material")]
        [Output("BHoM Environmental MaterialProperties")]
        public static BHI.IMaterialProperties ToBHoMProperties(this TBD.material tbdMaterial)
        {
            if (tbdMaterial == null) return null;

            BHE.MaterialType matType = ((TBD.MaterialTypes)tbdMaterial.type).ToBHoM();

            switch(matType)
            {
                case BHE.MaterialType.Gas:
                    return new BHP.MaterialPropertiesGas
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        ConvectionCoefficient = tbdMaterial.convectionCoefficient,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                    };
                case BHE.MaterialType.Opaque:
                    return new BHP.MaterialPropertiesOpaque
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
                case BHE.MaterialType.Transparent:
                    return new BHP.MaterialPropertiesTransparent
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
                        EmissivityInternal = tbdMaterial.internalEmissivity,
                    };
                default:
                    return new BHP.MaterialPropertiesTransparent();
            }
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD MaterialType from a BHoM Environmental MaterialType")]
        [Input("type", "BHoM Environmental MaterialType")]
        [Output("TAS TBD MaterialType")]
        public static TBD.MaterialTypes ToTAS(this BHE.MaterialType type)
        {
            switch(type)
            {
                case BHE.MaterialType.Gas:
                    return TBD.MaterialTypes.tcdGasLayer;
                case BHE.MaterialType.Opaque:
                    return TBD.MaterialTypes.tcdOpaqueMaterial;
                case BHE.MaterialType.Transparent:
                    return TBD.MaterialTypes.tcdTransparentLayer;
                default:
                    return TBD.MaterialTypes.tcdOpaqueMaterial;
            }
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Material from a BHoM Environmental Material")]
        [Input("material", "BHoM Environmental Material")]
        [Output("TAS TBD Material")]
        public static TBD.materialClass ToTAS(this BHM.Material material)
        {
            TBD.materialClass tbdMaterial = new TBD.materialClass();
            if (material == null) return tbdMaterial;

            tbdMaterial.name = material.Name;
            tbdMaterial.width = (float)material.Thickness;
            tbdMaterial.type = (int)material.MaterialType.ToTAS();

            switch (material.MaterialType)
            {
                case BHE.MaterialType.Gas:
                    tbdMaterial.convectionCoefficient = (float)((BHP.MaterialPropertiesGas)material.MaterialProperties).ConvectionCoefficient;
                    tbdMaterial.vapourDiffusionFactor = (float)((BHP.MaterialPropertiesGas)material.MaterialProperties).VapourDiffusionFactor;
                    break;
                case BHE.MaterialType.Opaque:
                    tbdMaterial.conductivity = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).Conductivity;
                    tbdMaterial.specificHeat = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).SpecificHeat;
                    tbdMaterial.density = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).Density;
                    tbdMaterial.vapourDiffusionFactor = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).VapourDiffusionFactor;
                    tbdMaterial.externalSolarReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).SolarReflectanceExternal;
                    tbdMaterial.internalSolarReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).SolarReflectanceInternal;
                    tbdMaterial.externalLightReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).LightReflectanceExternal;
                    tbdMaterial.internalLightReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).LightReflectanceInternal;
                    tbdMaterial.externalEmissivity = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).EmissivityExternal;
                    tbdMaterial.internalEmissivity = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).EmissivityInternal;
                    break;
                case BHE.MaterialType.Transparent:
                    tbdMaterial.conductivity = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).Conductivity;
                    tbdMaterial.vapourDiffusionFactor = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).VapourDiffusionFactor;
                    tbdMaterial.solarTransmittance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).SolarTransmittance;
                    tbdMaterial.externalSolarReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).SolarReflectanceExternal;
                    tbdMaterial.internalSolarReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).SolarReflectanceInternal;
                    tbdMaterial.lightTransmittance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).LightTransmittance;
                    tbdMaterial.externalLightReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).LightReflectanceExternal;
                    tbdMaterial.internalLightReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).LightReflectanceInternal;
                    tbdMaterial.externalEmissivity = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).EmissivityExternal;
                    tbdMaterial.internalEmissivity = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).EmissivityInternal;
                    break;
            }

            return tbdMaterial;
        }
    }
}
