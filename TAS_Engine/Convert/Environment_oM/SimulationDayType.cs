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
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.SimulationDayType from TAS TBD Day Type")]
        [Input("tbdDayType", "TAS TBD day type")]
        [Output("BHoM Environmental Simulation Day Type enum")]
        public static BHE.SimulationDayType ToBHoM(this TBD.dayType tbdDayType)
        {
            if (tbdDayType.name.Equals("Weekday"))
                return BHE.SimulationDayType.Weekday;
            if (tbdDayType.name.Equals("Monday"))
                return BHE.SimulationDayType.Monday;
            if (tbdDayType.name.Equals("Tuesday"))
                return BHE.SimulationDayType.Tuesday;
            if (tbdDayType.name.Equals("Wednesday"))
                return BHE.SimulationDayType.Wednesday;
            if (tbdDayType.name.Equals("Thursday"))
                return BHE.SimulationDayType.Thursday;
            if (tbdDayType.name.Equals("Friday"))
                return BHE.SimulationDayType.Friday;
            if (tbdDayType.name.Equals("Saturday"))
                return BHE.SimulationDayType.Saturday;
            if (tbdDayType.name.Equals("Sunday"))
                return BHE.SimulationDayType.Sunday;
            if (tbdDayType.name.Equals("Public Holiday"))
                return BHE.SimulationDayType.PublicHoliday;
            if (tbdDayType.name.Equals("CDD"))
                return BHE.SimulationDayType.CoolingDesignDay;
            if (tbdDayType.name.Equals("HDD"))
                return BHE.SimulationDayType.HeatingDesignDay;
            if (tbdDayType.name.Equals("Weekend"))
                return BHE.SimulationDayType.Weekend;

            return BHE.SimulationDayType.Undefined;
        }

        public static TBD.dayTypeClass ToTAS(this BHE.SimulationDayType dayType)
        {
            TBD.dayTypeClass tbdDayType = new TBD.dayTypeClass();

            switch(dayType)
            {
                case BHE.SimulationDayType.Weekday:
                    tbdDayType.name = "Weekday";
                    break;
                case BHE.SimulationDayType.Monday:
                    tbdDayType.name = "Monday";
                    break;
                case BHE.SimulationDayType.Tuesday:
                    tbdDayType.name = "Tuesday";
                    break;
                case BHE.SimulationDayType.Wednesday:
                    tbdDayType.name = "Wednesday";
                    break;
                case BHE.SimulationDayType.Thursday:
                    tbdDayType.name = "Thursday";
                    break;
                case BHE.SimulationDayType.Friday:
                    tbdDayType.name = "Friday";
                    break;
                case BHE.SimulationDayType.Saturday:
                    tbdDayType.name = "Saturday";
                    break;
                case BHE.SimulationDayType.Sunday:
                    tbdDayType.name = "Sunday";
                    break;
                case BHE.SimulationDayType.PublicHoliday:
                    tbdDayType.name = "Public Holiday";
                    break;
                case BHE.SimulationDayType.CoolingDesignDay:
                    tbdDayType.name = "CDD";
                    break;
                case BHE.SimulationDayType.HeatingDesignDay:
                    tbdDayType.name = "HDD";
                    break;
                case BHE.SimulationDayType.Weekend:
                    tbdDayType.name = "Weekend";
                    break;
                default:
                    tbdDayType.name = "";
                    break;
            }

            return tbdDayType;
        }
    }
}
