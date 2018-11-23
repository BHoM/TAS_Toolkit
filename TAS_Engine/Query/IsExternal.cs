﻿using System.ComponentModel;

using BH.oM.Base;
using BH.oM.Reflection.Attributes;
using BH.oM.Environment.Elements;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Cheks whatever Building Element is external element. Works only for Building Elements pulled from analytical model and adjacency have been assigned.")]
        [Input("buildingElement", "BuildingElement pulled from TAS model")]
        [Output("IsExternal")]
        public static bool IsExternal(this BuildingElement buildingElement)
        {
            if (buildingElement == null)
                return false;


            //if (buildingElement.CustomData == null)
            //    return false;


            //if (!buildingElement.CustomData.ContainsKey(Convert.SpaceId))
            //    return false;

            //if (!buildingElement.CustomData.ContainsKey(Convert.AdjacentSpaceId))
            //    return false;

            //int aSpaceId = buildingElement.SpaceId();
            //int aAdjacentSpaceId = buildingElement.AdjacentSpaceId();
            BuildingElement abuildingElement = buildingElement;

            //return aSpaceId != -1 && aAdjacentSpaceId == -1;
            return false;
        }

        /***************************************************/
    }
}