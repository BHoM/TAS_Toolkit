/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BHP = BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.SizingMethod from TAS TBD SizeMethod")]
        [Input("tbdSiziMethod", "TAS TBD SizingType")]
        [Output("BHoM Environmental SizingType Type enum")]
        public static BHP.SizingMethod ToBHoM(this TBD.SizingType sizingType)
        {
            if (sizingType.Equals(0))
                return BHP.SizingMethod.NoSizing;
            if (sizingType.Equals(1))
                return BHP.SizingMethod.DesignSizingOnly;
            if (sizingType.Equals(2))
                return BHP.SizingMethod.Sizing;
            if (sizingType.Equals(3))
                return BHP.SizingMethod.Mixed;
            if (sizingType.Equals(4))
                return BHP.SizingMethod.LoadPerArea;

            return BHP.SizingMethod.Undefined;
        }

        public static TBD.SizingType ToTAS(this BHP.SizingMethod sizingMethod)
        {
            TBD.SizingType tbdSizingType = new TBD.SizingType();

            switch(sizingMethod)
            {
                case BHP.SizingMethod.NoSizing:
                    tbdSizingType = TBD.SizingType.tbdNoSizing;
                    break;
                case BHP.SizingMethod.DesignSizingOnly:
                    tbdSizingType = TBD.SizingType.tbdDesignSizingOnly;
                    break;
                case BHP.SizingMethod.Sizing:
                    tbdSizingType = TBD.SizingType.tbdSizing;
                    break;
                case BHP.SizingMethod.Mixed:
                    tbdSizingType = TBD.SizingType.tbdMixed;
                    break;
                case BHP.SizingMethod.LoadPerArea:
                    tbdSizingType = TBD.SizingType.tbdLoadPerArea;
                    break;
                default:
                    tbdSizingType = TBD.SizingType.tbdSizing;
                    break;
            }

            return tbdSizingType;
        }
    }
}

