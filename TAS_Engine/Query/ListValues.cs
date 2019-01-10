/*
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

namespace BH.Engine.TAS
{
    public static partial class Query
    {
        public static double GValue(this TBD.buildingElement tbdBuildingElement, int decimals = 3)
        {
            TBD.Construction aConstruction = tbdBuildingElement.GetConstruction();
            if (aConstruction == null)
                return -1;
            TBD.ConstructionTypes aConstructionTypes = aConstruction.type;
            if (aConstructionTypes == TBD.ConstructionTypes.tcdTransparentConstruction)
            {
                object aObject = aConstruction.GetGlazingValues();
                List<float> aValueList = Convert.ToFloatList(aObject);
                return Math.Round(aValueList[5], decimals);
            }
            return 0;
        }

        public static double LTValue(this TBD.buildingElement tbdBuildingElement, int decimals = 3)
        {
            TBD.Construction aConstruction = tbdBuildingElement.GetConstruction();
            if (aConstruction == null)
                return 0;

            TBD.ConstructionTypes aConstructionTypes = aConstruction.type;
            if (aConstructionTypes == TBD.ConstructionTypes.tcdTransparentConstruction)
            {
                object aObject = aConstruction.GetGlazingValues();
                List<float> aValueList = Convert.ToFloatList(aObject);
                return Math.Round(aValueList[0], decimals);
            }
            return 0;
        }

        public static double UValue(this TBD.buildingElement tbdBuildingElement, int decimals = 3)
        {
            TBD.Construction aConstruction = tbdBuildingElement.GetConstruction();
            if (aConstruction == null)
                return -1;

            object aObject = aConstruction.GetUValue();
            List<float> aValueList = Convert.ToFloatList(aObject);
            switch ((TBD.BuildingElementType)tbdBuildingElement.BEType)
            {
                case TBD.BuildingElementType.CEILING:
                    return Math.Round(aValueList[4], decimals);
                case TBD.BuildingElementType.CURTAINWALL:
                    return Math.Round(aValueList[6], decimals);
                case TBD.BuildingElementType.DOORELEMENT:
                    return Math.Round(aValueList[0], decimals);
                case TBD.BuildingElementType.EXPOSEDFLOOR:
                    return Math.Round(aValueList[2], decimals);
                case TBD.BuildingElementType.EXTERNALWALL:
                    return Math.Round(aValueList[0], decimals);
                case TBD.BuildingElementType.FRAMEELEMENT:
                    return Math.Round(aValueList[0], decimals);
                case TBD.BuildingElementType.GLAZING:
                    return Math.Round(aValueList[6], decimals);
                case TBD.BuildingElementType.INTERNALFLOOR:
                    return Math.Round(aValueList[5], decimals);
                case TBD.BuildingElementType.INTERNALWALL:
                    return Math.Round(aValueList[3], decimals);
                case TBD.BuildingElementType.NOBETYPE:
                    return -1;
                case TBD.BuildingElementType.NULLELEMENT:
                    return -1;
                case TBD.BuildingElementType.RAISEDFLOOR:
                    return Math.Round(aValueList[5], decimals);
                case TBD.BuildingElementType.ROOFELEMENT:
                    return Math.Round(aValueList[1], decimals);
                case TBD.BuildingElementType.ROOFLIGHT:
                    return Math.Round(aValueList[6], decimals);
                case TBD.BuildingElementType.SHADEELEMENT:
                    return -1;
                case TBD.BuildingElementType.SLABONGRADE:
                    return Math.Round(aValueList[2], decimals);
                case TBD.BuildingElementType.SOLARPANEL:
                    return -1;
                case TBD.BuildingElementType.UNDERGROUNDCEILING:
                    return Math.Round(aValueList[2], decimals);
                case TBD.BuildingElementType.UNDERGROUNDSLAB:
                    return Math.Round(aValueList[2], decimals);
                case TBD.BuildingElementType.UNDERGROUNDWALL:
                    return Math.Round(aValueList[0], decimals);
                case TBD.BuildingElementType.VEHICLEDOOR:
                    return Math.Round(aValueList[0], decimals);
            }
            return -1;
        }
    }
}
