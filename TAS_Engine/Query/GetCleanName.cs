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
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BHE = BH.oM.Environment.Elements;


namespace BH.Engine.TAS
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        static public string GetCleanName(this string Name)
        {
            if (Name == null)
                return null;

            if (Name == string.Empty)
                return string.Empty;

            string aName = Name.Replace(":", "_");
            aName = aName.Replace(" ", string.Empty);

            return aName;
        }

        static public string GetCleanNameFromTAS(this string Name)
        {
            if (Name == null)
                return null;
            string aName = Name;

            if (Name == string.Empty)
                return string.Empty;
            if (Name.EndsWith(" -pane"))
                //aName = Name.Trim(" -pane");
                aName = Name.Remove(Name.Length - 6);

            return aName;
        }
    }
}




