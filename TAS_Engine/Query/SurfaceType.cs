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
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;
using BHEI = BH.oM.Environment.Interface;
using BH.Engine.Environment;

namespace BH.Engine.TAS
{
    public static partial class Query
    {

        /***************************************************/

        public static TBD.SurfaceType SurfaceType (BHEE.BuildingElement buildingElement)
        {
            object aValue;
            if(buildingElement.BuildingElementProperties.CustomData.TryGetValue("SAM_BuildingElementType", out aValue))
            {
                if(aValue is string)
                {
                    string aSurfaceTypeString = (string)aValue;
                    //AdjacentSpaces needs to be brought back to BuildingElement for this to work.TODO: 
                    //if (buildingElement.AdjacentSpaces.Count == 0)
                    {
                        if (aSurfaceTypeString == "Shade")
                            return TBD.SurfaceType.tbdExposed;

                        //if (aSurfaceTypeString == "Rooflight")
                        //    return TBD.SurfaceType.; Window

                        //if (aSurfaceTypeString == "Glazing")
                        //    return TBD.SurfaceType.; Window

                        //if (aSurfaceTypeString == "Door")
                        //    return TBD.SurfaceType.; Window _offset zero

                        // // we take window from revit and frame is transform in tas
                        //if (aSurfaceTypeString == "Frame")
                        //    return TBD.SurfaceType.; 

                        //if (aSurfaceTypeString == "Solar / PV Panel")
                        //    return TBD.SurfaceType.;
                    }
                    //TODO: AdjacentSpaces needs to be brought back to BuildingElement for this to work.
                    //if (buildingElement.AdjacentSpaces.Count == 1)
                    {

                        if (aSurfaceTypeString == "No Type")
                            return TBD.SurfaceType.tbdNullLink;

                        if (aSurfaceTypeString == "Exposed Floor")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Raised Floor")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Curtain Wall")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Roof")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "External Wall")
                            return TBD.SurfaceType.tbdExposed;

                        if (aSurfaceTypeString == "Slab on Grade")
                            return TBD.SurfaceType.tbdGround;

                        if (aSurfaceTypeString == "Underground Wall")
                            return TBD.SurfaceType.tbdGround;
                    }
                    //TODO: AdjacentSpaces needs to be brought back to BuildingElement for this to work.
                    //if (buildingElement.AdjacentSpaces.Count > 1)
                    {
                        if (aSurfaceTypeString == "No Type")
                            return TBD.SurfaceType.tbdNullLink;

                        if (aSurfaceTypeString == "Internal Wall")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Internal Floor")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Exposed Floor")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Underground Ceiling")
                            return TBD.SurfaceType.tbdLink;

                        if (aSurfaceTypeString == "Internal Ceiling")
                            return TBD.SurfaceType.tbdLink;
                    }
                }
            }

            return TBD.SurfaceType.tbdNullLink;
        }

        /***************************************************/
    }
}
