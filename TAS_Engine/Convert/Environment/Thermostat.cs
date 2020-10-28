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
using BHE = BH.oM.Environment.SpaceCriteria;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.oM.Adapters.TAS.Fragments;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets BHoM Thermostat from TAS TBD Thermostat")]
        [Input("tbdThermostat", "TAS TBD Thermostat")]
        [Output("BHoM Environmental Thermostat object")]
        public static BHE.Thermostat FromTAS(this TBD.Thermostat tbdThermostat)
        {
            if (tbdThermostat == null) return null;

            BHE.Thermostat thermostat = new BHE.Thermostat();
            thermostat.Name = tbdThermostat.name;
            thermostat.ControlRange = tbdThermostat.controlRange;
            thermostat.ProportionalControl = tbdThermostat.proportionalControl != 0;

            TASThermostatData tasData = new TASThermostatData();
            TASDescription tasDescription = new TASDescription();

            if (tasData != null)
            {
                tasData.RadiantProportion = tbdThermostat.radiantProportion;
                tasDescription.Description = tbdThermostat.description;
                thermostat.Fragments.Add(tasData);
                thermostat.Fragments.Add(tasDescription);
            }

            thermostat.Profiles = tbdThermostat.Profiles();
            return thermostat;
        }

        [Description("Gets TAS TBD Thermostat from BHoM Thermostat")]
        [Input("thermostat", "BHoM Environmental Thermostat object")]
        [Output("TAS TBD Thermostat")]
        public static TBD.Thermostat ToTAS(this BHE.Thermostat thermostat, TBD.Thermostat tbdThermostat)
        {
            if (thermostat == null) return tbdThermostat;

            tbdThermostat.name = thermostat.Name;
            tbdThermostat.controlRange = (float)thermostat.ControlRange;
            tbdThermostat.proportionalControl = (thermostat.ProportionalControl ? 1 : 0);

            Dictionary<string, object> tasData = thermostat.CustomData;

            if (tasData != null)
            {
                tbdThermostat.radiantProportion = (tasData.ContainsKey("RadiantProportion") ? (float)System.Convert.ToDouble(tasData["RadiantProportion"]) : 0);
                tbdThermostat.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
            }
            return tbdThermostat;
        }
    }
}

