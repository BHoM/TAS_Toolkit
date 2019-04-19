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

using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets BH.oM.Environment.Elements.Space from TAS TBD Zone")]
        [Input("tbdSpace", "TAS TBD Zone")]
        [Output("BHoM Environmental Space object")]
        public static BHE.Space ToBHoM(this TBD.zone tbdSpace, TBD.TBDDocument tbdDocument)
        {
            BHE.Space space = new BHE.Space();
            space.Name = tbdSpace.name + tbdSpace.number.ToString();

            BHP.LoadFragment loads = new BHP.LoadFragment();
            loads.CoolingLoad = tbdSpace.maxCoolingLoad;
            loads.HeatingLoad = tbdSpace.maxHeatingLoad;
            space.FragmentProperties.Add(loads);

            //Adding data to Extended Poroperties--------------------------------------------------------------------------------------------------------------

            //EnvironmentContextProperties
            BHP.OriginContextFragment environmentContextProperties = new BHP.OriginContextFragment();
            environmentContextProperties.ElementID = tbdSpace.GUID.RemoveBrackets();
            environmentContextProperties.Description = tbdSpace.description;
            environmentContextProperties.TypeName = tbdSpace.name;
            space.FragmentProperties.Add(environmentContextProperties);

            //SpaceContextProperties
            BHP.SpaceContextFragment spaceContextProperties = new BHP.SpaceContextFragment();
            spaceContextProperties.Colour = BH.Engine.TAS.Query.GetRGB(tbdSpace.colour).ToString();
            spaceContextProperties.IsExternal = tbdSpace.external != 0;

            //spaceContextProperties.ConnectedElements = tbdSpace.external != 0;
            space.FragmentProperties.Add(spaceContextProperties);

            //SpaceAnalyticalProperties
            BHP.SpaceAnalyticalFragment spaceAnalyticalProperties = new BHP.SpaceAnalyticalFragment();
            spaceAnalyticalProperties.DaylightFactor = tbdSpace.daylightFactor;
            spaceAnalyticalProperties.FacadeLength = tbdSpace.facadeLength;
            spaceAnalyticalProperties.FixedConvectionCoefficient = tbdSpace.fixedConvectionCoefficient;
            spaceAnalyticalProperties.SizeCoolingMethod =((TBD.SizingType)tbdSpace.sizeCooling).ToBHoM();
            spaceAnalyticalProperties.SizeHeatingMethod = ((TBD.SizingType)tbdSpace.sizeCooling).ToBHoM();
            space.FragmentProperties.Add(spaceAnalyticalProperties);

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
            tasData.Add("GUID", tbdSpace.GUID.RemoveBrackets());
            tasData.Add("Length", tbdSpace.length);
            tasData.Add("SizeCooling", tbdSpace.sizeCooling);
            tasData.Add("SizeHeating", tbdSpace.sizeHeating);
            tasData.Add("Volume", tbdSpace.volume);
            tasData.Add("WallFloorAreaRatio", tbdSpace.wallFloorAreaRatio);

            //Proces to extract Number of people directly into space if needed
            //double[] YearlyPeopleSensibleSepcificGain = Query.GetNumberOfPeople(tbdDocument, tbdSpace);
            //double MaxSpecificSensibleGain = YearlyPeopleSensibleSepcificGain.Max();
            //double[] YearlyPeopleLatenteSepcificGain = Query.GetNumberOfPeople(tbdDocument, tbdSpace, TBD.Profiles.ticOLG);
            //double MaxSpecificLatentGain = YearlyPeopleLatenteSepcificGain.Max();
            //double NumberOfPeople = PeopleDesity / tbdSpace.floorArea;

            space.CustomData = tasData;

            return space;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets TAS TBD Zone from BH.oM.Environment.Elements.Space")]
        [Input("space", "BHoM Environmental InternalCondition object")]
        [Output("TAS TBD Zone")]
        public static TBD.zone ToTAS(this BHE.Space space, TBD.zone tbdSpace)
        {
            //TODO:Assign Internal Conditions to Zones
            //TODO:Assign Building Elements (Surfaces) to Zones

            if (space == null) return tbdSpace;
            tbdSpace.name = space.Name;

            BHP.LoadFragment loads = space.FindFragment<BHP.LoadFragment>(typeof(BHP.LoadFragment));

            if (loads != null)
            {
                tbdSpace.maxHeatingLoad = (float)loads.HeatingLoad;
                tbdSpace.maxCoolingLoad = (float)loads.CoolingLoad;
            }

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
                tbdSpace.GUID = (tasData.ContainsKey("GUID") ? tasData["GUID"].ToString() : "");
                tbdSpace.length = (tasData.ContainsKey("Length") ? (float)System.Convert.ToDouble(tasData["Length"]) : 0);
                tbdSpace.sizeCooling = (tasData.ContainsKey("SizeCooling") ? System.Convert.ToInt32(tasData["SizeCooling"]) : 0);
                tbdSpace.sizeHeating = (tasData.ContainsKey("SizeHeating") ? System.Convert.ToInt32(tasData["SizeHeating"]) : 0);
                //tbdSpace.variableConvectionCoefficient = (tasData.ContainsKey("variableConvectionCoefficient") ? tasData["variableConvectionCoefficient"].ToString() : "");
                tbdSpace.volume = (tasData.ContainsKey("Volume") ? (float)System.Convert.ToDouble(tasData["Volume"]) : 0);
                tbdSpace.wallFloorAreaRatio = (tasData.ContainsKey("WallFloorAreaRatio") ? (float)System.Convert.ToDouble(tasData["WallFloorAreaRatio"]) : 0);
            }

            return tbdSpace;
        }
    }
}
