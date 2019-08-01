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
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;
using BH.Engine.Environment;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public partial class TasT3DAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        
        //private bool Delete(BHE.Elements.Space space, IEnumerable<BHE.Elements.Space> spaces)
        //{

        //    TAS3D.zone t3dZone = m_T3DDocument.Building.AddZone();
        //    TAS3D.room t3dRoom = t3dZone.AddRoom();
        //    tbdZone = Engine.TAS.Convert.ToTAS3D(space, t3dZone);


        //    foreach (BHE.Elements.BuildingElement element in space.BuildingElements)
        //    {
        //        //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
        //        TAS3D.buildingElement be = m_T3DDocument.Building.AddBuildingElement();

        //        //Add zoneSrf and convert it
        //        TAS3D.zoneSurface t3dZoneSrf = t3dZone.AddSurface();
        //        t3dZoneSrf = Engine.TAS.Convert.ToTAS3D(element.BuildingElementGeometry, t3dZoneSrf);

        //        //Add roomSrf, create face, get its controlpoints and convert to TAS
        //        TAS3D.Polygon t3dPolygon = t3dRoom.AddSurface().CreatePerimeter().CreateFace();
        //        t3dPolygon = Engine.TAS.Convert.ToTAS3D(element.BuildingElementGeometry.ICurve(), t3dPolygon);

        //        //Set the building Element
        //        t3dZoneSrf.buildingElement = Engine.TAS.Convert.ToTAS3D(element, be);

        //        //tasZoneSrf.type = BH.Engine.TAS.Query.GetSurfaceType(element, spaces);
        //        t3dZoneSrf.orientation = (float)BH.Engine.Environment.Query.Azimuth(element.BuildingElementGeometry, new BHG.Vector());
        //        //tasZoneSrf.orientation = BH.Engine.TAS.Query.GetOrientation(element.BuildingElementGeometry, bHoMSpace);
        //        t3dZoneSrf.inclination = (float)BH.Engine.Environment.Query.Tilt(element.BuildingElementGeometry);
        //        //tasZoneSrf.inclination = BH.Engine.TAS.Query.GetInclination(element.BuildingElementGeometry, bHoMSpace);
        //    }
        //    return true;
        //}

        /***************************************************/
    }
}
