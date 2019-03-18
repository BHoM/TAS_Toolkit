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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BH.Adapter.TAS;
using BH.Engine.TAS;
using BH.oM.Environment.Elements;
using System.Collections.Generic;
using BH.oM.TAS;
using BH.oM.Geometry;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;
using BHP = BH.oM.Environment.Properties;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TAS_Test.Engine.Convert
{
    [TestClass]
    public class ToBHoM_Building
    {
        [TestMethod]
        public void TestToBHoM_Building()
        {
            //Determines whether a Building created by ToBHoM has it's given name.   
            BHE.Building building = new BHE.Building();
            string buildingName = "TestBuildingName";
            building.Name = buildingName;
            Assert.IsTrue(building.Name.Equals("TestBuildingName"));
        }
    }
}
          