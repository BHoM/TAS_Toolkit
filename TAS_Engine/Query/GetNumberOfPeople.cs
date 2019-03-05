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

        private static TBD.dayType[] GetDayTypeArray(TBD.TBDDocument TBDDocument)
        {
            TBD.dayType[] aDayTypeArray = new TBD.dayType[365];

            TBD.Calendar aCalendar = TBDDocument.Building.GetCalendar();
            for (int i = 0; i < 365; i++)
                aDayTypeArray[i] = aCalendar.days(i + 1).dayType;

            return aDayTypeArray;
        }

        private static List<TBD.dayType> GetUnique(IEnumerable<TBD.dayType> DayTypes)
        {
            List<TBD.dayType> aDayTypeList = new List<TBD.dayType>();
            foreach (TBD.dayType aDayType in DayTypes)
                if (aDayTypeList.Find(x => x.name == aDayType.name) == null)
                    aDayTypeList.Add(aDayType);
            return aDayTypeList;
        }

        public static List<TBD.zone> GetZones(TBD.TBDDocument TBDDocument)
        {
            if (TBDDocument == null)
                return null;

            List<TBD.zone> aZoneList = new List<TBD.zone>();

            int aIndex = 0;
            TBD.zone aZone = TBDDocument.Building.GetZone(aIndex);
            while (aZone != null)
            {
                aZoneList.Add(aZone);
                aIndex++;
                aZone = TBDDocument.Building.GetZone(aIndex);
            }

            return aZoneList;
        }

        private static bool HasDayType(TBD.InternalCondition InternalCondition, TBD.dayType DayType)
        {
            int aIndex = 0;
            TBD.dayType aDayType = InternalCondition.GetDayType(aIndex);
            while (aDayType != null)
            {
                if (aDayType.name == DayType.name)
                    return true;

                aIndex++;
                aDayType = InternalCondition.GetDayType(aIndex);
            }

            return false;
        }

        private static TBD.InternalCondition GetInternalCondition(TBD.zone Zone, TBD.dayType DayType)
        {
            if (Zone == null)
                return null;


            int aIndex = 0;
            TBD.InternalCondition aInternalCondition = Zone.GetIC(aIndex);
            while (aInternalCondition != null)
            {
                if (HasDayType(aInternalCondition, DayType))
                    return aInternalCondition;

                aIndex++;
                aInternalCondition = Zone.GetIC(aIndex);
            }

            return null; ;

        }

        public static double[] GetHourlyValues(TBD.profile Profile)
        {
            double[] aResult = new double[24];

            for (int i = 1; i <= 24; i++)
                aResult[i - 1] = Profile.hourlyValues[i];

            return aResult;
        }

        public static double[] MultiplyByFactor(double[] x, double factor)
        {
            if (x == null) throw new ArgumentNullException();
            return x.Select(r => r * factor).ToArray();
        }

        /***************************************************/

        public static double[] GetNumberOfPeople(TBD.TBDDocument TBDDocument, TBD.zone tbdZone)
        {

            if (TBDDocument == null)
                return null;

            TBD.dayType[] aDayTypeArray = GetDayTypeArray(TBDDocument);
            List<TBD.dayType> aDayTypeList_Unique = GetUnique(aDayTypeArray);

            double[] aHourlyValues = null;
            double[] aHourlyValuesLatent = null;
            double aTotalPersonGain = 0;

            Dictionary<string, double[]> aDictionary_DayType = new Dictionary<string, double[]>();
            Dictionary<string, double[]> aDictionary_DayTypeLatent = new Dictionary<string, double[]>();
            foreach (TBD.dayType aDayType in aDayTypeList_Unique)
            {
                TBD.InternalCondition aInternalCondition = GetInternalCondition(tbdZone, aDayType);
                if (aInternalCondition == null)
                    continue;

                TBD.IInternalGain aInternalGain = aInternalCondition.GetInternalGain();
                aTotalPersonGain = aInternalGain.personGain;
                TBD.profile aProfile = aInternalGain.GetProfile((int)TBD.Profiles.ticOSG);
                TBD.profile aProfileLatent = aInternalGain.GetProfile((int)TBD.Profiles.ticOLG);

                //check if profile hourly or yearly if yearly take it and skip next step
                aHourlyValues = GetHourlyValues(aProfile);
                aHourlyValuesLatent = GetHourlyValues(aProfileLatent);

                //double[] result = x.Select(r => r * factor).ToArray();
                aHourlyValues = MultiplyByFactor(aHourlyValues, aProfile.factor);
                aHourlyValuesLatent = MultiplyByFactor(aHourlyValuesLatent, aProfile.factor);
                aDictionary_DayType.Add(aDayType.name, aHourlyValues);
                aDictionary_DayTypeLatent.Add(aDayType.name, aHourlyValuesLatent);

            }

            bool aExists = true;

            double[] aYearlyValues = new double[8760];
            int aCount = 0;
            for (int i = 0; i < 365; i++)
            {
                TBD.dayType aDayType = aDayTypeArray[i];

                if (!aDictionary_DayType.ContainsKey(aDayType.name))
                {
                    aExists = false;
                    break;
                }

                aHourlyValues = aDictionary_DayType[aDayType.name];
                for (int j = 0; j < 24; j++)
                {
                    aYearlyValues[aCount] = aHourlyValues[j];
                    aCount++;
                }

            }

            double[] aYearlyValuesLatent = new double[8760];
            aCount = 0;
            for (int i = 0; i < 365; i++)
            {
                TBD.dayType aDayType = aDayTypeArray[i];

                if (!aDictionary_DayType.ContainsKey(aDayType.name))
                {
                    aExists = false;
                    break;
                }

                aHourlyValuesLatent = aDictionary_DayTypeLatent[aDayType.name];
                for (int j = 0; j < 24; j++)
                {
                    aYearlyValuesLatent[aCount] = aHourlyValuesLatent[j];
                    aCount++;
                }

            }

            double[] aNumberOfPeople = new double[7];
            //if (aExists)
            //{
            //    aYearlyValues;
            //}

            double aMaxSpecificSensibleGain = aYearlyValues.Max();//Unit W/m2 sensible gain
            aNumberOfPeople[0] = aMaxSpecificSensibleGain;
            double aMaxSpecificLatentGain = aYearlyValuesLatent.Max();//Unit W/m2 latent gain
            aNumberOfPeople[1] = aMaxSpecificLatentGain;
            double aPeopleDesity = (aMaxSpecificLatentGain + aMaxSpecificSensibleGain) / aTotalPersonGain; //Unit people/m2
            aNumberOfPeople[2] = aPeopleDesity;
            double aPeople = tbdZone.floorArea * aPeopleDesity;
            aNumberOfPeople[3] = aPeople;
            double aPersonSensibleGain = aMaxSpecificSensibleGain / aPeopleDesity; //sensible gain per person
            aNumberOfPeople[4] = aPersonSensibleGain;
            double aPersonLatenteGain = aMaxSpecificLatentGain / aPeopleDesity; //sensible gain per person
            aNumberOfPeople[5] = aPersonLatenteGain;
            double aMetabolicRateCheck = aPersonSensibleGain + aPersonLatenteGain;
            aNumberOfPeople[6] = aMetabolicRateCheck;

            return aYearlyValues;

        }

        /***************************************************/
    }




}


