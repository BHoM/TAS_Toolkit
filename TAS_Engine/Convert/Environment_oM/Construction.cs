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
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Construction from a TAS TBD Construction")]
        [Input("tbdConstruction", "TAS TBD Construction")]
        [Output("BHoM Environmental Construction")]
        public static BHE.Construction ToBHoM(this TBD.Construction tbdConstruction)
        {
            if (tbdConstruction == null) return null;

            BHE.Construction construction = new BHE.Construction();
            construction.Name = tbdConstruction.name;

            //int mIndex = 1;
            //TBD.material tbdMaterial = null;
            //double thickness = 0;
            //while ((tbdMaterial = tbdConstruction.materials(mIndex)) != null)
            //{
            //    construction.Materials.Add(tbdMaterial.ToBHoM());
            //    thickness += (construction.Materials.Last() as BHM.Material).Thickness;
            //    mIndex++;
            //}

            //construction.Thickness = thickness;
            ////construction.BHoM_Guid = new Guid(tbdConstruction.GUID);
            //construction.AdditionalHeatTransfer = tbdConstruction.additionalHeatTransfer;
            //construction.ConstructionType = tbdConstruction.type.ToBHoM();
            //construction.FFactor = tbdConstruction.FFactor;

            //Dictionary<string, object> tasData = new Dictionary<string, object>();
            //tasData.Add("Description", tbdConstruction.description);

            //construction.CustomData = tasData;

            return construction;
        }

        //[Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental ConstructionType from a TAS TBD ConstructionType")]
        //[Input("tbdType", "TAS TBD ConstructionType")]
        //[Output("BHoM Environmental ConstructionType")]
        //public static BHE.ConstructionType ToBHoM(this TBD.ConstructionTypes tbdType)
        //{
        //    switch (tbdType)
        //    {
        //        case TBD.ConstructionTypes.tcdOpaqueConstruction:
        //            return BHE.ConstructionType.Opaque;
        //        case TBD.ConstructionTypes.tcdTransparentConstruction:
        //            return BHE.ConstructionType.Transparent;
        //        default:
        //            return BHE.ConstructionType.Undefined;
        //    }
        //}

        //[Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD ConstructionType from a BHoM Environmental ConstructionType")]
        //[Input("type", "BHoM Environmental ConstructionType")]
        //[Output("TAS TBD ConstructionType")]
        //public static TBD.ConstructionTypes ToTAS(this BHE.ConstructionType type)
        //{
        //    switch (type)
        //    {
        //        case BHE.ConstructionType.Opaque:
        //            return TBD.ConstructionTypes.tcdOpaqueConstruction;
        //        case BHE.ConstructionType.Transparent:
        //            return TBD.ConstructionTypes.tcdTransparentConstruction;
        //        default:
        //            return TBD.ConstructionTypes.tcdOpaqueConstruction;
        //    }
        //}

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Construction from a BHoM Environmental Construction")]
        [Input("construction", "BHoM Environmental Construction")]
        [Output("TAS TBD Construction")]
        public static TBD.Construction ToTAS(this BHE.Construction construction)
        {
            TBD.ConstructionClass tbdConstruction = new TBD.ConstructionClass();
            if (construction == null) return tbdConstruction;

            tbdConstruction.name = construction.Name;
            //tbdConstruction.GUID = construction.BHoM_Guid.ToString();
            //tbdConstruction.additionalHeatTransfer = (float)construction.AdditionalHeatTransfer;
            //tbdConstruction.FFactor = (float)construction.FFactor;
            //tbdConstruction.type = construction.ConstructionType.ToTAS();

            //tbdConstruction.ma
            //foreach (BHM.Material material in construction.Materials)
            //{
            //    TBD.materialClass mat = material.ToTAS();
            //    TBD.material matAdd = tbdConstruction.AddMaterial();

            //    matAdd.name = mat.name;
            //    matAdd.width = mat.width;
            //    matAdd.type = mat.type;
            //    mat.convectionCoefficient = mat.convectionCoefficient;
            //    mat.vapourDiffusionFactor = mat.vapourDiffusionFactor;
            //    matAdd.conductivity = mat.conductivity;
            //    matAdd.specificHeat = mat.specificHeat;
            //    matAdd.density = mat.density;
            //    matAdd.externalSolarReflectance = mat.externalSolarReflectance;
            //    matAdd.internalSolarReflectance = mat.internalSolarReflectance;
            //    matAdd.externalLightReflectance = mat.externalLightReflectance;
            //    matAdd.internalLightReflectance = mat.internalLightReflectance;
            //    matAdd.externalEmissivity = mat.externalEmissivity;
            //    matAdd.internalEmissivity = mat.internalEmissivity;
            //    matAdd.solarTransmittance = mat.solarTransmittance;
            //    matAdd.lightTransmittance = mat.lightTransmittance;
            //}

            //Dictionary<string, object> tasData = construction.CustomData;

            //if (tasData != null)
            //{
            //    tbdConstruction.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
            //}

            return tbdConstruction;
        }
    }
}
