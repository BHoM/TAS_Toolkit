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

using BHI = BH.oM.Environment.Interface;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        public static double ConstructionThickness(this TBD.Construction tbdConstruction, int decimals = 3)
        {
            double tbdMaterialThickness = 0;

            if (tbdConstruction != null)
            {
                List<BHI.IMaterial> bHoMMaterial = new List<BHI.IMaterial>();
                int aIndex = 1;
                TBD.material tbdMaterial = null;
                while ((tbdMaterial = tbdConstruction.materials(aIndex)) != null)
                {
                    tbdMaterial = tbdConstruction.materials(aIndex);
                    tbdMaterialThickness += tbdMaterial.width;
                    aIndex++;
                }
            }

            return tbdMaterialThickness;
        }
    }
}