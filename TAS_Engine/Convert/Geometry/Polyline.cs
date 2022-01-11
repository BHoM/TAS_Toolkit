/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.Engine.Geometry;
using BH.Engine.Adapters.TAS;
using BH.oM.Base.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets a BHoM Geomtry Polyline TAS TBD Perimeter")]
        [Input("tbdPerimeter", "TAS TBD Perimeter")]
        [Output("BHoM Geometry Polyline")]
        public static BHG.Polyline FromTAS(this TBD.Perimeter tbdPerimeter)
        {
            return tbdPerimeter.GetFace().FromTAS();
        }

        [Description("Gets a BHoM Geomtry Polyline TAS TBD Polygon")]
        [Input("tbdPolygon", "TAS TBD Polygon")]
        [Output("BHoM Geometry Polyline")]
        public static BHG.Polyline FromTAS(this TBD.Polygon tbdPolygon)
        {
            List<BHG.Point> pnts = new List<BHG.Point>();

            int pIndex = 0;
            TBD.TasPoint tPt = null;

            try
            {

                while ((tPt = tbdPolygon.GetPoint(pIndex)) != null)
                {
                    pnts.Add(tPt.FromTAS());
                    pIndex++;
                }

                //if(pnts.First().Distance(pnts.Last()) > BHG.Tolerance.Distance)
                if (pnts.First() != pnts.Last())
                    pnts.Add(pnts[0]); //Close the polyline
            }
            catch (Exception ex)
            {
                BH.Engine.Base.Compute.RecordError(ex.ToString());

            }

            return new BHG.Polyline { ControlPoints = pnts };
        }

        [Description("Gets a TAS TBD Perimeter from a BHoM Geometry Polyline")]
        [Input("polyline", "BHoM Geomtry Polyline")]
        [Output("TAS TBD Perimeter")]
        public static TBD.Perimeter ToTAS(this BHG.Polyline polyline, TBD.Perimeter tbdPerimeter)
        {
            if (polyline == null) return tbdPerimeter;

            TBD.Polygon poly = tbdPerimeter.CreateFace();
            poly = polyline.ToTASPolygon(poly);

            return tbdPerimeter;
        }

        [Description("Gets a TAS TBD Polygon from a BHoM Geometry Polyline")]
        [Input("polyline", "BHoM Geomtry Polyline")]
        [Output("TAS TBD Polygon")]
        public static TBD.Polygon ToTASPolygon(this BHG.Polyline polyline, TBD.Polygon tbdPolygon)
        {
            if (polyline == null) return tbdPolygon;

            foreach (BHG.Point pt in polyline.ControlPoints)
            {
                TBD.TasPoint p = tbdPolygon.AddPoint();
                p = pt.ToTAS(p);
            }

            return tbdPolygon;
        }
    }
}



