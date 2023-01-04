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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BHG = BH.oM.Geometry;
//using BHE = BH.oM.Environment;
//using BHEE = BH.oM.Environment.Elements;
//using BHEI = BH.oM.Environment.Interface;
//using BH.Engine.Environment;

//namespace BH.Engine.Adapters.TAS
//{
//    public static partial class Query
//    {
//        /***************************************************/

//        public static TBD.SurfaceType GetSurfaceType(BHE.Elements.BuildingElement bHoMBuildingElement, IEnumerable<BHE.Elements.Space> spaces)
//        {

//            List<BHE.Elements.Space> adjSpace = BH.Engine.Environment.Query.AdjacentSpaces(bHoMBuildingElement, spaces, "");

//            if (adjSpace == null)
//                return TBD.SurfaceType.tbdNullLink; //Adiabatic; 



//            if (adjSpace.Count <= 1)
//            {
//                if (bHoMBuildingElement.Level.Elevation < 0) //under ground. is this correct?
//                    return TBD.SurfaceType.tbdGround;
//                else
//                    return TBD.SurfaceType.tbdExposed;
//            }

//            else if (adjSpace.Count > 1)
//                return TBD.SurfaceType.tbdLink; //TODO: if it is linked we want to know to which space. 
//            else
//                return TBD.SurfaceType.tbdNullLink; //Adiabatic


//        }

//        /***************************************************/
//    }
//}




