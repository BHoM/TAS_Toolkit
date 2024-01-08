using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Interface methods                  ****/
        /***************************************************/

        public static bool ICloseTASDocument(ITASFile document, bool save)
        {
            return CloseTASDocument(document as dynamic, save);
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private static void CloseTASDocument(TBDDocument document, bool save)
        {
            if (save)
                document.Document.save();

            document.Document.close();
            ClearCOMObject(document.Document);
        }

        /***************************************************/

        private static void CloseTASDocument(TSDDocument document, bool save)
        {
            if (save)
                document.Document.save();

            document.Document.close();
            ClearCOMObject(document.Document);
        }

        /***************************************************/
        /**** Fallback methods                          ****/
        /***************************************************/

        private static void CloseTASDocument(ITASFile document, bool save)
        {
            BH.Engine.Base.Compute.RecordError("An error occurred while closing and saving the TAS document.");
        }
    }
}
