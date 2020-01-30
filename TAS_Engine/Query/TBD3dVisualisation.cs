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

using BH.oM.Base;
using BH.oM.Geometry;

using TAS3D;
using TBD;
using TSD;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.InteropServices;
using BH.oM.Environment.Elements;

using BH.oM.Environment.Fragments;
using System;

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        [Description("Get TAS TBD 3d Visulisation, when finish close window and click False to end process. Use Shift + Middle mouse")]
        [Input("tbdFile", "The full file path of the TBD file")]
        [Input("run", "Set to true when you want to run the component, default false")]
        [Output("success", "True if TBD file has been successfully run, false otherwise")]
        public static bool TBD3dVisulisation(string tbdFile, bool run = false)
        {

            if (!run) return false;

            if (string.IsNullOrEmpty(tbdFile))
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide a valid TBD file path");
                return false;
            }


            TBDDocument tbdDocument = new TBDDocument();
            if (tbdDocument != null)
            {
                tbdDocument.openReadOnly(tbdFile);
                tbdDocument.ShowVisualisation();

            }


            return true;
        }

        ////awaing for EDSL implementation
        //[Description("BH.Engine.TAS Query- Get TAS TBD 3d Visulisation, when finish close window and click False to end process. Use Shift + Middle mouse")]
        //[Input("tbdFile", "The full file path of the TBD file")]
        //[Input("run", "Set to true when you want to run the component, default false")]
        //[Output("success", "True if TBD file has been successfully run, false otherwise")]
        //public static bool TSD3dVisulisation(string tsdFile, bool run = false)
        //{

        //    if (!run) return false;

        //    if (string.IsNullOrEmpty(tsdFile))
        //    {
        //        BH.Engine.Reflection.Compute.RecordError("Please provide a valid TBD file path");
        //        return false;
        //    }

        //    TSDDocument tsdDocument = new TSDDocument();
        //    tsdDocument.openReadOnly(tsdFile);
        //    tsdDocument.ShowVisualisation();


        //    return true;
        //}


    }
}

