using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using TBD;
using TAS3D;
using BHEE = BH.Engine.Environment;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Elements;
using System.Collections;
using BH.Engine.TAS;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/

        public static BHE.Elements.Building ToBHoM(this TBD.Building tbdBuilding)
        {

            // by MD 2018-05-21 get BuildingElementsProperties  - in BHoM Building element it contrail geomety and building element property
            // in TAS BuildingElement is just an object with BuildingElement propoerties so please avoid confusion
            List<BHE.Properties.BuildingElementProperties> buildingElementPropertiesList = new List<BHE.Properties.BuildingElementProperties>();

            int buildingElementIndex = 0;
            TBD.buildingElement aBuildingElement = null;
            while ((aBuildingElement = tbdBuilding.GetBuildingElement(buildingElementIndex)) != null)
            {
                //buildingElementPropertiesList.Add(ToBHoM(aBuildingElement));
                buildingElementIndex++;
            }

            // get Spaces from TAS TBD in TAS TBD Spaces index start from 0 
            List<BHE.Elements.Space> spaceList = new List<BHE.Elements.Space>();

            int spaceIndex = 0;
            TBD.zone aSpace = null;
            while ((aSpace = tbdBuilding.GetZone(spaceIndex)) != null)
            {
                spaceList.Add(ToBHoM(aSpace));
                spaceIndex++;
            }

            // here we outputing Building data 
            BHE.Elements.Building bHoMBuilding = new BHE.Elements.Building
            {
                Name = tbdBuilding.name,
                Latitude = tbdBuilding.latitude,
                Longitude = tbdBuilding.longitude,
                Elevation = tbdBuilding.maxBuildingAltitude,
                //tbdBuilding.peakCooling
                //BuildingElementProperties = buildingElementPropertiesList,
                //Spaces = spaceList,


                //TODO: location, equipment, spaces, storeys, profiles, IC, EquipmentProperties
            };

            string tbdBuildingGUID = tbdBuilding.GUID;
            bHoMBuilding.CustomData.Add("BuildingGUID", tbdBuildingGUID);

            string tbdBuildingDescription = tbdBuilding.description;
            bHoMBuilding.CustomData.Add("BuildingDescription", tbdBuildingDescription);

            string tbdBuildingName = tbdBuilding.name;
            bHoMBuilding.CustomData.Add("BuildingName", tbdBuildingName);

            double tbdBuildingNorthAngle = tbdBuilding.northAngle;
            bHoMBuilding.CustomData.Add("BuildingNorthAngle", tbdBuildingNorthAngle);

            string tbdBuildingPath3DFile = tbdBuilding.path3DFile;
            bHoMBuilding.CustomData.Add("BuildingPath3DFile", tbdBuildingPath3DFile);

            double tbdBuildingPeakCooling = tbdBuilding.peakCooling;
            bHoMBuilding.CustomData.Add("BuildingPeakCooling", tbdBuildingPeakCooling);

            double tbdBuildingPeakHeating = tbdBuilding.peakHeating;
            bHoMBuilding.CustomData.Add("BuildingPeakHeating", tbdBuildingPeakHeating);

            string tbdBuildingTBDGUID = tbdBuilding.TBDGUID;
            bHoMBuilding.CustomData.Add("BuildingTBDGUID", tbdBuildingTBDGUID);

            double tbdBuildingTimeZone = tbdBuilding.timeZone;
            bHoMBuilding.CustomData.Add("BuildingTimeZone", tbdBuildingTimeZone);

            double tbdBuildingYear = tbdBuilding.year;
            bHoMBuilding.CustomData.Add("BuildingYear", tbdBuildingYear);

            return bHoMBuilding;
        }

        /***************************************************/

        public static BHE.Elements.Space ToBHoM(this TBD.zone tbdZone)
        {
            BHE.Elements.Space bHoMSpace = new BHE.Elements.Space();

            //Space Data
            bHoMSpace.Number = tbdZone.number.ToString();
            bHoMSpace.Name = tbdZone.name;
            bHoMSpace.CoolingLoad = tbdZone.maxCoolingLoad;
            bHoMSpace.HeatingLoad = tbdZone.maxHeatingLoad;

            //all not supported information in BHoM Space are added to Space_Custom Data

            System.Drawing.Color spaceRGB = Query.GetRGB(tbdZone.colour);
            bHoMSpace.CustomData.Add("Colour", spaceRGB);

            double tbdDaylightFactor = tbdZone.daylightFactor;
            bHoMSpace.CustomData.Add("tbdDaylightFactor", tbdDaylightFactor);

            string tbdDescription = tbdZone.description;
            bHoMSpace.CustomData.Add("tbdDescription", tbdDescription);

            //This is the exposed length of the perimeter of the Zone at a height of 0m. If the Zone is on the ground floor and the ground 
            //construction has an F-factor this length is used to calculate the additional heat loss
            double tbdExposedPerimeter = tbdZone.exposedPerimeter;
            bHoMSpace.CustomData.Add("tbdExposedPerimeter", tbdExposedPerimeter);

            double tbdExternal = tbdZone.external; //defines if space is external
            bHoMSpace.CustomData.Add("tbdExternal", tbdExternal);

            //This value is the exposed length of the façade at a height of 1.1m and is defined in the NCM document. 
            //It is used for the Criterion 3 Solar gain check to calculate the limiting solar value
            double tbdFacadeLength = tbdZone.facadeLength;
            bHoMSpace.CustomData.Add("tbdFacadeLength", tbdFacadeLength);

            double tbdFixedConvectionCoefficient = tbdZone.fixedConvectionCoefficient; //external spaces have special ConvectionCoefficient
            bHoMSpace.CustomData.Add("tbdFixedConvectionCoefficient", tbdFixedConvectionCoefficient);

            double tbdFloorArea = tbdZone.floorArea;
            bHoMSpace.CustomData.Add("tbdFloorArea", tbdFloorArea);

            string tbdGUID = tbdZone.GUID;
            bHoMSpace.CustomData.Add("tbdGUID", tbdGUID);

            //This is the maximum length between two edges in the Zone. This is an internal value and is used internally for checking geometry
            double tbdLength = tbdZone.length;
            bHoMSpace.CustomData.Add("tbdLength", tbdLength);

            double tbdNumber = tbdZone.number;
            bHoMSpace.CustomData.Add("tbdNumber", tbdNumber);

            double tbdSizeCooling = tbdZone.sizeCooling;
            bHoMSpace.CustomData.Add("tbdSizeCooling", tbdSizeCooling);

            double tbdSizeHeating = tbdZone.sizeHeating;
            bHoMSpace.CustomData.Add("tbdSizeHeating", tbdSizeHeating);

            double tbdVolume = tbdZone.volume;
            bHoMSpace.CustomData.Add("tbdVolume", tbdVolume);

            double tbdWallFloorAreaRatio = tbdZone.wallFloorAreaRatio;
            bHoMSpace.CustomData.Add("tbdWallFloorAreaRatio", tbdWallFloorAreaRatio);

            int internalConditionIndex = 0;

            TBD.InternalCondition tbdInternalCondition = null;

            while ((tbdInternalCondition = tbdZone.GetIC(internalConditionIndex)) != null)
            {
                bHoMSpace.InternalConditions.Add(ToBHoM(tbdInternalCondition));
                internalConditionIndex++;
            }

            ////Geometry
            //int tbdZoneSurfaceIndex = 0;
            //minElevation = double.MaxValue;
            //zoneSurface tbdZoneSurface = null;
            //while ((tbdZoneSurface = tbdZone.GetSurface(tbdZoneSurfaceIndex)) != null)
            //{
            //    int tbdRoomSurfaceIndex = 0;
            //    RoomSurface tbdRoomSurface = null;
            //    while ((tbdRoomSurface = tbdZoneSurface.GetRoomSurface(tbdRoomSurfaceIndex)) != null)
            //    {
            //        if (tbdRoomSurface.GetPerimeter() != null)
            //        {
            //            BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties = ToBHoM(tbdZoneSurface.buildingElement);
            //            BHE.Elements.BuildingElement bHoMBuildingElement = new BuildingElement()
            //            {
            //                Name = bHoMBuildingElementProperties.Name,
            //                //BuildingElementGeometry = tasRoomSrf.ToBHoM(),
            //                BuildingElementProperties = bHoMBuildingElementProperties
            //            };

            //            minElevation = Math.Min(minElevation, BH.Engine.TAS.Query.MinElevation(tbdRoomSurface.GetPerimeter()));
            //            //bHoMSpace.BuildingElements.Add(bHoMBuildingElement);
            //        }
            //        tbdRoomSurfaceIndex++;
            //    }
            //    tbdZoneSurfaceIndex++;
            //}

            //Space Custom Data


            return bHoMSpace;
        }

        // we do not need aMin Elecation
        //public static BHE.Elements.Space ToBHoM(this TBD.zone tasZone)
        //{
        //    double aMinElevation;
        //    return ToBHoM(tasZone, out aMinElevation);
        //}

        /***************************************************/

        //public static BuildingElement ToBHoM(this TBD.buildingElement tbdBuildingElement, TBD.RoomSurface tbdRoomSurface)
        //{
        //    BuildingElement bHoMBuildingElement = new BHE.Elements.BuildingElement();

        //    zoneSurface tbdZoneSurface = null;
        //    tbdZoneSurface = tbdRoomSurface.zoneSurface;

        //    bHoMBuildingElement.Name = "Z" + tbdZoneSurface.zone.number + "_" + tbdZoneSurface.number + " _" + tbdZoneSurface.zone.name;

        //    string tbdZoneSurfaceGUID = tbdZoneSurface.GUID;
        //    bHoMBuildingElement.CustomData.Add("SurfaceGUID", tbdZoneSurfaceGUID);

        //    TBD.SurfaceType tbdZoneSurfaceType = tbdZoneSurface.type;
        //    bHoMBuildingElement.CustomData.Add("SurfaceType", tbdZoneSurfaceType);

        //    double tbdZoneSurfaceAltitude = tbdZoneSurface.altitude;
        //    bHoMBuildingElement.CustomData.Add("SurfaceAltitude", tbdZoneSurfaceAltitude);

        //    double tbdZoneSurfaceAltitudeRange = tbdZoneSurface.altitudeRange;
        //    bHoMBuildingElement.CustomData.Add("SurfaceAltitudeRange", tbdZoneSurfaceAltitudeRange);

        //    double tbdZoneSurfaceArea = tbdZoneSurface.area;
        //    bHoMBuildingElement.CustomData.Add("SurfaceArea", tbdZoneSurfaceArea);

        //    double tbdZoneSurfaceInclination = tbdZoneSurface.inclination;
        //    bHoMBuildingElement.CustomData.Add("SurfaceInclination", tbdZoneSurfaceInclination);

        //    double tbdZoneSurfaceInternalArea = tbdZoneSurface.internalArea;
        //    bHoMBuildingElement.CustomData.Add("SurfaceInternalArea", tbdZoneSurfaceInternalArea);

        //    double tbdZoneSurfaceOrientation = tbdZoneSurface.orientation;
        //    bHoMBuildingElement.CustomData.Add("SurfaceOrientation", tbdZoneSurfaceOrientation);

        //    double tbdZoneSurfaceReversed = tbdZoneSurface.reversed;
        //    bHoMBuildingElement.CustomData.Add("SurfaceReversed", tbdZoneSurfaceReversed);

        //    //TO DO: 2018-11-23 how to get name form zone here
        //    string tbdZoneSurfaceZoneName = tbdZoneSurface.zone.name;
        //    bHoMBuildingElement.CustomData.Add("SurfaceZone", tbdZoneSurfaceZoneName);

        //    //Get Geometry from Building Element
        //    List<BH.oM.Geometry.ICurve> panelCurves = new List<BH.oM.Geometry.ICurve>();

        //    TBD.Perimeter tbdPerimeter = tbdRoomSurface.GetPerimeter();
        //    TBD.Polygon tbdPolygon = tbdPerimeter.GetFace();

        //    BHG.ICurve curve = ToBHoM(tbdPolygon);
        //    BHG.PolyCurve polyCurve = Geometry.Create.PolyCurve(new List<BHG.ICurve> { curve });

        //    panelCurves.Add(polyCurve);

        //    bHoMBuildingElement.PanelCurve = BH.Engine.Geometry.Create.PolyCurve(panelCurves);

        //    ////Get Openings from Building Element
        //    List<BH.oM.Geometry.ICurve> openingCurves = new List<BH.oM.Geometry.ICurve>();

        //    int tbdOpeningPolygonIndex = 0;
        //    TBD.Polygon tbdOpeningPolygon = null;
        //    while ((tbdOpeningPolygon = tbdPerimeter.GetHole(tbdOpeningPolygonIndex)) != null)
        //    {
        //        //BHG.ICurve openingCurve = ToBHoM(tbdOpeningPolygon);
        //        bHoMBuildingElement.Openings.Add(ToBHoMOpening(tbdOpeningPolygon));
        //        tbdOpeningPolygonIndex++;
        //    }

        //    //BHG.PolyCurve openingPolyCurve = Geometry.Create.PolyCurve(new List<BHG.ICurve> { curve });
        //    //openingCurves.Add(openingPolyCurve);

        //    //bHoMBuildingElement.Openings[0].OpeningCurve = BH.Engine.Geometry.Create.PolyCurve(openingCurves);
        //    ///temp off to check noneopening issue



        //    //bHoMBuildingElement.Construction = tbdBuildingElement.construction;

        //    //Geometry
        //    ////int tbdZoneSurfaceIndex = 0;
        //    ////double minElevation = double.MaxValue;
        //    ////zoneSurface tbdZoneSurface = null;
        //    ////while ((tbdZoneSurface = tbdZone.GetSurface(tbdZoneSurfaceIndex)) != null)
        //    ////{
        //    ////    int tbdRoomSurfaceIndex = 0;
        //    ////    RoomSurface tbdRoomSurface = null;
        //    ////    while ((tbdRoomSurface = tbdZoneSurface.GetRoomSurface(tbdRoomSurfaceIndex)) != null)
        //    ////    {
        //    ////        if (tbdRoomSurface.GetPerimeter() != null)
        //    ////        {
        //    ////            BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties = ToBHoM(tbdZoneSurface.buildingElement);
        //    ////            BHE.Elements.BuildingElement bHoMBuildingElement = new BuildingElement()
        //    ////            {
        //    ////                Name = bHoMBuildingElementProperties.Name,
        //    ////                //BuildingElementGeometry = tasRoomSrf.ToBHoM(),
        //    ////                BuildingElementProperties = bHoMBuildingElementProperties
        //    ////            };

        //    ////            minElevation = Math.Min(minElevation, BH.Engine.TAS.Query.MinElevation(tbdRoomSurface.GetPerimeter()));
        //    ////            //bHoMSpace.BuildingElements.Add(bHoMBuildingElement);
        //    ////        }
        //    ////        tbdRoomSurfaceIndex++;
        //    ////    }
        //    ////    tbdZoneSurfaceIndex++;
        //    ////}

        //    /*TBD.zone tbdZone = new TBD.zone();
        //    zoneSurface tbdZoneSurface = null;
        //    int tbdZoneSurfaceIndex = 0;
        //    while ((tbdZoneSurface = tbdZone.GetSurface(tbdZoneSurfaceIndex)) != null)
        //    {
        //        int tbdRoomSurfaceIndex = 0;
        //        RoomSurface tbdRoomSurface = null;
        //        while ((tbdRoomSurface = tbdZoneSurface.GetRoomSurface(tbdRoomSurfaceIndex)) != null)
        //        {
        //            if (tbdRoomSurface.GetPerimeter() != null)
        //            {
        //                BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties = BH.Engine.TAS.Convert.ToBHoM(tbdZoneSurface.buildingElement);
        //                {
        //                    bHoMBuildingElement.PanelCurve=tbdBuildingElement
                                
        //                    //BuildingElementGeometry = tasRoomSrf.ToBHoM(),
        //                    BuildingElementProperties = bHoMBuildingElementProperties
        //                };
        //            }
        //            tbdRoomSurfaceIndex++;
        //        }
        //        tbdZoneSurfaceIndex++;
        //    }*/

        //    return bHoMBuildingElement;
        //}

        public static BuildingElement ToBHoM(this TBD.buildingElement tbdBuildingElement, TBD.zoneSurface tbdZoneSurface)
        {
            BuildingElement bHoMBuildingElement = new BHE.Elements.BuildingElement();

            //zoneSurface tbdZoneSurface = null;
            //tbdZoneSurface = tbdRoomSurface.zoneSurface;

            bHoMBuildingElement.Name = "Z" + tbdZoneSurface.zone.number + "_" + tbdZoneSurface.number + " _" + tbdZoneSurface.zone.name;

            string tbdZoneSurfaceGUID = tbdZoneSurface.GUID;
            bHoMBuildingElement.CustomData.Add("SurfaceGUID", tbdZoneSurfaceGUID);

            TBD.SurfaceType tbdZoneSurfaceType = tbdZoneSurface.type;
            bHoMBuildingElement.CustomData.Add("SurfaceType", tbdZoneSurfaceType);

            double tbdZoneSurfaceAltitude = tbdZoneSurface.altitude;
            bHoMBuildingElement.CustomData.Add("SurfaceAltitude", tbdZoneSurfaceAltitude);

            double tbdZoneSurfaceAltitudeRange = tbdZoneSurface.altitudeRange;
            bHoMBuildingElement.CustomData.Add("SurfaceAltitudeRange", tbdZoneSurfaceAltitudeRange);

            double tbdZoneSurfaceArea = tbdZoneSurface.area;
            bHoMBuildingElement.CustomData.Add("SurfaceArea", tbdZoneSurfaceArea);

            double tbdZoneSurfaceInclination = tbdZoneSurface.inclination;
            bHoMBuildingElement.CustomData.Add("SurfaceInclination", tbdZoneSurfaceInclination);

            double tbdZoneSurfaceInternalArea = tbdZoneSurface.internalArea;
            bHoMBuildingElement.CustomData.Add("SurfaceInternalArea", tbdZoneSurfaceInternalArea);

            double tbdZoneSurfaceOrientation = tbdZoneSurface.orientation;
            bHoMBuildingElement.CustomData.Add("SurfaceOrientation", tbdZoneSurfaceOrientation);

            double tbdZoneSurfaceReversed = tbdZoneSurface.reversed;
            bHoMBuildingElement.CustomData.Add("SurfaceReversed", tbdZoneSurfaceReversed);

            //TO DO: 2018-11-23 how to get name form zone here
            string tbdZoneSurfaceZoneName = tbdZoneSurface.zone.name;
            bHoMBuildingElement.CustomData.Add("SurfaceZone", tbdZoneSurfaceZoneName);

            //add Building Element Properties
            TBD.Construction tbdConstruction = tbdBuildingElement.GetConstruction();
            BH.oM.Environment.Elements.BuildingElementType bHoMBuildingElementType = ToBHoM((TBD.BuildingElementType)tbdBuildingElement.BEType);
            bHoMBuildingElement.BuildingElementProperties = ToBHoM(tbdConstruction, tbdBuildingElement.name, bHoMBuildingElementType, tbdBuildingElement);

            //Get Geometry from Building Element
            List<BH.oM.Geometry.ICurve> panelCurves = new List<BH.oM.Geometry.ICurve>();

            int roomSrfIndex = 0;
            TBD.RoomSurface tbdRoomSurface = null;

            while ((tbdRoomSurface = tbdZoneSurface.GetRoomSurface(roomSrfIndex)) != null)
            {
                if (tbdRoomSurface.GetPerimeter() != null)
                {
                    //Sometimes we can have a srf object in TAS without a geometry
                    // buildingElements.Add(zoneSrf.buildingElement.ToBHoM(tbdRoomSurface));
                    //TBD.RoomSurface tbdRoomSurface = null;
                    //tbdRoomSurface = tbdRoomSurface.zoneSurface;

                    TBD.Perimeter tbdPerimeter = tbdRoomSurface.GetPerimeter();
                    TBD.Polygon tbdPolygon = tbdPerimeter.GetFace();

                    BHG.ICurve curve = ToBHoM(tbdPolygon);
                    BHG.PolyCurve polyCurve = Geometry.Create.PolyCurve(new List<BHG.ICurve> { curve });

                    panelCurves.Add(polyCurve);

                    bHoMBuildingElement.PanelCurve = BH.Engine.Geometry.Create.PolyCurve(panelCurves);

                    ////Get Openings from Building Element
                    List<BH.oM.Geometry.ICurve> openingCurves = new List<BH.oM.Geometry.ICurve>();

                    int tbdOpeningPolygonIndex = 0;
                    TBD.Polygon tbdOpeningPolygon = null;
                    while ((tbdOpeningPolygon = tbdPerimeter.GetHole(tbdOpeningPolygonIndex)) != null)
                    {
                        //BHG.ICurve openingCurve = ToBHoM(tbdOpeningPolygon);
                        bHoMBuildingElement.Openings.Add(ToBHoMOpening(tbdOpeningPolygon));
                        tbdOpeningPolygonIndex++;
                    }
                }
                roomSrfIndex++;
            }




            return bHoMBuildingElement;
        }

        /***************************************************/

        /*public static BuildingElementProperties ToBHoM(this TBD.buildingElement tbdBuildingElement)
{
//  by MD 2018-05-21 IN TAS Building Element is type with property and does not have geometry. 
// IN BHoM Building element  is instance including geometry and property
// BuildingProperty is Type with all data for this type
    if (tbdBuildingElement == null)
        return null;

    BuildingElementProperties bHoMBuildingElementProperties = null;

    BHE.Elements.BuildingElementType aBuildingElementType = ToBHoM((TBD.BuildingElementType)tbdBuildingElement.BEType);
    string aName = tbdBuildingElement.name;

    Construction tbdConstruction = tbdBuildingElement.GetConstruction();
    if (tbdConstruction != null)
        bHoMBuildingElementProperties = tbdConstruction.ToBHoM(aName, aBuildingElementType);

    if (bHoMBuildingElementProperties == null)
        bHoMBuildingElementProperties = new BuildingElementProperties();

    //bHoMBuildingElementProperties.BuildingElementType = aBuildingElementType;
    bHoMBuildingElementProperties.Name = aName;
    //BuildingElementProperties do not handle Thickness.
    //bHoMBuildingElementProperties.Thickness = tasBuildingElement.width;

    //BuildingElement Custom Data
    System.Drawing.Color buildingElementRGB = Query.GetRGB(tbdBuildingElement.colour);
    bHoMBuildingElementProperties.CustomData.Add("Colour", buildingElementRGB);

    return bHoMBuildingElementProperties;

}*/

        /***************************************************/

        public static BHE.Elements.Opening ToBHoMOpening(this TBD.Polygon tbdOpeningPolygon)
        {

            BHE.Elements.Opening opening = BH.Engine.Environment.Create.Opening(tbdOpeningPolygon.ToBHoM());

            return opening;

        }

        /***************************************************/

        public static BuildingElementProperties ToBHoM(this TBD.Construction tbdConstruction, string name, BHE.Elements.BuildingElementType buildingElementType, TBD.buildingElement tbdBuildingElement) //double thickness = 0, bool Internal = true, BHE.Elements.BuildingElementType buildingElementType = BHE.Elements.BuildingElementType.Wall)
        {
            //  by MD 2018-05-21 - there 6 values in TBDTas for Uvalue, we have BuildingElement BE that have construction and then material layers
            // form BE we can get geometrical thickness that is used for Volume calculations, in tas there are 6 Uvalues:
            //0.Uexternalhorizontal 1.Uexternalupwards  2.Uexternaldownwards
            //3.Uinternalhorizontal 4.Uinternaupwards  5.Uinternadownwards
            //Here we use aUvalue as this is special case where we want to see caputal U to be clear

            // we pulling BEProperties
            BuildingElementProperties bhomBuildingElementProperties = new BHE.Properties.BuildingElementProperties()
            {

                BuildingElementType = buildingElementType,
                Name = tbdBuildingElement.name,
            };

            BHE.Elements.BuildingElement bHoMBuildingElement = new BHE.Elements.BuildingElement();

            bHoMBuildingElement.BuildingElementProperties.BuildingElementType = ((TBD.BuildingElementType)tbdBuildingElement.BEType).ToBHoM();

            bHoMBuildingElement.BuildingElementProperties.Name = tbdBuildingElement.name;

            double buildingElementBEType = tbdBuildingElement.BEType;
            bhomBuildingElementProperties.CustomData.Add("buildingElementBEType", buildingElementBEType);

            System.Drawing.Color buildingElementColour = Query.GetRGB(tbdBuildingElement.colour);
            bhomBuildingElementProperties.CustomData.Add("buildingElementColour", buildingElementColour);

            string buildingElementDescription = tbdBuildingElement.description;
            bhomBuildingElementProperties.CustomData.Add("buildingElementDescription", buildingElementDescription);

            bool buildingElementIsAir = tbdBuildingElement.ghost != 0;
            bhomBuildingElementProperties.CustomData.Add("buildingElementIsAir", buildingElementIsAir);

            bool buildingElementIsGround = tbdBuildingElement.ground != 0;
            bhomBuildingElementProperties.CustomData.Add("buildingElementIsGround", buildingElementIsGround);

            string buildingElementGUID = tbdBuildingElement.GUID;
            bhomBuildingElementProperties.CustomData.Add("buildingElementGUID", buildingElementGUID);

            string buildingElementName = tbdBuildingElement.name;
            bhomBuildingElementProperties.CustomData.Add("buildingElementName", buildingElementName);

            //why it was not imported into bhomBuildingElementProperties????
            //double buildingElementWidth = tbdBuildingElement.width;
            //bHoMBuildingElement.BuildingElementProperties.CustomData.Add("buildingElementWidth", buildingElementWidth);

            double buildingElementWidth = tbdBuildingElement.width;
            bhomBuildingElementProperties.CustomData.Add("buildingElementWidth", Math.Round(buildingElementWidth, 3));

            double aUvalue = 0;
            double agValue = 0;
            double aLtValue = 0;

            //tas exposes tranparent building element all value as list  
            //1. LtValuegValue,  7. Uvalue,  6. gValue
            agValue = G(tbdBuildingElement);
            bhomBuildingElementProperties.CustomData.Add("buildingElementgvalue", agValue);

            aLtValue = LT(tbdBuildingElement);
            bhomBuildingElementProperties.CustomData.Add("buildingElementLtvalue", aLtValue);

            aUvalue = U(tbdBuildingElement);
            bhomBuildingElementProperties.CustomData.Add("buildingElementUvalue", aUvalue);

            //if (tbdBuildingElement.ghost != -1)
            //{
            //Assign Materila Layer to the object

            bhomBuildingElementProperties.Construction = tbdConstruction.ToBHoMConstruction();


            //List<BHE.Interface.IMaterial> bHoMMaterial = new List<BHE.Interface.IMaterial>();
            //double tbdMaterialThickness = 0;
            //int aIndex = 1;
            //material tbdMaterial = null;
            //while ((tbdMaterial = tbdConstruction.materials(aIndex)) != null)
            //{
            //    tbdMaterial = tbdConstruction.materials(aIndex);
            //    tbdMaterialThickness += tbdMaterial.width;
            //    //aThicknessAnalytical += tbdConstruction.materialWidth[aIndex];
            //    aIndex++;
            //}

            bhomBuildingElementProperties.CustomData.Add("MaterialLayersThickness", ToTBDConstructionThickness(tbdBuildingElement.GetConstruction()));

            //}

            return bhomBuildingElementProperties;
        }

        /***************************************************/

        public static double ToTBDConstructionThickness(this TBD.Construction tbdConstruction, int Decimals = 3)

        {
            double tbdMaterialThickness = 0;

            if (tbdConstruction == null)
                tbdMaterialThickness = 0;
            else
            {
                List<BHE.Interface.IMaterial> bHoMMaterial = new List<BHE.Interface.IMaterial>();
                int aIndex = 1;
                material tbdMaterial = null;
                while ((tbdMaterial = tbdConstruction.materials(aIndex)) != null)
                {
                    tbdMaterial = tbdConstruction.materials(aIndex);
                    tbdMaterialThickness += tbdMaterial.width;
                    aIndex++;
                }
            }


            return tbdMaterialThickness;
        }

        /***************************************************/

        public static double U(TBD.buildingElement tbdBuildingElement, int Decimals = 3)
        {
            TBD.Construction aConstruction = tbdBuildingElement.GetConstruction();
            if (aConstruction == null)
                return -1;

            object aObject = aConstruction.GetUValue();
            List<float> aValueList = Generic.Functions.GetList(aObject);
            switch ((BuildingElementType)tbdBuildingElement.BEType)
            {
                case BuildingElementType.Ceiling:
                    return Math.Round(aValueList[4], Decimals);
                case BuildingElementType.CurtainWall:
                    return Math.Round(aValueList[6], Decimals);
                case BuildingElementType.DoorElement:
                    return Math.Round(aValueList[0], Decimals);
                case BuildingElementType.ExposedFloor:
                    return Math.Round(aValueList[2], Decimals);
                case BuildingElementType.ExternalWall:
                    return Math.Round(aValueList[0], Decimals);
                case BuildingElementType.FrameELement:
                    return Math.Round(aValueList[0], Decimals);
                case BuildingElementType.Glazing:
                    return Math.Round(aValueList[6], Decimals);
                case BuildingElementType.InternalFloor:
                    return Math.Round(aValueList[5], Decimals);
                case BuildingElementType.InternallWall:
                    return Math.Round(aValueList[3], Decimals);
                case BuildingElementType.NoBEType:
                    return -1;
                case BuildingElementType.NullElement:
                    return -1;
                case BuildingElementType.RaisedFloor:
                    return Math.Round(aValueList[5], Decimals);
                case BuildingElementType.RoofElement:
                    return Math.Round(aValueList[1], Decimals);
                case BuildingElementType.RoofLight:
                    return Math.Round(aValueList[6], Decimals);
                case BuildingElementType.ShadeElement:
                    return -1;
                case BuildingElementType.SlabOnGrade:
                    return Math.Round(aValueList[2], Decimals);
                case BuildingElementType.SolarPanel:
                    return -1;
                case BuildingElementType.UndergroundCeiling:
                    return Math.Round(aValueList[2], Decimals);
                case BuildingElementType.UndergroundSlab:
                    return Math.Round(aValueList[2], Decimals);
                case BuildingElementType.UndergroundWall:
                    return Math.Round(aValueList[0], Decimals);
                case BuildingElementType.VehicleDoor:
                    return Math.Round(aValueList[0], Decimals);
            }
            return -1;
        }

        public static List<float> U(TBD.Construction tbdConstruction)
        {

            object aObject = tbdConstruction.GetUValue();
            List<float> aValueList = Generic.Functions.GetList(aObject);
            aValueList.Add(0);
            aValueList.Add(1);
            aValueList.Add(2);
            aValueList.Add(3);
            aValueList.Add(4);
            aValueList.Add(5);
            aValueList = aValueList.Cast<float>().ToList();
            return aValueList;
        }

        public static double G(TBD.buildingElement tbdBuildingElement, int Decimals = 3)
        {
            TBD.Construction aConstruction = tbdBuildingElement.GetConstruction();
            if (aConstruction == null)
                return -1;
            TBD.ConstructionTypes aConstructionTypes = aConstruction.type;
            if (aConstructionTypes == TBD.ConstructionTypes.tcdTransparentConstruction)
            {
                object aObject = aConstruction.GetGlazingValues();
                List<float> aValueList = Generic.Functions.GetList(aObject);
                return Math.Round(aValueList[5], Decimals);
            }
            return 0;
        }

        public static double LT(TBD.buildingElement tbdBuildingElement, int Decimals = 3)
        {
            TBD.Construction aConstruction = tbdBuildingElement.GetConstruction();
            if (aConstruction == null)
                return 0;

            TBD.ConstructionTypes aConstructionTypes = aConstruction.type;
            if (aConstructionTypes == TBD.ConstructionTypes.tcdTransparentConstruction)
            {
                object aObject = aConstruction.GetGlazingValues();
                List<float> aValueList = Generic.Functions.GetList(aObject);
                return Math.Round(aValueList[0], Decimals);
            }
            return 0;
        }

        private static object ToDouble(object arg)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHE.Elements.Construction ToBHoMConstruction(this TBD.Construction tbdConstruction)
        {
            if (tbdConstruction == null)
                return null;
            else
            {
                double tbdMaterialThickness = 0;
                int aIndex = 1;
                material tbdMaterial = null;
                while ((tbdMaterial = tbdConstruction.materials(aIndex)) != null)
                {
                    tbdMaterial = tbdConstruction.materials(aIndex);
                    tbdMaterialThickness += tbdMaterial.width;
                    aIndex++;
                }

                BHE.Elements.Construction bhomConstruction = new BHE.Elements.Construction()
                {
                    Materials = tbdConstruction.ToBHoM(),
                    Thickness = Math.Round(tbdMaterialThickness, 3),
                    Name = tbdConstruction.name,
                    BHoM_Guid = new Guid(tbdConstruction.GUID),
                    //UValues = tbdConstruction.GetUValue() as List<double>,
                    UValues = (U(tbdConstruction) as List<float>).ConvertAll(x => (double)x),
                    ConstructionType = tbdConstruction.type.ToBHoM(),
                    AdditionalHeatTransfer = tbdConstruction.additionalHeatTransfer,
                    FFactor = tbdConstruction.FFactor,
                };
                bhomConstruction.CustomData.Add("tbdConstrDescription", tbdConstruction.description);
                return bhomConstruction;

            }
        }

        /***************************************************/

        public static BHE.Elements.ConstructionType ToBHoM(this TBD.ConstructionTypes type)
        {
            switch (type)
            {
                case ConstructionTypes.tcdOpaqueConstruction:
                    return ConstructionType.Opaque;
                case ConstructionTypes.tcdTransparentConstruction:
                    return ConstructionType.Transparent;
                default:
                    return ConstructionType.Undefined;
            }
        }

        /***************************************************/

        public static BHA.Elements.Level ToBHoM(this TAS3D.Floor t3dFloor)
        {
            return new BHA.Elements.Level()
            {
                Elevation = t3dFloor.level,
                Name = t3dFloor.name,
            };
        }

        /***************************************************/

        public static BHE.Elements.Panel ToBHoM(this TBD.RoomSurface tbdRoomSurface)
        {
            BHE.Elements.Panel bHoMPanel = new BHE.Elements.Panel();

            TBD.Perimeter tbdPerimeter = tbdRoomSurface.GetPerimeter();
            TBD.Polygon tbdPolygon = tbdPerimeter.GetFace();

            BHG.ICurve curve = ToBHoM(tbdPolygon);
            BHG.PolyCurve polyCurve = Geometry.Create.PolyCurve(new List<BHG.ICurve> { curve });

            bHoMPanel.PanelCurve = polyCurve;
            //bHoMPanel.ElementType = ((TBD.BuildingElementType)tasRoomSrf.zoneSurface.buildingElement.BEType).ToString();

            return bHoMPanel;

        }

        /***************************************************/

        public static BHE.Interface.IMaterial ToBHoM(this TBD.material tbdMaterial)
        {
            BHE.Elements.MaterialType materialtype = ToBHoM((TBD.MaterialTypes)tbdMaterial.type);
            BHE.Materials.Material material = new BHE.Materials.Material();
            material.Name = tbdMaterial.name;
            material.Thickness = tbdMaterial.width;
            material.MaterialType = ToBHoM((TBD.MaterialTypes)tbdMaterial.type);

            switch (material.MaterialType)
            {
                case MaterialType.Opaque:

                    material.MaterialProperties = new BHE.Properties.MaterialPropertiesOpaque
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        Conductivity = tbdMaterial.conductivity,
                        SpecificHeat = tbdMaterial.specificHeat,
                        Density = tbdMaterial.density,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                        SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                        LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                        LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                        EmissivityExternal = tbdMaterial.externalEmissivity,
                        EmissivityInternal = tbdMaterial.internalEmissivity,
                    };
                    break;

                case MaterialType.Transparent:

                    material.MaterialProperties = new BHE.Properties.MaterialPropertiesTransparent
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        Conductivity = tbdMaterial.conductivity,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                        SolarTransmittance = tbdMaterial.solarTransmittance,
                        SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                        LightTransmittance = tbdMaterial.lightTransmittance,
                        LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                        LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                        EmissivityExternal = tbdMaterial.externalEmissivity,
                        EmissivityInternal = tbdMaterial.internalEmissivity
                    };
                    material.MaterialProperties.CustomData.Add("MaterialIsBling", tbdMaterial.isBlind);
                    break;

                case MaterialType.Gas:

                    material.MaterialProperties = new BHE.Properties.MaterialPropertiesGas
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        ConvectionCoefficient = tbdMaterial.convectionCoefficient,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor
                    };

                    break;
            }
            return material;
        }

        public static List<BHE.Interface.IMaterial> ToBHoM(this TBD.Construction tbdConstruction)
        {
            //Assign Material Layer to the object
            List<BHE.Interface.IMaterial> bHoMMaterials = new List<BHE.Interface.IMaterial>();

            double tbdMaterialThickness = 0;
            int aIndex = 1;
            material tbdMaterial = null;
            //check if not null


            while ((tbdMaterial = tbdConstruction.materials(aIndex)) != null)
            {
                tbdMaterial = tbdConstruction.materials(aIndex);
                tbdMaterialThickness += tbdMaterial.width;
                //aThicknessAnalytical += tbdConstruction.materialWidth[aIndex];
                //BHE.Elements.MaterialType materialtype = ToBHoMMaterialType((TBD.MaterialTypes)tbdMaterial.type);

                BHE.Materials.Material material = new BHE.Materials.Material();
                material.Name = tbdMaterial.name;
                material.Thickness = tbdMaterial.width;
                material.MaterialType = ToBHoM((TBD.MaterialTypes)tbdMaterial.type);

                switch (material.MaterialType)
                {
                    case MaterialType.Opaque:

                        material.MaterialProperties = new BHE.Properties.MaterialPropertiesOpaque
                        {
                            Name = tbdMaterial.name,
                            Description = tbdMaterial.description,
                            Conductivity = tbdMaterial.conductivity,
                            SpecificHeat = tbdMaterial.specificHeat,
                            Density = tbdMaterial.density,
                            VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                            SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                            SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                            LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                            LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                            EmissivityExternal = tbdMaterial.externalEmissivity,
                            EmissivityInternal = tbdMaterial.internalEmissivity,
                        };
                        break;

                    case MaterialType.Transparent:
                        material.MaterialProperties = new BHE.Properties.MaterialPropertiesTransparent
                        {
                            Name = tbdMaterial.name,
                            Description = tbdMaterial.description,
                            Conductivity = tbdMaterial.conductivity,
                            VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                            SolarTransmittance = tbdMaterial.solarTransmittance,
                            SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                            SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                            LightTransmittance = tbdMaterial.lightTransmittance,
                            LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                            LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                            EmissivityExternal = tbdMaterial.externalEmissivity,
                            EmissivityInternal = tbdMaterial.internalEmissivity
                        };

                        //if (bhomTransparentMaterial.MaterialProperties is GlazingMaterialProperties)
                        //    (bhomTransparentMaterial.MaterialProperties as GlazingMaterialProperties).IsBlind = (tbdMaterial.isBlind == 1);
                        //bHoMMaterial.Add(bhomTransparentMaterial);
                        material.MaterialProperties.CustomData.Add("MaterialIsBlind", tbdMaterial.isBlind);
                        break;

                    case MaterialType.Gas:
                        material.MaterialProperties = new BHE.Properties.MaterialPropertiesGas
                        {
                            Name = tbdMaterial.name,
                            Description = tbdMaterial.description,
                            ConvectionCoefficient = tbdMaterial.convectionCoefficient,
                            VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor
                        };
                        break;
                }

                bHoMMaterials.Add(material);
                aIndex++;
            }


            return bHoMMaterials;
        }

        /***************************************************/

        public static BHE.Elements.InternalCondition ToBHoM(this TBD.InternalCondition tbdInternalCondition)
        {
            if (tbdInternalCondition == null)
                return null;

            // get Internal Condition
            BHE.Elements.InternalCondition bHoMInternalCondition = new BHE.Elements.InternalCondition
            {
                Name = tbdInternalCondition.name,
                IncludeSolarInMeanRadiantTemp = tbdInternalCondition.includeSolarInMRT != 0,//converting TAS int to Bool

            };
            string tbdICDescription = tbdInternalCondition.description;
            bHoMInternalCondition.CustomData.Add("tbdICDescription", tbdICDescription);

            //Day Types
            int GetTypeIndex = 0;
            TBD.dayType tbdICDayType = null;
            while ((tbdICDayType = tbdInternalCondition.GetDayType(GetTypeIndex)) != null)
            {
                bHoMInternalCondition.DayTypes.Add(tbdICDayType.ToBHoM());
                GetTypeIndex++;
            }

            //get Internal Gain
            TBD.IInternalGain tbdICInternalGain = null;
            tbdICInternalGain = tbdInternalCondition.GetInternalGain();

            bHoMInternalCondition.InternalGain.Illuminance = tbdICInternalGain.targetIlluminance;
            bHoMInternalCondition.InternalGain.Name = tbdICInternalGain.name;
            bHoMInternalCondition.InternalGain.OutsideAirRatePerPerson = tbdICInternalGain.freshAirRate;
            bHoMInternalCondition.InternalGain.PersonGain = tbdICInternalGain.personGain;
            bHoMInternalCondition.InternalGain.RadiationProperties.EquipmentRadiation = tbdICInternalGain.equipmentRadProp;
            bHoMInternalCondition.InternalGain.CoefficientProperties.EquipmentViewCoefficient = tbdICInternalGain.equipmentViewCoefficient;
            bHoMInternalCondition.InternalGain.RadiationProperties.LightingRadiation = tbdICInternalGain.lightingRadProp;
            bHoMInternalCondition.InternalGain.CoefficientProperties.LightingViewCoefficient = tbdICInternalGain.lightingViewCoefficient;
            bHoMInternalCondition.InternalGain.RadiationProperties.OccupantRadiation = tbdICInternalGain.occupantRadProp;
            bHoMInternalCondition.InternalGain.CoefficientProperties.OccupantViewCoefficient = tbdICInternalGain.occupantViewCoefficient;

            double tbdInternalGainActivityID = tbdICInternalGain.activityID;
            bHoMInternalCondition.InternalGain.CustomData.Add("tbdInternalGainActivityID", tbdInternalGainActivityID);

            string tbdInternalGainDescription = tbdICInternalGain.description;
            bHoMInternalCondition.InternalGain.CustomData.Add("tbdInternalGainDescription", tbdInternalGainDescription);

            double tbdInternalDomesticHotWater = tbdICInternalGain.domesticHotWater;
            bHoMInternalCondition.InternalGain.CustomData.Add("tbdInternalDomesticHotWater", tbdInternalDomesticHotWater);

            //get Emitter
            TBD.Emitter tbdEmitter = null;
            tbdEmitter = tbdInternalCondition.GetHeatingEmitter();
            bHoMInternalCondition.Emitter.EmitterType = EmitterType.Heating;
            bHoMInternalCondition.Emitter.Name = tbdEmitter.name;
            bHoMInternalCondition.Emitter.EmitterProperties.RadiantProportion = tbdEmitter.radiantProportion;
            bHoMInternalCondition.Emitter.EmitterProperties.ViewCoefficient = tbdEmitter.viewCoefficient;
            bHoMInternalCondition.Emitter.EmitterProperties.SwitchOffOutsideTemp = tbdEmitter.offOutsideTemp;
            bHoMInternalCondition.Emitter.EmitterProperties.MaxOutsideTemp = tbdEmitter.maxOutsideTemp;

            string tbdEmitterDescriptionHeating = tbdEmitter.description;
            bHoMInternalCondition.Emitter.CustomData.Add("tbdEmitterDescriptionHeating", tbdEmitterDescriptionHeating);

            tbdEmitter = tbdInternalCondition.GetCoolingEmitter();
            bHoMInternalCondition.Emitter.EmitterType = EmitterType.Cooling;
            bHoMInternalCondition.Emitter.Name = tbdEmitter.name;
            bHoMInternalCondition.Emitter.EmitterProperties.RadiantProportion = tbdEmitter.radiantProportion;
            bHoMInternalCondition.Emitter.EmitterProperties.ViewCoefficient = tbdEmitter.viewCoefficient;
            bHoMInternalCondition.Emitter.EmitterProperties.SwitchOffOutsideTemp = tbdEmitter.offOutsideTemp;
            bHoMInternalCondition.Emitter.EmitterProperties.MaxOutsideTemp = tbdEmitter.maxOutsideTemp;

            string tbdEmitterDescriptionCooling = tbdEmitter.description;
            bHoMInternalCondition.Emitter.CustomData.Add("tbdEmitterDescriptionCooling", tbdEmitterDescriptionCooling);

            //get Thermostat
            TBD.Thermostat tbdICThermostat = null;
            tbdICThermostat = tbdInternalCondition.GetThermostat();

            bHoMInternalCondition.Thermostat.Name = tbdICThermostat.name;
            bHoMInternalCondition.Thermostat.ControlRange = tbdICThermostat.controlRange;
            bHoMInternalCondition.Thermostat.ProportionalControl = tbdICThermostat.proportionalControl != 0; //converting TAS int to Bool

            double tbdThermostatRadiantProportion = tbdICThermostat.radiantProportion;
            bHoMInternalCondition.Thermostat.CustomData.Add("tbdThermostatRadiantProportion", tbdThermostatRadiantProportion);

            string tbdThermostatDescription = tbdICThermostat.description;
            bHoMInternalCondition.Thermostat.CustomData.Add("tbdThermostatDescription", tbdThermostatDescription);

            bHoMInternalCondition.Thermostat.CustomData.Add("upperLimit", GetSingleValueUpperLimitFromThermostat(tbdICThermostat));

            bHoMInternalCondition.Thermostat.CustomData.Add("lowerLimit", GetSingleValueLowerLimitFromThermostat(tbdICThermostat));

            //add Profiles
            //To DO add profiles in Groups firsts thermostat and second InternalGains
            bHoMInternalCondition.Thermostat.Profiles = BH.Engine.TAS.Query.Profiles(tbdICThermostat);

            return bHoMInternalCondition;
        }

        /***************************************************/
        
        //Get Upper value from tbdUpperLimitProfile - Cooling Set Point
        public static float GetSingleValueUpperLimitFromThermostat(this TBD.Thermostat tbdICThermostat)
        {
            float maxUL = 150;

            if (tbdICThermostat == null)
                return -1;

            TBD.profile tbdUpperLimitProfile = tbdICThermostat.GetProfile((int)TBD.Profiles.ticUL);
            switch (tbdUpperLimitProfile.type)
            {
                case TBD.ProfileTypes.ticValueProfile:
                    maxUL = tbdUpperLimitProfile.value;
                    break;
                case TBD.ProfileTypes.ticHourlyProfile:
                    for (int i = 1; i <= 24; i++)
                    {
                        if (tbdUpperLimitProfile.hourlyValues[i] <= maxUL)
                            maxUL = tbdUpperLimitProfile.hourlyValues[i];
                    }

                    break;
                case TBD.ProfileTypes.ticYearlyProfile:
                    for (int i = 1; i <= 8760; i++)
                    {
                        if (tbdUpperLimitProfile.yearlyValues[i] >= maxUL)
                            maxUL = tbdUpperLimitProfile.yearlyValues[i];
                    }
                    break;
                    // case other profile types etc.
            }
            return maxUL;

        }

        //Get Lower value from tbdLowerLimitProfile - Heating Set Point
        public static float GetSingleValueLowerLimitFromThermostat(this TBD.Thermostat tbdICThermostat)
        {
            float minLL = -50;

            if (tbdICThermostat == null)
                return -1;

            TBD.profile tbdLowerLimitProfile = tbdICThermostat.GetProfile((int)TBD.Profiles.ticLL);
            switch (tbdLowerLimitProfile.type)
            {
                case TBD.ProfileTypes.ticValueProfile:
                    minLL = tbdLowerLimitProfile.value;
                    break;

                case TBD.ProfileTypes.ticHourlyProfile:
                    for (int i = 1; i <= 24; i++)
                    {
                        if (tbdLowerLimitProfile.hourlyValues[i] >= minLL)
                            minLL = tbdLowerLimitProfile.hourlyValues[i];
                    }
                    break;

                case TBD.ProfileTypes.ticYearlyProfile:
                    for (int i = 1; i <= 8760; i++)
                    {
                        if (tbdLowerLimitProfile.yearlyValues[i] >= minLL)
                            minLL = tbdLowerLimitProfile.yearlyValues[i];
                    }
                    break;
                    // case other profile types etc.
            }
            return minLL;

        }


        /***************************************************/
        public static BHE.Elements.SimulationDayType ToBHoM(this TBD.dayType tbdDayType)
        {
            if (tbdDayType.name.Equals("Weekday"))
                return SimulationDayType.Weekday;
            if (tbdDayType.name.Equals("Monday"))
                return SimulationDayType.Monday;
            if (tbdDayType.name.Equals("Tuesday"))
                return SimulationDayType.Tuesday;
            if (tbdDayType.name.Equals("Wednesday"))
                return SimulationDayType.Wednesday;
            if (tbdDayType.name.Equals("Thursday"))
                return SimulationDayType.Thursday;
            if (tbdDayType.name.Equals("Friday"))
                return SimulationDayType.Friday;
            if (tbdDayType.name.Equals("Saturday"))
                return SimulationDayType.Saturday;
            if (tbdDayType.name.Equals("Sunday"))
                return SimulationDayType.Sunday;
            if (tbdDayType.name.Equals("Public Holiday"))
                return SimulationDayType.PublicHoliday;
            if (tbdDayType.name.Equals("CDD"))
                return SimulationDayType.CoolingDesignDay;
            if (tbdDayType.name.Equals("HDD"))
                return SimulationDayType.HeatingDesignDay;
            if (tbdDayType.name.Equals("Weekend"))
                return SimulationDayType.Weekend;

            return SimulationDayType.Undefined;
        }


        /***************************************************/

        public static BHE.Elements.Emitter ToBHoM(this TBD.Emitter tasEmitterProperties)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        internal static BHE.Elements.Profile ToBHoMProfile(this TBD.profile tbdProfile, ProfileCategory prfileCategory)
        {
            BHE.Elements.Profile bHoMProfile = new Profile();
            bHoMProfile.Category = ProfileCategory.Thermostat;
            switch (tbdProfile.type)
            {

                case TBD.ProfileTypes.ticValueProfile:
                    bHoMProfile.ProfileType = ProfileType.Value;
                    bHoMProfile.Value = tbdProfile.value;
                    bHoMProfile.Name = tbdProfile.name;
                    bHoMProfile.CustomData.Add("ProfileDescriptionUL", tbdProfile.description);
                    bHoMProfile.MultiplicationFactor = tbdProfile.factor;
                    bHoMProfile.SetBackValue = tbdProfile.setbackValue;
                    break;

                case TBD.ProfileTypes.ticHourlyProfile:
                    bHoMProfile.ProfileType = ProfileType.Hourly;
                    bHoMProfile.Name = tbdProfile.name;
                    bHoMProfile.CustomData.Add("ProfileDescriptionUL", tbdProfile.description);
                    break;

                case TBD.ProfileTypes.ticYearlyProfile:
                    bHoMProfile.ProfileType = ProfileType.Yearly;
                    bHoMProfile.Name = tbdProfile.name;
                    bHoMProfile.CustomData.Add("ProfileDescriptionUL", tbdProfile.description);
                    break;
                    // case other profile types etc.
            }

            return bHoMProfile;

        }


        /***************************************************/



        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this TBD.TasPoint tbdPoint)
        {
            BHG.Point bHoMPoint = new BHG.Point()
            {
                X = tbdPoint.x,
                Y = tbdPoint.y,
                Z = tbdPoint.z
            };
            return bHoMPoint;
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this TBD.Polygon tbdPolygon)  // TODO : When BH.oM.Geometry.Contour is implemented, Polyline can be replaced with Contour
        {
            //
            //  Not sure how this is working but that's a very strange way of getting points for Tas. Are you sure it is the only way?
            //
            List<BHG.Point> bHoMPointList = new List<BHG.Point>();
            int pointIndex = 0;
            TasPoint tasPoint = null;
            while ((tasPoint = tbdPolygon.GetPoint(pointIndex)) != null)
            {
                tasPoint = tbdPolygon.GetPoint(pointIndex);
                if (tasPoint == null) { break; }
                bHoMPointList.Add(tasPoint.ToBHoM());
                pointIndex++;
            }
            bHoMPointList.Add(bHoMPointList[0]);
            return new BHG.Polyline { ControlPoints = bHoMPointList };
        }

        /***************************************************/



        /***************************************************/
        /**** Enums                                     ****/
        /***************************************************/

        public static BHE.Elements.MaterialType ToBHoM(this TBD.MaterialTypes tbdMaterialType)
        {
            switch (tbdMaterialType)
            {
                case MaterialTypes.tcdOpaqueLayer:
                case MaterialTypes.tcdOpaqueMaterial:
                    return BHE.Elements.MaterialType.Opaque;
                case MaterialTypes.tcdTransparentLayer:
                    return BHE.Elements.MaterialType.Transparent;
                case MaterialTypes.tcdGasLayer:
                    return BHE.Elements.MaterialType.Gas;
                default:
                    return BHE.Elements.MaterialType.Opaque;
            }
        }

        /***************************************************/

        public static BHE.Elements.BuildingElementType ToBHoM(this TBD.BuildingElementType tbdBuildingElementType)
        {
            switch (tbdBuildingElementType)
            {
                case TBD.BuildingElementType.EXTERNALWALL:
                case TBD.BuildingElementType.INTERNALWALL:
                case TBD.BuildingElementType.UNDERGROUNDWALL:
                    return BHE.Elements.BuildingElementType.Wall;
                case TBD.BuildingElementType.ROOFELEMENT:
                case TBD.BuildingElementType.ROOFLIGHT:
                    return BHE.Elements.BuildingElementType.Roof;
                case TBD.BuildingElementType.CEILING:
                case TBD.BuildingElementType.UNDERGROUNDCEILING:
                    return BHE.Elements.BuildingElementType.Ceiling;
                case TBD.BuildingElementType.EXPOSEDFLOOR:
                case TBD.BuildingElementType.INTERNALFLOOR:
                case TBD.BuildingElementType.RAISEDFLOOR:
                case TBD.BuildingElementType.SLABONGRADE:
                case TBD.BuildingElementType.UNDERGROUNDSLAB:
                    return BHE.Elements.BuildingElementType.Floor;
                case TBD.BuildingElementType.DOORELEMENT:
                    return BHE.Elements.BuildingElementType.Door;
                case TBD.BuildingElementType.GLAZING:
                    return BHE.Elements.BuildingElementType.Window;
                case TBD.BuildingElementType.CURTAINWALL:
                    return BHE.Elements.BuildingElementType.CurtainWall;
                case TBD.BuildingElementType.FRAMEELEMENT:
                case TBD.BuildingElementType.NOBETYPE:
                case TBD.BuildingElementType.NULLELEMENT:
                case TBD.BuildingElementType.SHADEELEMENT:
                case TBD.BuildingElementType.SOLARPANEL:
                case TBD.BuildingElementType.VEHICLEDOOR:
                    return BHE.Elements.BuildingElementType.Undefined;
                default:
                    return BHE.Elements.BuildingElementType.Wall;
            }
        }

        /***************************************************/
        //TAS TBD Building Element Type
        public enum BuildingElementType
        {
            /// <summary>Ceiling</summary>
            Ceiling = 8,
            /// <summary>Curtain Wall</summary>
            CurtainWall = 16,
            /// <summary>Door Element</summary>
            DoorElement = 14,
            /// <summary>Exposed Floor</summary>
            ExposedFloor = 19,
            /// <summary>External Wall</summary>
            ExternalWall = 2,
            /// <summary>Frame Element</summary>
            FrameELement = 15,
            /// <summary>Glazing</summary>
            Glazing = 12,
            /// <summary>Internal Floor</summary>
            InternalFloor = 4,
            /// <summary>Internal Wall</summary>
            InternallWall = 1,
            /// <summary>No Building Element Type</summary>
            NoBEType = 0,
            /// <summary>Null Element</summary>
            NullElement = 17,
            /// <summary>Raised Floor</summary>
            RaisedFloor = 10,
            /// <summary>Roof Element</summary>
            RoofElement = 3,
            /// <summary>Roof Light</summary>
            RoofLight = 13,
            /// <summary>Shade Element</summary>
            ShadeElement = 5,
            /// <summary>Slab On Grade</summary>
            SlabOnGrade = 11,
            /// <summary>Solar Panel</summary>
            SolarPanel = 18,
            /// <summary>Underground Ceiling</summary>
            UndergroundCeiling = 9,
            /// <summary>Underground Slab</summary>
            UndergroundSlab = 7,
            /// <summary>Underground Wall</summary>
            UndergroundWall = 6,
            /// <summary>Vehicle Door</summary>
            VehicleDoor = 20,
        }

    }
}
