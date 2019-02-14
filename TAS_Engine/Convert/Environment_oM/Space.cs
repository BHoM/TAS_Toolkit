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
using BHP = BH.oM.Environment.Properties;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.Space from TAS TBD Zone")]
        [Input("tbdSpace", "TAS TBD Zone")]
        [Output("BHoM Environmental Space object")]
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

            //Adding data to Extended Poroperties--------------------------------------------------------------------------------------------------------------

            //EnvironmentContextProperties
            BHP.EnvironmentContextProperties environmentContextProperties = new BHP.EnvironmentContextProperties();
            environmentContextProperties.ElementID = tbdSpace.GUID.GetCleanGUIDFromTAS();
            environmentContextProperties.Description = tbdSpace.description;
            environmentContextProperties.TypeName = tbdSpace.name;
            space.ExtendedProperties.Add(environmentContextProperties);

            //SpaceContextProperties
            BHP.SpaceContextProperties spaceContextProperties = new BHP.SpaceContextProperties();
            spaceContextProperties.Colour = BH.Engine.TAS.Query.GetRGB(tbdSpace.colour).ToString();
            spaceContextProperties.IsExternal = tbdSpace.external != 0;

            //spaceContextProperties.ConnectedElements = tbdSpace.external != 0;
            space.ExtendedProperties.Add(spaceContextProperties);

            //SpaceAnalyticalProperties
            BHP.SpaceAnalyticalProperties spaceAnalyticalProperties = new BHP.SpaceAnalyticalProperties();
            spaceAnalyticalProperties.DaylightFactor = tbdSpace.daylightFactor;
            spaceAnalyticalProperties.FacadeLength = tbdSpace.facadeLength;
            spaceAnalyticalProperties.FixedConvectionCoefficient = tbdSpace.fixedConvectionCoefficient;
            spaceAnalyticalProperties.SizeCoolingMethod =((TBD.SizingType)tbdSpace.sizeCooling).ToBHoM();
            spaceAnalyticalProperties.SizeHeatingMethod = ((TBD.SizingType)tbdSpace.sizeCooling).ToBHoM();
            space.ExtendedProperties.Add(spaceAnalyticalProperties);

            //Extended Poroperties-------------------------------------------------------------------------------------------------------------------------

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("SpaceColour", tbdSpace.colour);
            tasData.Add("DaylightFactor", tbdSpace.daylightFactor);
            tasData.Add("Description", tbdSpace.description);
            tasData.Add("ExposedPerimeter", tbdSpace.exposedPerimeter);
            tasData.Add("External", tbdSpace.external);
            tasData.Add("FacadeLength", tbdSpace.facadeLength);
            tasData.Add("FixedConvectionCoefficient", tbdSpace.fixedConvectionCoefficient);
            tasData.Add("FloorArea", tbdSpace.floorArea);
            tasData.Add("GUID", tbdSpace.GUID.GetCleanGUIDFromTAS());
            tasData.Add("Length", tbdSpace.length);
            tasData.Add("SizeCooling", tbdSpace.sizeCooling);
            tasData.Add("SizeHeating", tbdSpace.sizeHeating);
            tasData.Add("Volume", tbdSpace.volume);
            tasData.Add("WallFloorAreaRatio", tbdSpace.wallFloorAreaRatio);

            space.CustomData = tasData;

            return space;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD Zone from BH.oM.Environment.Elements.Space")]
        [Input("space", "BHoM Environmental InternalCondition object")]
        [Output("TAS TBD Zone")]
        public static TBD.zoneClass ToTAS(this BHE.Space space)
        {
            TBD.zoneClass tbdSpace = new TBD.zoneClass();
            if (space == null) return tbdSpace;

            tbdSpace.number = System.Convert.ToInt32(space.Number);
            tbdSpace.name = space.Name;
            tbdSpace.maxCoolingLoad = (float)space.CoolingLoad;
            tbdSpace.maxHeatingLoad = (float)space.HeatingLoad;

            foreach(BHE.InternalCondition condition in space.InternalConditions)
                tbdSpace.AssignIC(condition.ToTAS(), true);

            Dictionary<string, object> tasData = space.CustomData;

            if (tasData != null)
            {
                tbdSpace.colour = (tasData.ContainsKey("SpaceColour") ? System.Convert.ToUInt32(tasData["SpaceColour"]) : 0);
                tbdSpace.daylightFactor = (tasData.ContainsKey("DaylightFactor") ? (float)System.Convert.ToDouble(tasData["DaylightFactor"]) : 0);
                tbdSpace.description = (tasData.ContainsKey("Description") ? tasData["Description"].ToString() : "");
                tbdSpace.exposedPerimeter = (tasData.ContainsKey("ExposedPerimeter") ? (float)System.Convert.ToDouble(tasData["ExposedPerimeter"]) : 0);
                tbdSpace.external = (tasData.ContainsKey("External") ? System.Convert.ToInt32(tasData["External"]) : 0);
                tbdSpace.facadeLength = (tasData.ContainsKey("FacadeLength") ? (float)System.Convert.ToDouble(tasData["FacadeLength"]) : 0);
                tbdSpace.fixedConvectionCoefficient = (tasData.ContainsKey("FixedConvectionCoefficient") ? (float)System.Convert.ToDouble(tasData["FixedConvectionCoefficient"]) : 0);
                tbdSpace.floorArea = (tasData.ContainsKey("FloorArea") ? (float)System.Convert.ToDouble(tasData["FloorArea"]) : 0);
                //tbdSpace.GUID = (tasData.ContainsKey("GUID") ? tasData["GUID"].ToString() : "");
                tbdSpace.length = (tasData.ContainsKey("Length") ? (float)System.Convert.ToDouble(tasData["Length"]) : 0);
                tbdSpace.sizeCooling = (tasData.ContainsKey("SizeCooling") ? System.Convert.ToInt32(tasData["SizeCooling"]) : 0);
                tbdSpace.sizeHeating = (tasData.ContainsKey("SizeHeating") ? System.Convert.ToInt32(tasData["SizeHeating"]) : 0);
                tbdSpace.volume = (tasData.ContainsKey("Volume") ? (float)System.Convert.ToDouble(tasData["Volume"]) : 0);
                tbdSpace.wallFloorAreaRatio = (tasData.ContainsKey("WallFloorAreaRatio") ? (float)System.Convert.ToDouble(tasData["WallFloorAreaRatio"]) : 0);
            }

            return tbdSpace;
        }
    }
}
