﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using BH.oM.Adapter;
using BH.oM.Environment.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Adapters.TAS
{
    public class TASTSDConfig : ActionConfig, ITASConfig
    {
        //TASTSD properties
        public virtual FileSettings TSDFile { get; set; } = null;
        public TSDResultType ResultQuery { get; set; } = TSDResultType.Simulation;
        public SimulationResultType SimulationType { get; set; } = SimulationResultType.BuildingResult;
        public ProfileResultUnit ResultUnit { get; set; } = ProfileResultUnit.Yearly;
        public ProfileResultType ResultType { get; set; } = ProfileResultType.TemperatureExternal;
        public int Hour { get; set; } = -1;
        public int Day { get; set; } = -1;

        //TASSettings properties
        public virtual double DistanceTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double PlanarTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double MinimumSegmentLength { get; set; } = BH.oM.Geometry.Tolerance.Distance;
        public virtual double AngleTolerance { get; set; } = BH.oM.Geometry.Tolerance.Angle;
    }
}
