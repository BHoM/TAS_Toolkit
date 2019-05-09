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
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BH.Engine.Environment;
using BH.oM.Geometry;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static bool ElementIsOpening(this TBD.BuildingElementType tbdType)
        {
            switch (tbdType)
            {
                case TBD.BuildingElementType.ROOFLIGHT:
                case TBD.BuildingElementType.DOORELEMENT:
                case TBD.BuildingElementType.VEHICLEDOOR:
                case TBD.BuildingElementType.GLAZING:
                case TBD.BuildingElementType.CURTAINWALL:
                case TBD.BuildingElementType.FRAMEELEMENT:
                    return true;
                case TBD.BuildingElementType.NOBETYPE:
                case TBD.BuildingElementType.NULLELEMENT:
                    return false;
                default:
                    return false;
            }
        }

        public static bool OpeningIsFrame(this BHEE.OpeningType openingType)
        {
            switch(openingType)
            {
                case oM.Environment.Elements.OpeningType.Frame:
                case BHEE.OpeningType.RooflightWithFrame:
                case BHEE.OpeningType.WindowWithFrame:
                    return true;

                default:
                    return false;
            }
        }
    }
}