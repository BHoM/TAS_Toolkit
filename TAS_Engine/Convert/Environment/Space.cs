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
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BHP = BH.oM.Environment.Fragments;

using BH.Engine.Environment;
using BH.oM.Adapters.TAS.Fragments;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets BHoM Space from TAS TBD Zone")]
        [Input("tbdSpace", "TAS TBD Zone")]
        [Output("BHoM Environmental Space object")]
        public static BHE.Space FromTAS(this TBD.zone tbdSpace, TBD.TBDDocument tbdDocument)
        {
            BHE.Space space = new BHE.Space();
            space.Name = tbdSpace.name + tbdSpace.number.ToString();

            BHP.LoadFragment loads = new BHP.LoadFragment();
            loads.CoolingLoad = tbdSpace.maxCoolingLoad;
            loads.HeatingLoad = tbdSpace.maxHeatingLoad;
            space.Fragments.Add(loads);

            //Adding data to Extended Poroperties--------------------------------------------------------------------------------------------------------------

            //EnvironmentContextProperties
            BHP.OriginContextFragment environmentContextProperties = new BHP.OriginContextFragment();
            environmentContextProperties.ElementID = tbdSpace.GUID.RemoveBrackets();
            environmentContextProperties.Description = tbdSpace.description;
            environmentContextProperties.TypeName = tbdSpace.name;
            space.Fragments.Add(environmentContextProperties);

            //SpaceContextProperties
            BHP.SpaceContextFragment spaceContextProperties = new BHP.SpaceContextFragment();
            spaceContextProperties.Colour = BH.Engine.Adapters.TAS.Query.GetRGB(tbdSpace.colour).ToString();
            spaceContextProperties.IsExternal = tbdSpace.external != 0;

            //spaceContextProperties.ConnectedElements = tbdSpace.external != 0;
            space.Fragments.Add(spaceContextProperties);

            //SpaceAnalyticalProperties
            BHP.SpaceAnalyticalFragment spaceAnalyticalProperties = new BHP.SpaceAnalyticalFragment();
            spaceAnalyticalProperties.DaylightFactor = tbdSpace.daylightFactor;
            spaceAnalyticalProperties.FacadeLength = tbdSpace.facadeLength;
            spaceAnalyticalProperties.FixedConvectionCoefficient = tbdSpace.fixedConvectionCoefficient;
            spaceAnalyticalProperties.SizeCoolingMethod =((TBD.SizingType)tbdSpace.sizeCooling).FromTAS();
            spaceAnalyticalProperties.SizeHeatingMethod = ((TBD.SizingType)tbdSpace.sizeCooling).FromTAS();
            space.Fragments.Add(spaceAnalyticalProperties);

            //Extended Properties

            TASSpaceData tasData = new TASSpaceData();
            tasData.Colour = System.Convert.ToUInt32(tbdSpace.colour);
            tasData.DaylightFactor = tbdSpace.daylightFactor;
            tasData.ExposedPerimeter = tbdSpace.exposedPerimeter;
            tasData.External = tbdSpace.external;
            tasData.FacadeLength = tbdSpace.facadeLength;
            tasData.FixedConvectionCoefficient = tbdSpace.fixedConvectionCoefficient;
            tasData.FloorArea = tbdSpace.floorArea;
            tasData.TASID = tbdSpace.GUID.RemoveBrackets();
            tasData.Length = tbdSpace.length;
            tasData.SizeCooling = tbdSpace.sizeCooling;
            tasData.SizeHeating = tbdSpace.sizeHeating;
            tasData.Volume = tbdSpace.volume;
            tasData.WallFloorAreaRatio = tbdSpace.wallFloorAreaRatio;

            TASDescription tasDescription = new TASDescription();
            tasDescription.Description = tbdSpace.description;
            
            //Proces to extract Number of people directly into space if needed
            //double[] YearlyPeopleSensibleSepcificGain = Query.GetNumberOfPeople(tbdDocument, tbdSpace);
            //double MaxSpecificSensibleGain = YearlyPeopleSensibleSepcificGain.Max();
            //double[] YearlyPeopleLatenteSepcificGain = Query.GetNumberOfPeople(tbdDocument, tbdSpace, TBD.Profiles.ticOLG);
            //double MaxSpecificLatentGain = YearlyPeopleLatenteSepcificGain.Max();
            //double NumberOfPeople = PeopleDesity / tbdSpace.floorArea;

            space.Fragments.Add(tasData);
            space.Fragments.Add(tasDescription);

            return space;
        }

        [Description("Gets TAS TBD Zone from BHoM Space")]
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

            TASSpaceData tasFragment = space.FindFragment<TASSpaceData>(typeof(TASSpaceData));
            if (tasFragment != null)
            {
                tbdSpace.colour = tasFragment.Colour;
                tbdSpace.daylightFactor = (float)tasFragment.DaylightFactor;
                tbdSpace.exposedPerimeter = (float)tasFragment.ExposedPerimeter;
                tbdSpace.external = tasFragment.External;
                tbdSpace.facadeLength = (float)tasFragment.FacadeLength;
                tbdSpace.fixedConvectionCoefficient = (float)tasFragment.FixedConvectionCoefficient;
                tbdSpace.floorArea = (float)tasFragment.FloorArea;
                tbdSpace.GUID = tasFragment.TASID;
                tbdSpace.length = (float)tasFragment.Length;
                tbdSpace.sizeCooling = tasFragment.SizeCooling;
                tbdSpace.sizeHeating = tasFragment.SizeHeating;
                tbdSpace.volume = (float)tasFragment.Volume;
                tbdSpace.wallFloorAreaRatio = (float)tasFragment.WallFloorAreaRatio;
            }

            TASDescription tasDescription = space.FindFragment<TASDescription>(typeof(TASDescription));
            if (tasFragment != null)
            {
                tbdSpace.description = tasDescription.Description;
            }

            return tbdSpace;
        }
    }
}

