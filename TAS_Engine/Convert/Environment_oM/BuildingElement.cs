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
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental BuildingElement from a TAS TBD BuildingElement and TAS TBD ZoneSurface")]
        [Input("tbdElement", "TAS TBD BuildingElement")]
        [Input("tbdSurface", "TAS TBD ZoneSurface")]
        [Output("BHoM Environmental BuildingElement")]
        public static BHE.BuildingElement ToBHoM(this TBD.buildingElement tbdElement, TBD.zoneSurface tbdSurface)
        {
            BHE.BuildingElement element = new BHE.BuildingElement();

            element.Name = tbdElement.name;
            element.BuildingElementProperties.Name = tbdElement.name;
            element.BuildingElementProperties.BuildingElementType = ((TBD.BuildingElementType)tbdElement.BEType).ToBHoM().FixType(tbdElement, tbdSurface);

            //Space/Adjacent IDs...
            element.CustomData.Add("SpaceID", tbdSurface.zone.name);
            if ((int)tbdSurface.type == 3)
                element.CustomData.Add("AdjacentSpaceID", tbdSurface.linkSurface.zone.name);
            else
                element.CustomData.Add("AdjacentSpaceID", -1);

            Dictionary<string, object> tasData = new Dictionary<string, object>();
            tasData.Add("SurfaceGUID", tbdSurface.GUID);
            tasData.Add("SurfaceName", "Z_" + tbdSurface.zone.number + "_" + tbdSurface.number + "_" + tbdSurface.zone.name);
            tasData.Add("SurfaceType", tbdSurface.type);
            tasData.Add("SurfaceAltitude", tbdSurface.altitude);
            tasData.Add("SurfaceAltitudeRange", tbdSurface.altitudeRange);
            tasData.Add("SurfaceArea", tbdSurface.area);
            tasData.Add("SurfaceInclination", tbdSurface.inclination);
            tasData.Add("SurfaceInternalArea", tbdSurface.internalArea);
            tasData.Add("SurfaceOrientation", tbdSurface.orientation);
            tasData.Add("SurfaceReversed", tbdSurface.reversed);
            tasData.Add("ElementColour", Query.GetRGB(tbdElement.colour));
            tasData.Add("ElementDescription", tbdElement.description);
            tasData.Add("ElementIsAir", tbdElement.ghost != 0);
            tasData.Add("ElementIsGround", tbdElement.ground != 0);
            tasData.Add("ElementGUID", tbdElement.GUID);
            tasData.Add("ElementWidth", tbdElement.width);
            tasData.Add("ElementGValue", tbdElement.GValue());
            tasData.Add("ElementLTValue", tbdElement.LTValue());
            tasData.Add("ElementUValue", tbdElement.UValue());
            tasData.Add("MaterialLayersThickness", tbdElement.GetConstruction().ConstructionThickness());

            return element;

        }

        /*[Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Building from a BHoM Environmental Building")]
        [Input("building", "BHoM Environmental Building")]
        [Output("TAS TBD Building")]
        public static TBD.BuildingClass ToTAS(this BHE.Building building)
        {

        }*/
    }
}
