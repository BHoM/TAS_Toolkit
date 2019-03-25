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
using BHE = BH.oM.Environment;
using BHEE = BH.oM.Environment.Elements;
using BHM = BH.oM.Environment.Materials;
using BHG = BH.oM.Geometry;
using BHP = BH.oM.Environment.Properties;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Construction from a TAS TBD Construction")]
        [Input("tbdConstruction", "TAS TBD Construction")]
        [Output("BHoM Environmental Construction")]
        public static BHEE.Construction ToBHoM(this TBD.Construction tbdConstruction)
        {
            if (tbdConstruction == null) return null;

            BHEE.Construction construction = new BHEE.Construction();
            construction.Name = tbdConstruction.name;

            int mIndex = 1;
            TBD.material tbdMaterial = null;
            double thickness = 0;
            while ((tbdMaterial = tbdConstruction.materials(mIndex)) != null)
            {
                construction.Materials.Add(tbdMaterial.ToBHoM());
                thickness += (construction.Materials.Last() as BHM.Material).Thickness;
                mIndex++;
            }

            construction.Thickness = thickness;
            //construction.BHoM_Guid = new Guid(tbdConstruction.GUID);
            construction.AdditionalHeatTransfer = tbdConstruction.additionalHeatTransfer;
            construction.ConstructionType = tbdConstruction.type.ToBHoM();
            construction.FFactor = tbdConstruction.FFactor;

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("Description", tbdConstruction.description);

            construction.CustomData = tasData;

            return construction;
        }

        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental ConstructionType from a TAS TBD ConstructionType")]
        [Input("tbdType", "TAS TBD ConstructionType")]
        [Output("BHoM Environmental ConstructionType")]
        public static BHEE.ConstructionType ToBHoM(this TBD.ConstructionTypes tbdType)
        {
            switch (tbdType)
            {
                case TBD.ConstructionTypes.tcdOpaqueConstruction:
                    return BHEE.ConstructionType.Opaque;
                case TBD.ConstructionTypes.tcdTransparentConstruction:
                    return BHEE.ConstructionType.Transparent;
                default:
                    return BHEE.ConstructionType.Undefined;
            }
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD ConstructionType from a BHoM Environmental ConstructionType")]
        [Input("type", "BHoM Environmental ConstructionType")]
        [Output("TAS TBD ConstructionType")]
        public static TBD.ConstructionTypes ToTAS(this BHEE.ConstructionType type)
        {
            switch (type)
            {
                case BHEE.ConstructionType.Opaque:
                    return TBD.ConstructionTypes.tcdOpaqueConstruction;
                case BHEE.ConstructionType.Transparent:
                    return TBD.ConstructionTypes.tcdTransparentConstruction;
                default:
                    return TBD.ConstructionTypes.tcdOpaqueConstruction;
            }
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Construction from a BHoM Environmental Construction")]
        [Input("construction", "BHoM Environmental Construction")]
        [Output("TAS TBD Construction")]
        public static TBD.Construction ToTAS(this BHEE.Construction construction)
        {
            TBD.ConstructionClass tbdConstruction = new TBD.ConstructionClass();
            if (construction == null) return tbdConstruction;

            tbdConstruction.name = construction.Name;
            //tbdConstruction.GUID = construction.BHoM_Guid.ToString();
            tbdConstruction.additionalHeatTransfer = (float)construction.AdditionalHeatTransfer;
            tbdConstruction.FFactor = (float)construction.FFactor;
            tbdConstruction.type = construction.ConstructionType.ToTAS();
            //bool success = true;
            //foreach (BH.oM.Environment.Materials.Material aMaterial in construction.Materials)
            //{
            //    success &= Create(aMaterial);
            //}

            //foreach (BHM.Material material in construction.Materials)
            //{
            //    TBD.material tbdMaterial = tbdConstruction.AddMaterial();

            //    tbdMaterial.name = material.Name;
            //    tbdMaterial.width = (float)material.Thickness;
            //    tbdMaterial.type = (int)material.MaterialType.ToTAS();
            //    switch (material.MaterialType)
            //    {
            //        case BHE.Elements.MaterialType.Gas:
            //            tbdMaterial.convectionCoefficient = (float)((BHP.MaterialPropertiesGas)material.MaterialProperties).ConvectionCoefficient;
            //            tbdMaterial.vapourDiffusionFactor = (float)((BHP.MaterialPropertiesGas)material.MaterialProperties).VapourDiffusionFactor;
            //            tbdMaterial.description = ((BHP.MaterialPropertiesGas)material.MaterialProperties).Description;

            //            break;
            //        case BHE.Elements.MaterialType.Opaque:
            //            tbdMaterial.conductivity = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).Conductivity;
            //            tbdMaterial.specificHeat = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).SpecificHeat;
            //            tbdMaterial.density = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).Density;
            //            tbdMaterial.vapourDiffusionFactor = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).VapourDiffusionFactor;
            //            tbdMaterial.externalSolarReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).SolarReflectanceExternal;
            //            tbdMaterial.internalSolarReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).SolarReflectanceInternal;
            //            tbdMaterial.externalLightReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).LightReflectanceExternal;
            //            tbdMaterial.internalLightReflectance = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).LightReflectanceInternal;
            //            tbdMaterial.externalEmissivity = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).EmissivityExternal;
            //            tbdMaterial.internalEmissivity = (float)((BHP.MaterialPropertiesOpaque)material.MaterialProperties).EmissivityInternal;
            //            tbdMaterial.description = ((BHP.MaterialPropertiesOpaque)material.MaterialProperties).Description;
            //            break;
            //        case BHE.Elements.MaterialType.Transparent:
            //            tbdMaterial.conductivity = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).Conductivity;
            //            tbdMaterial.vapourDiffusionFactor = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).VapourDiffusionFactor;
            //            tbdMaterial.solarTransmittance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).SolarTransmittance;
            //            tbdMaterial.externalSolarReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).SolarReflectanceExternal;
            //            tbdMaterial.internalSolarReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).SolarReflectanceInternal;
            //            tbdMaterial.lightTransmittance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).LightTransmittance;
            //            tbdMaterial.externalLightReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).LightReflectanceExternal;
            //            tbdMaterial.internalLightReflectance = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).LightReflectanceInternal;
            //            tbdMaterial.externalEmissivity = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).EmissivityExternal;
            //            tbdMaterial.internalEmissivity = (float)((BHP.MaterialPropertiesTransparent)material.MaterialProperties).EmissivityInternal;
            //            tbdMaterial.description = ((BHP.MaterialPropertiesTransparent)material.MaterialProperties).Description;
            //            break;
            //    }
            //}

            Dictionary<string, object> tasData = construction.CustomData;

            if (tasData != null)
            {
                tbdConstruction.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
            }

            return tbdConstruction;
        }
    }
}
