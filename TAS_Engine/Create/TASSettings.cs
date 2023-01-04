/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.TAS;
using BH.oM.Base.Attributes;
using System.ComponentModel;
using BH.oM.Adapters.TAS.Settings;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        [Description("Create a TASSettings object for use with the TAS adapters")]
        [Input("distanceTolerance", "distanceTolerance is used as input for CleanPolyline method used for opening, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("planarTolerance", "Set tolerance for planar surfaces, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("angleTolerance", "The tolerance of the angle that defines a straight line. Default is set to the value defined by BH.oM.Geometry.Tolerance.Angle")]
        [Input("minimumSegmentLength", "The length of the smallest allowed segment.Default is set to the value defined by BH.oM.Geometry.Tolerance.Distance")]
        [Output("tasSettings", "The TAS setting to use with the TAS adapter")]
        [ToBeRemoved("4.0", "Deprecated in favour of default create methods for settings objects.")]
        public static TASSettings TASSettings(double planarTolerance = BH.oM.Geometry.Tolerance.Distance, double distanceTolerance = BH.oM.Geometry.Tolerance.Distance, double minimumSegmentLength = BH.oM.Geometry.Tolerance.Distance, double angleTolerance = BH.oM.Geometry.Tolerance.Angle)
        {
            return new TASSettings
            {
                PlanarTolerance = planarTolerance,
                DistanceTolerance = distanceTolerance,
                MinimumSegmentLength = minimumSegmentLength,
                AngleTolerance = angleTolerance,
            };
        }
    }
}


