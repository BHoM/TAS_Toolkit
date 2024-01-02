/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHS = BH.oM.Spatial.SettingOut;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Data;

using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets a list of BHoM Architectural Levels from a TAS TBD Building")]
        [Input("tbdBuilding", "TAS TBD Building")]
        [Output("BHoM Architectural Levels")]
        public static List<BHS.Level> FromTASLevels(this TBD.Building tbdBuilding)
        {
            List<BHS.Level> levels = new List<BHS.Level>();

            int storeyIndex = 0;
            TBD.BuildingStorey storey = null;
            while((storey = tbdBuilding.GetStorey(storeyIndex)) != null)
            {
                levels.Add(storey.FromTAS());
                storeyIndex++;
            }

            return levels;
        }

        [Description("Gets a BHoM Architectural Level from a TAS TBD Building Storey")]
        [Input("tbdStorey", "TAS TBD Building Storey")]
        [Output("BHoM Architectural Level")]
        public static BHS.Level FromTAS(this TBD.BuildingStorey tbdStorey)
        {
            BHS.Level level = new BHS.Level();

            if (tbdStorey.GetPerimeter(0) != null)
            {
                BHG.Polyline levelCurve = tbdStorey.GetPerimeter(0).FromTAS();
                double elevation = levelCurve.ControlPoints.First().Z;
                level.Name = "Level " + elevation.Round();
                level.Elevation = elevation.Round();
            }

            return level;
        }

        [Description("Gets a BHoM Architectural Level from a TAS T3D Floor")]
        [Input("tbdFloor", "TAS T3D Floor")]
        [Output("BHoM Architectural Level")]
        public static BHS.Level FromTAS(this TAS3D.Floor tbdFloor)
        {
            BHS.Level level = new BHS.Level();

            level.Name = tbdFloor.name;
            level.Elevation = tbdFloor.level.Round();

            return level;
        }
        /*
        [Description("BH.Engine.Adapters.TAS.Convert ToTAS => gets a TAS TBD Building from a list of BHoM Architecture Levels")]
        [Input("levels","BHoM Architectural Levels")]
        [Output("TAS TBD Building")]
        public static TBD.BuildingStoreyClass ToTAS(this List<BHS.Level> levels)
        {
            TBD.BuildingStoreyClass tbdLevels = new TBD.BuildingStoreyClass();
            if (levels == null) return tbdLevels;

            return tbdLevels; 
        }
        */
        [Description("Gets a TAS TBD Building Storey from a BHoM Architecture Level")]
        [Input("level", "BHoM Architecture Level")]
        [Output("TAS TBD Building Storey")]
        public static TBD.BuildingStoreyClass ToTAS(this BHS.Level level)
        {
            TBD.BuildingStoreyClass tbdLevel = new TBD.BuildingStoreyClass();
            if (level == null) return tbdLevel;

            //Hard to build the tbdLevel when the BHA Level does not store any geometry information at this stage...
            return tbdLevel; //ToDo: do this better with geometry information from somewhere else...
        }
    }
}





