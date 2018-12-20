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
using TSD;

namespace BH.Engine.TAS.Generic
{
    public enum DataType
    {
        /// <summary>Building Data</summary>
        BuildingData,
        /// <summary>Cooling Design Data</summary>
        CoolingDesignData,
        /// <summary>Heating Design Data</summary>
        HeatingDesignData,
    }

    internal static class Functions
    {
        //internal static void GetMax(ZoneData ZoneData, out float Max, out int Index, TSD.tsdZoneArray ZoneArray)
        //{
        //    if (ZoneData == null)
        //    {
        //        Max = float.MinValue;
        //        Index = -1;
        //        return;
        //    }

        //    Max = float.NaN;
        //    List<float> aFloatList = GetAnnualZoneResultList(ZoneData, ZoneArray);
        //    Max = aFloatList.Max();
        //    Index = aFloatList.IndexOf(Max);
        //}

        //internal static void GetMax(List<ZoneData> ZoneDataList, out float Max, out int Index, out ZoneData ZoneData, TSD.tsdZoneArray ZoneArray)
        //{
        //    Max = float.MinValue;
        //    Index = -1;
        //    ZoneData = null;

        //    for (int i = 0; i < ZoneDataList.Count; i++)
        //    {
        //        List<float> aResultList = GetAnnualZoneResultList(ZoneDataList[i], ZoneArray);
        //        float aMax = aResultList.Max();
        //        if (aMax > Max)
        //        {
        //            Max = aMax;
        //            Index = aResultList.IndexOf(aMax);
        //            ZoneData = ZoneDataList[i];
        //        }
        //    }
        //}

        //internal static void GetMax(List<ZoneData> ZoneDataList, int Index, out float Max, out ZoneData ZoneData, TSD.tsdZoneArray ZoneArray)
        //{
        //    Max = float.MinValue;
        //    ZoneData = null;

        //    for (int i = 0; i < ZoneDataList.Count; i++)
        //    {
        //        List<float> aResultList = GetAnnualZoneResultList(ZoneDataList[i], ZoneArray);
        //        float aMax = aResultList[Index];
        //        if (aMax > Max)
        //        {
        //            Max = aMax;
        //            ZoneData = ZoneDataList[i];
        //        }
        //    }
        //}

        //internal static void GetMin(List<ZoneData> ZoneDataList, int Index, out float Min, out ZoneData ZoneData, TSD.tsdZoneArray ZoneArray)
        //{
        //    Min = float.MaxValue;
        //    ZoneData = null;

        //    for (int i = 0; i < ZoneDataList.Count; i++)
        //    {
        //        List<float> aResultList = GetAnnualZoneResultList(ZoneDataList[i], ZoneArray);
        //        float aMin = aResultList[Index];
        //        if (aMin < Min)
        //        {
        //            Min = aMin;
        //            ZoneData = ZoneDataList[i];
        //        }
        //    }
        //}

        //internal static void GetMaxLatent(ZoneData ZoneData, out float Max, out int Index)
        //{
        //    Max = float.MinValue;
        //    Index = -1;
        //    List<float> aFloatList_EquipmentLatentGain = GetAnnualZoneResultList(ZoneData, TSD.tsdZoneArray.equipmentLatentGain);
        //    List<float> aFloatList_OccupancyLatentGain = GetAnnualZoneResultList(ZoneData, TSD.tsdZoneArray.occupancyLatentGain);
        //    if (aFloatList_EquipmentLatentGain.Count > 0 && aFloatList_EquipmentLatentGain == aFloatList_OccupancyLatentGain)
        //    {
        //        for (int i = 0; i < aFloatList_EquipmentLatentGain.Count; i++)
        //        {
        //            float aSum = aFloatList_EquipmentLatentGain[i] + aFloatList_OccupancyLatentGain[1];
        //            if (aSum > Max)
        //            {
        //                Max = aSum;
        //                Index = i;
        //            }
        //        }
        //    }
        //}

        //internal static List<float> GetAnnualZoneResultList(ZoneData ZoneData, TSD.tsdZoneArray ZoneArray)
        //{
        //    object aObject = ZoneData.GetAnnualZoneResult(ZoneData, (int)ZoneArray);
        //    return GetList(aObject);
        //}

        //internal static List<float> GetAnnualZoneResultList(List<ZoneData> ZoneDataList, TSD.tsdZoneArray ZoneArray)
        //{
        //    if (ZoneDataList.Count < 1)
        //        return null;

        //    List<float> aResultList = GetAnnualZoneResultList(ZoneDataList.First(), ZoneArray);
        //    for (int i = 0; i < aResultList.Count; i++)
        //        if (aResultList[i] < 0)
        //            aResultList[i] = 0;

        //    for (int i = 1; i < ZoneDataList.Count; i++)
        //    {
        //        List<float> aResultList_Temp = GetAnnualZoneResultList(ZoneDataList[i], ZoneArray);
        //        for (int j = 0; j < aResultList_Temp.Count; j++)
        //            if (aResultList_Temp[j] > 0)
        //                aResultList[j] += aResultList_Temp[j];
        //    }

        //    return aResultList;
        //}

        //internal static List<float> GetAnnualBuildingResultList(BuildingData aBuildingData, TSD.tsdBuildingArray BuildingArray)
        //{
        //    object aObject = BuildingData.AnnualBuildingResult(aBuildingData, (int)BuildingArray);
        //    return GetList(aObject);
        //}

        internal static List<float> GetList(object Object)
        {
            List<float> aResult = new List<float>();
            if (Object != null)
            {
                System.Collections.IEnumerable aValues = Object as System.Collections.IEnumerable;
                if (aValues != null)
                    aResult = aValues.Cast<float>().ToList();
            }
            return aResult;
        }

        //internal static float GetAtIndex(ZoneData ZoneData, int Index, TSD.tsdZoneArray ZoneArray)
        //{
        //    List<float> aFloatList = GetAnnualZoneResultList(ZoneData, ZoneArray);
        //    if (aFloatList.Count > Index)
        //        return aFloatList[Index];
        //    return float.NaN;
        //}

        //internal static float GetAtIndex(List<ZoneData> ZoneDataList, int Index, TSD.tsdZoneArray ZoneArray)
        //{
        //    float aResult = 0;
        //    foreach (ZoneData aZoneData in ZoneDataList)
        //    {
        //        List<float> aResutList = GetAnnualZoneResultList(aZoneData, ZoneArray);
        //        if (aResutList[Index] > 0)
        //            aResult += aResutList[Index];

        //    }
        //    return aResult;
        //}

        //internal static void GetMaxBuildingResults(BuildingData BuildingData, TSD.tsdBuildingArray TSDBuildingArray, out float Max, out int Index)
        //{
        //    List<float> aFloatList = GetAnnualBuildingResultList(BuildingData, TSDBuildingArray);
        //    Max = float.NaN;
        //    Index = -1;
        //    if (aFloatList.Count > 0)
        //    {
        //        Max = aFloatList.Max();
        //        Index = aFloatList.IndexOf(Max);
        //    }
        //}

        //internal static void GetMinBuildingResults(BuildingData BuildingData, TSD.tsdBuildingArray TSDBuildingArray, out float Min, out int Index)
        //{
        //    List<float> aFloatList = GetAnnualBuildingResultList(BuildingData, TSDBuildingArray);
        //    Min = float.NaN;
        //    Index = -1;
        //    if (aFloatList.Count > 0)
        //    {
        //        Min = aFloatList.Min();
        //        Index = aFloatList.IndexOf(Min);
        //    }
        //}

        //internal static void AddValues(List<float> ListFloat, List<float> ListFloatToAdd)
        //{
        //    for (int i = 0; i < ListFloat.Count; i++)
        //        ListFloat[i] += ListFloatToAdd[i];
        //}

        //internal static float GetTotalLatentGain(List<ZoneData> ZoneDataList, int Index, float Volume)
        //{
        //    int aIndex_Temp = Index;
        //    if (aIndex_Temp < 2)
        //        aIndex_Temp++;

        //    float aVal_1 = GetAtIndex(ZoneDataList, aIndex_Temp, TSD.tsdZoneArray.humidityRatio);
        //    float aVal_2 = GetAtIndex(ZoneDataList, aIndex_Temp - 1, TSD.tsdZoneArray.humidityRatio);
        //    float aVal_3 = GetAtIndex(ZoneDataList, aIndex_Temp, TSD.tsdZoneArray.latentLoad);
        //    return (float)(1.2 * Volume * (aVal_1 - aVal_2) / 3600 * 2257 * 1000 - aVal_3);
        //}
    }
}
