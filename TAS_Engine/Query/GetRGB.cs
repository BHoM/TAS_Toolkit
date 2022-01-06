/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;

using BH.Engine.Environment;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Query
    {
        /***************************************************/

        // From .net/dynamo to a colour set back into Tas
        //public static uint ColorToUIntRGB(System.Drawing.Color Color)
        //{
        //    return (uint)((Color.Blue << 16) | (Color.Green << 8) | (Color.Red << 0));
        //}

        // From Tas to display in .net/dynamo
        public static System.Drawing.Color GetRGB(uint uInt)
        {
            byte b = (byte)(uInt >> 16);
            byte g = (byte)(uInt >> 8);
            byte r = (byte)(uInt >> 0);

            return System.Drawing.Color.FromArgb(r, b, g);
        }


        /***************************************************/
    }
}



