/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BHR = BH.oM.Environment.Results;
using BHG = BH.oM.Geometry;
using BHER = BH.Engine.Reflection.Compute;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.Adapters.TAS.Convert ToTAS => gets a TAS TSD Building Array from a BHoM Environmental Profile Result Type")]
        [Input("resultType", "BHoM Environmental Profile Result Type")]
        [Output("TAS TSB Building Array")]
        public static TSD.tsdBuildingArray? ToTASBuildingType(this BHR.ProfileResultType resultType)
        {
            switch (resultType)
            {
                case BHR.ProfileResultType.HumidityExternal:
                    return TSD.tsdBuildingArray.externalHumidity;
                case BHR.ProfileResultType.TemperatureExternal:
                    return TSD.tsdBuildingArray.externalTemperature;
                case BHR.ProfileResultType.RadiationGlobal:
                    return TSD.tsdBuildingArray.globalRadiation;
                case BHR.ProfileResultType.RadiationDiffuse:
                    return TSD.tsdBuildingArray.diffuseRadiation;
                case BHR.ProfileResultType.WindSpeed:
                    return TSD.tsdBuildingArray.windSpeed;

                case BHR.ProfileResultType.WindDirection:
                    return TSD.tsdBuildingArray.windDirection;
                case BHR.ProfileResultType.LoadHeating:
                    return TSD.tsdBuildingArray.heatingProfile;
                case BHR.ProfileResultType.LoadCooling:
                    return TSD.tsdBuildingArray.coolingProfile;
                case BHR.ProfileResultType.LoadLatentAddition:
                    return TSD.tsdBuildingArray.additionProfile;
                case BHR.ProfileResultType.LoadLatentRemoval:
                    return TSD.tsdBuildingArray.removalProfile;
                case BHR.ProfileResultType.CloudCover:
                    return TSD.tsdBuildingArray.cloudCover;

                default:
                    return null;
            }
        }

        [Description("BH.Engine.Adapters.TAS.Convert ToTAS => gets a TAS TSD Surface Array from a BHoM Environmental Profile Result Type")]
        [Input("resultType", "BHoM Environmental Profile Result Type")]
        [Output("TAS TSB Surface Array")]
        public static TSD.tsdSurfaceArray? ToTASSurfaceType(this BHR.ProfileResultType resultType)
        {
            switch (resultType)
            {
                case BHR.ProfileResultType.TemperatureResultant:  //= Internal temperature?
                    return TSD.tsdSurfaceArray.intTemp;
                case BHR.ProfileResultType.TemperatureExternal:
                    return TSD.tsdSurfaceArray.extTemp;
                case BHR.ProfileResultType.GainInternalSolar:
                    return TSD.tsdSurfaceArray.intSolarGain;
                case BHR.ProfileResultType.GainSolar:               //= External Solar Gain?
                    return TSD.tsdSurfaceArray.extSolarGain;
                case BHR.ProfileResultType.ApertureFlowIn:
                    return TSD.tsdSurfaceArray.apertureFlowIn;
                case BHR.ProfileResultType.ApertureFlowOut:
                    return TSD.tsdSurfaceArray.apertureFlowOut;

                case BHR.ProfileResultType.CondensationInternal:
                    return TSD.tsdSurfaceArray.intCondensation;
                case BHR.ProfileResultType.CondensationExternal:
                    return TSD.tsdSurfaceArray.extCondensation;
                case BHR.ProfileResultType.ConductionInternal:
                    return TSD.tsdSurfaceArray.intConduction;
                case BHR.ProfileResultType.ConductionExternal:
                    return TSD.tsdSurfaceArray.extConduction;
                case BHR.ProfileResultType.ApertureOpening:
                    return TSD.tsdSurfaceArray.apertureOpening;

                case BHR.ProfileResultType.LongWaveInternal:
                    return TSD.tsdSurfaceArray.intLongWave;
                case BHR.ProfileResultType.LongWaveExternal:
                    return TSD.tsdSurfaceArray.extLongWave;
                case BHR.ProfileResultType.ConvectionInternal:
                    return TSD.tsdSurfaceArray.intConvection;
                case BHR.ProfileResultType.ConvectionExternal:
                    return TSD.tsdSurfaceArray.extConvection;
                case BHR.ProfileResultType.CondensationInterstitial:
                    return TSD.tsdSurfaceArray.interCondensation;

                default:
                    return null;
            }
        }

        [Description("BH.Engine.Adapters.TAS.Convert ToTAS => gets a TAS TSD Zone Array from a BHoM Environmental Profile Result Type")]
        [Input("resultType", "BHoM Environmental Profile Result Type")]
        [Output("TAS TSB Zone Array")]
        public static TSD.tsdZoneArray? ToTASSpaceType(this BHR.ProfileResultType resultType)
        {
            switch (resultType)
            {
                case BHR.ProfileResultType.TemperatureDryBulb:
                    return TSD.tsdZoneArray.dryBulbTemp;
                case BHR.ProfileResultType.TemperatureMeanRadiant:
                    return TSD.tsdZoneArray.MRTemp;
                case BHR.ProfileResultType.TemperatureResultant:
                    return TSD.tsdZoneArray.resultantTemp;
                case BHR.ProfileResultType.LoadSensible:
                    return TSD.tsdZoneArray.sensibleLoad;
                case BHR.ProfileResultType.LoadHeating:
                    return TSD.tsdZoneArray.heatingLoad;
                case BHR.ProfileResultType.LoadCooling:
                    return TSD.tsdZoneArray.coolingLoad;
                case BHR.ProfileResultType.GainSolar:
                    return TSD.tsdZoneArray.solarGain;
                case BHR.ProfileResultType.GainLighting:
                    return TSD.tsdZoneArray.lightingGain;
                case BHR.ProfileResultType.GainInfiltrationVentilation:
                    return TSD.tsdZoneArray.infVentGain;
                case BHR.ProfileResultType.GainAirMovement:
                    return TSD.tsdZoneArray.airMovementGain;

                case BHR.ProfileResultType.HeatTransferBuilding:
                    return TSD.tsdZoneArray.buildingHeatTransfer;
                case BHR.ProfileResultType.ConductionExternalOpaque:
                    return TSD.tsdZoneArray.externalConductionOpaque;
                case BHR.ProfileResultType.ConductionExternalGlazing:
                    return TSD.tsdZoneArray.externalConductionGlazing;
                case BHR.ProfileResultType.GainOccupantSensible:
                    return TSD.tsdZoneArray.occupantSensibleGain;
                case BHR.ProfileResultType.GainEquipmentSensible:
                    return TSD.tsdZoneArray.equipmentSensibleGain;
                case BHR.ProfileResultType.HumidityRatio:
                    return TSD.tsdZoneArray.humidityRatio;

                case BHR.ProfileResultType.HumidityRelative:
                    return TSD.tsdZoneArray.relativeHumidity;
                case BHR.ProfileResultType.GainOccupancyLatent:
                    return TSD.tsdZoneArray.occupancyLatentGain;
                case BHR.ProfileResultType.GainEquipmentLatent:
                    return TSD.tsdZoneArray.equipmentLatentGain;
                case BHR.ProfileResultType.LoadLatent:
                    return TSD.tsdZoneArray.latentLoad;
                case BHR.ProfileResultType.LoadLatentRemoval:
                    return TSD.tsdZoneArray.latentRemovalLoad;
                case BHR.ProfileResultType.LoadLatentAddition:
                    return TSD.tsdZoneArray.latentAdditionLoad;
                case BHR.ProfileResultType.PressureVapour:
                    return TSD.tsdZoneArray.vapourPressure;

                case BHR.ProfileResultType.Pollutant:
                    return TSD.tsdZoneArray.pollutant;
                case BHR.ProfileResultType.PressureRelative:
                    return TSD.tsdZoneArray.pressure;
                case BHR.ProfileResultType.Infiltration:
                    return TSD.tsdZoneArray.infiltration;
                case BHR.ProfileResultType.Ventilation:
                    return TSD.tsdZoneArray.ventilation;
                case BHR.ProfileResultType.ApertureFlowIn:          //-For zone ApertureFlowIn?
                    return TSD.tsdZoneArray.zoneApertureFlowIn;
                case BHR.ProfileResultType.ApertureFlowOut:         //-For zone ApertureFlowOut?
                    return TSD.tsdZoneArray.zoneApertureFlowOut;
                case BHR.ProfileResultType.IZAMIn:
                    return TSD.tsdZoneArray.izamIn;
                case BHR.ProfileResultType.IZAMOut:
                    return TSD.tsdZoneArray.izamOut;

                default:
                    return null;
            }
        }
    }
}


