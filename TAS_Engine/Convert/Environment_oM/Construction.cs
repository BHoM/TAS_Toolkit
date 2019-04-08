﻿/*
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
using BHM = BH.oM.Environment.MaterialFragments;
using BHG = BH.oM.Geometry;
using BHP = BH.oM.Environment.Fragments;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

using BHPC = BH.oM.Physical.Constructions;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Construction from a TAS TBD Construction")]
        [Input("tbdConstruction", "TAS TBD Construction")]
        [Output("BHoM Environmental Construction")]
        public static BHPC.Construction ToBHoM(this TBD.Construction tbdConstruction)
        {
            if (tbdConstruction == null) return null;

            BHPC.Construction construction = new BHPC.Construction();
            construction.Name = tbdConstruction.name;

            int mIndex = 1;
            TBD.material tbdMaterial = null;
            double thickness = 0;
            while ((tbdMaterial = tbdConstruction.materials(mIndex)) != null)
            {
                construction.Layers.Add(tbdMaterial.ToBHoM(tbdConstruction));
                (construction.Layers.Last()).Thickness = (tbdConstruction.materialWidth[mIndex] == 0 ? (construction.Layers.Last()).Thickness : tbdConstruction.materialWidth[mIndex]); // temp solution set material thickness as value from construction.material.thickness
                thickness += construction.Layers.Last().Thickness;
                mIndex++;
            }

            //construction.FFactor = tbdConstruction.FFactor;

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("Description", tbdConstruction.description);

            construction.CustomData = tasData;

            return construction;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Construction from a BHoM Environmental Construction")]
        [Input("construction", "BHoM Environmental Construction")]
        [Output("TAS TBD Construction")]
        public static TBD.Construction ToTAS(this BHPC.IConstruction construction, TBD.Construction tbdConstruction)
        {
            return ToTAS(construction as dynamic, tbdConstruction);
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Construction from a BHoM Environmental Construction")]
        [Input("construction", "BHoM Environmental Construction")]
        [Output("TAS TBD Construction")]
        public static TBD.Construction ToTAS(this BHPC.Construction construction, TBD.Construction tbdConstruction)
        {
            if (construction == null) return tbdConstruction;



            tbdConstruction.name = construction.Name;
            //tbdConstruction.additionalHeatTransfer = (float)construction.AdditionalHeatTransfer();
            //tbdConstruction.FFactor = (float)construction.FFactor; //ToDo: Fix these from the fragment after fragment implementation

            if (construction.Layers.Count > 0 && construction.Layers[0].Material != null)
            {
                if (construction.Layers[0].Material.IsTransparent())
                    tbdConstruction.type = TBD.ConstructionTypes.tcdTransparentConstruction;
                else
                    tbdConstruction.type = TBD.ConstructionTypes.tcdOpaqueConstruction;
            }

            //tbdConstruction.GUID = construction.BHoM_Guid.ToString();

            foreach(BH.oM.Physical.Constructions.Layer layer in construction.Layers)
                layer.ToTAS(tbdConstruction.AddMaterial());

            ////copy construction thickness to material thickness
            //if (tbdConstruction.type == TBD.ConstructionTypes.tcdOpaqueConstruction)
            //{
            //    int mIndex = 1;
            //    TBD.material m = null;
            //    while ((m = tbdConstruction.materials(mIndex)) != null)
            //    {
            //        tbdConstruction.materials = tbdConstruction.materialWidth[mIndex];
            //        //tbdConstruction.materialWidth[mIndex] = (float)((construction.Materials.Where(x => x.Name == m.name).FirstOrDefault() as BHM.Material).Thickness);
            //        mIndex++;
            //    }
            //}

            //if (tbdConstruction.type == TBD.ConstructionTypes.tcdOpaqueConstruction)
            //{
            int mIndex = 1;
            TBD.material m = null;
            while ((m = tbdConstruction.materials(mIndex)) != null)
            {
                tbdConstruction.materialWidth[mIndex] = (float)(construction.Layers.Where(x => x.Material.Name == m.name).FirstOrDefault().Thickness);
                mIndex++;
            }

            Dictionary<string, object> tasData = construction.CustomData;

            if (tasData != null)
            {
                tbdConstruction.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
            }

            return tbdConstruction;
        }
    }
}
