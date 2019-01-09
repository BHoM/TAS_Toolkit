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

using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        public static BHE.Space ToBHoM(this TBD.zone tbdSpace)
        {
            BHE.Space space = new BHE.Space();
            space.Number = tbdSpace.number.ToString();
            space.Name = tbdSpace.name;
            space.CoolingLoad = tbdSpace.maxCoolingLoad;
            space.HeatingLoad = tbdSpace.maxHeatingLoad;

            TBD.InternalCondition tbdCondition = null;
            int conditionIndex = 0;
            while((tbdCondition = tbdSpace.GetIC(conditionIndex)) != null)
            {
                space.InternalConditions.Add(tbdCondition.ToBHoM());
                conditionIndex++;
            }

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("SpaceColour", tbdSpace.colour);
            tasData.Add("DaylightFactor", tbdSpace.daylightFactor);
            tasData.Add("Description", tbdSpace.description);
            tasData.Add("ExposedPerimeter", tbdSpace.exposedPerimeter);
            tasData.Add("External", tbdSpace.external);
            tasData.Add("FacadeLength", tbdSpace.facadeLength);
            tasData.Add("FixedConvectionCoefficient", tbdSpace.fixedConvectionCoefficient);
            tasData.Add("FloorArea", tbdSpace.floorArea);
            tasData.Add("GUID", tbdSpace.GUID);
            tasData.Add("Length", tbdSpace.length);
            tasData.Add("SizeCooling", tbdSpace.sizeCooling);
            tasData.Add("SizeHeating", tbdSpace.sizeHeating);
            tasData.Add("Volume", tbdSpace.volume);
            tasData.Add("WallFloorAreaRatio", tbdSpace.wallFloorAreaRatio);

            space.CustomData.Add("TASData", tasData);

            return space;
        }
    }
}
