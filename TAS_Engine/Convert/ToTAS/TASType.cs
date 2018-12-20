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
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using TBD;
using TSD;
using TAS3D;
using BHEE = BH.Engine.Environment;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Elements;
using System.Collections;

using BH.oM.Environment.Results;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        public static tsdBuildingArray? ToTASBuildingType(this ProfileResultType resultType)
        {
            switch(resultType)
            {
                case ProfileResultType.HumidityExternal:
                    return tsdBuildingArray.externalHumidity;
                case ProfileResultType.TemperatureExternal:
                    return tsdBuildingArray.externalTemperature;
                case ProfileResultType.RadiationGlobal:
                    return tsdBuildingArray.globalRadiation;
                case ProfileResultType.RadiationDiffuse:
                    return tsdBuildingArray.diffuseRadiation;
                case ProfileResultType.WindSpeed:
                    return tsdBuildingArray.windSpeed;

                case ProfileResultType.WindDirection:
                    return tsdBuildingArray.windDirection;
                case ProfileResultType.LoadHeating:
                    return tsdBuildingArray.heatingProfile;
                case ProfileResultType.LoadCooling:
                    return tsdBuildingArray.coolingProfile;
                case ProfileResultType.LoadLatentAddition:
                    return tsdBuildingArray.additionProfile;
                case ProfileResultType.LoadLatentRemoval:
                    return tsdBuildingArray.removalProfile;
                case ProfileResultType.CloudCover:
                    return tsdBuildingArray.cloudCover;

                default:
                    return null;
            }
        }

        public static tsdSurfaceArray? ToTASSurfaceType(this ProfileResultType resultType)
        {
            switch(resultType)
            {
                case ProfileResultType.TemperatureResultant:  //= Internal temperature?
                    return tsdSurfaceArray.intTemp;
                case ProfileResultType.TemperatureExternal:
                    return tsdSurfaceArray.extTemp;
                case ProfileResultType.GainInternalSolar:
                    return tsdSurfaceArray.intSolarGain;
                case ProfileResultType.GainSolar:               //= External Solar Gain?
                    return tsdSurfaceArray.extSolarGain;
                case ProfileResultType.ApertureFlowIn:
                    return tsdSurfaceArray.apertureFlowIn;
                case ProfileResultType.ApertureFlowOut:
                    return tsdSurfaceArray.apertureFlowOut;
                    
                case ProfileResultType.CondensationInternal:
                    return tsdSurfaceArray.intCondensation;
                case ProfileResultType.CondensationExternal:
                    return tsdSurfaceArray.extCondensation;
                case ProfileResultType.ConductionInternal:
                    return tsdSurfaceArray.intConduction;
                case ProfileResultType.ConductionExternal:
                    return tsdSurfaceArray.extConduction;
                case ProfileResultType.ApertureOpening:
                    return tsdSurfaceArray.apertureOpening;

                case ProfileResultType.LongWaveInternal:
                    return tsdSurfaceArray.intLongWave;
                case ProfileResultType.LongWaveExternal:
                    return tsdSurfaceArray.extLongWave;
                case ProfileResultType.ConvectionInternal:
                    return tsdSurfaceArray.intConvection;
                case ProfileResultType.ConvectionExternal:
                    return tsdSurfaceArray.extConvection;
                case ProfileResultType.CondensationInterstitial:
                    return tsdSurfaceArray.interCondensation;
                
                default:
                    return null;
            }
        }

        public static tsdZoneArray? ToTASSpaceType(this ProfileResultType resultType)
        {
            switch(resultType)
            {
                case ProfileResultType.TemperatureDryBulb:
                    return tsdZoneArray.dryBulbTemp;
                case ProfileResultType.TemperatureMeanRadiant:
                    return tsdZoneArray.MRTemp;
                case ProfileResultType.TemperatureResultant:
                    return tsdZoneArray.resultantTemp;
                case ProfileResultType.LoadSensible:
                    return tsdZoneArray.sensibleLoad;
                case ProfileResultType.LoadHeating:
                    return tsdZoneArray.heatingLoad;
                case ProfileResultType.LoadCooling:
                    return tsdZoneArray.coolingLoad;
                case ProfileResultType.GainSolar:
                    return tsdZoneArray.solarGain;
                case ProfileResultType.GainLighting:
                    return tsdZoneArray.lightingGain;
                case ProfileResultType.GainInfiltrationVentilation:
                    return tsdZoneArray.infVentGain;
                case ProfileResultType.GainAirMovement:
                    return tsdZoneArray.airMovementGain;

                case ProfileResultType.HeatTransferBuilding:
                    return tsdZoneArray.buildingHeatTransfer;
                case ProfileResultType.ConductionExternalOpaque:
                    return tsdZoneArray.externalConductionOpaque;
                case ProfileResultType.ConductionExternalGlazing:
                    return tsdZoneArray.externalConductionGlazing;
                case ProfileResultType.GainOccupantSensible:
                    return tsdZoneArray.occupantSensibleGain;
                case ProfileResultType.GainEquipmentSensible:
                    return tsdZoneArray.equipmentSensibleGain;
                case ProfileResultType.HumidityRatio:
                    return tsdZoneArray.humidityRatio;

                case ProfileResultType.HumidityRelative:
                    return tsdZoneArray.relativeHumidity;
                case ProfileResultType.GainOccupancyLatent:
                    return tsdZoneArray.occupancyLatentGain;
                case ProfileResultType.GainEquipmentLatent:
                    return tsdZoneArray.equipmentLatentGain;
                case ProfileResultType.LoadLatent:
                    return tsdZoneArray.latentLoad;
                case ProfileResultType.LoadLatentRemoval:
                    return tsdZoneArray.latentRemovalLoad;
                case ProfileResultType.LoadLatentAddition:
                    return tsdZoneArray.latentAdditionLoad;
                case ProfileResultType.PressureVapour:
                    return tsdZoneArray.vapourPressure;

                case ProfileResultType.Pollutant:
                    return tsdZoneArray.pollutant;
                case ProfileResultType.PressureRelative:
                    return tsdZoneArray.pressure;
                case ProfileResultType.Infiltration:
                    return tsdZoneArray.infiltration;
                case ProfileResultType.Ventilation:
                    return tsdZoneArray.ventilation;
                case ProfileResultType.ApertureFlowIn:          //-For zone ApertureFlowIn?
                    return tsdZoneArray.zoneApertureFlowIn;
                case ProfileResultType.ApertureFlowOut:         //-For zone ApertureFlowOut?
                    return tsdZoneArray.zoneApertureFlowOut;
                case ProfileResultType.IZAMIn:
                    return tsdZoneArray.izamIn;
                case ProfileResultType.IZAMOut:
                    return tsdZoneArray.izamOut;
                    
                default:
                    return null;
            }
        }
    }
}