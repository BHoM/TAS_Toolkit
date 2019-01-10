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
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Geomtry Point TAS TBD Point")]
        [Input("tbdPoint", "TAS TBD Point")]
        [Output("BHoM Geometry Point")]
        public static BHG.Point ToBHoM(this TBD.TasPoint tbdPoint)
        {
            return new BHG.Point
            {
                X = tbdPoint.x,
                Y = tbdPoint.y,
                Z = tbdPoint.z,
            };
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Point from a BHoM Geometry Point")]
        [Input("point", "BHoM Geomtry Point")]
        [Output("TAS TBD Point")]
        public static TBD.TasPointClass ToTAS(this BHG.Point point)
        {
            return new TBD.TasPointClass
            {
                x = (float)point.X,
                y = (float)point.Y,
                z = (float)point.Z,
            };
        }
    }
}
