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

using BH.oM.Base;
using BH.oM.Geometry;

using TAS3D;
using TBD;
using TSD;

using System.ComponentModel;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.InteropServices;
using BH.oM.Environment.Elements;

using BH.oM.Environment.Fragments;
using System;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Compute
    {

        [Description("Run Design Day Sizing")]
        [Input("tbdFile", "The full file path of the TBD file")]
        [Input("run", "Set to true when you want to run the component, default false")]
        [Output("success", "True if sizing has been successfully run, false otherwise")]
        public static bool TBDSizing(string tbdFile, bool run = false)
        {

            if (!run) return false;

            if (string.IsNullOrEmpty(tbdFile))
            {
                BH.Engine.Base.Compute.RecordError("Please provide a valid TBD file path");
                return false;
            }

            TBDDocument tbdDocument = new TBDDocument();
            tbdDocument.open(tbdFile);
            if (tbdDocument != null)
                tbdDocument.sizing(0);
            tbdDocument.save();
            tbdDocument.close();

            int intrefcountTBD = 0;
            do
            {
                intrefcountTBD = Marshal.FinalReleaseComObject(tbdDocument);
            } while (intrefcountTBD > 0);
            tbdDocument = null;

            return true;

        }

        [Description("Run Dynamic Simulation")]
        [Input("tbdFile", "The full file path of the TBD file")]
        [Input("startDay", "Set start day, default 0")]
        [Input("endDay", "Set end day, default 365")]
        [Input("autoViewResults", "Set to true when you want to open results when simulation complete, default false")]
        [Input("run", "Set to true when you want to run the component, default false")]
        [Output("success", "True if simulation has been successfully run, false otherwise")]
        public static bool TBDSizing(string tbdFile, int startDay = 0, int endDay = 365, bool autoViewResults = false, bool run = false)
        {

            if (!run) return false;

            if (!run) return false;

            if (string.IsNullOrEmpty(tbdFile))
            {
                BH.Engine.Base.Compute.RecordError("Please provide a valid TBD file path");
                return false;
            }

            TBDDocument tbdDocument = new TBDDocument();
            tbdDocument.open(tbdFile);

            string tsdFile = tbdFile.Replace(".tbd", ".tsd");
            if (tbdDocument != null)
            {
                tbdDocument.simulate(startDay, endDay, autoViewResults ? 1 : 0, 1, 0, 0, tsdFile, 0, 0);
                tbdDocument.save();
                tbdDocument.close();

            }

            int intrefcountTBD = 0;
            do
            {
                intrefcountTBD = Marshal.FinalReleaseComObject(tbdDocument);
            } while (intrefcountTBD > 0);
            tbdDocument = null;

            return true;

        }

    }
}



