using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environmental.Elements;

namespace BH.Engine.TAS.Query
{
    public static partial class Query
    {

        //public static float GetOrientation(BHEE.BuildingElementPanel bHoMPanel)
        //{
        //    List<BHG.Point> controlpoints = BH.Engine.Geometry.Query.ControlPoints(bHoMPanel.PolyCurve);
        //    BHG.Vector normal = Geometry.Compute.FitPlane(bHoMPanel.PolyCurve).Normal;

        //    float orientation;

        //    if (normal.X == 0 && normal.Y == 0 && normal.Z == 1)
        //        orientation = 270;
        //    else if (normal.X == 1 && normal.Y == 0 && normal.Z == 0)
        //        orientation = 90;
        //    else
        //        orientation = 0;

        //    return orientation;
        //}

        /***************************************************/

        //public static double GetFloorArea(BHEE.Space bHoMSpace)
        //{
        //    double floorArea = 0;
        //    List<BHEE.BuildingElementPanel> bHoMPanels = bHoMSpace.BuildingElementPanel;
        //    List<double> areaSum = new List<double>();
        //    foreach (BHEE.BuildingElementPanel panel in bHoMPanels)
        //    {
        //        if (GetInclanation(panel) == 180) // if floor
        //        floorArea = (float)Engine.Geometry.Query.Area(panel.PolyCurve);
        //        areaSum.Add(floorArea); //if we have many floor surfaces in the same space we ned to calculate the sum
        //    }
        //    return areaSum.Sum();
        //}

        /***************************************************/

        //public static float GetInclanation(BHEE.BuildingElementPanel bHoMBuildingElementPanel)
        //{

        //    float inclanation; //TAS uses float and therefore we do that as well
        //    BHG.Vector normal = Geometry.Compute.FitPlane(bHoMBuildingElementPanel.PolyCurve).Normal;

        //    if (normal.X == 0 && normal.Y == 0 && normal.Z == -1)
        //        inclanation = 0; //ceiling
        //    else if (normal.X == 0 && normal.Y == 0 && normal.Z == 1)
        //        inclanation = 180; //floor
        //    else
        //        inclanation = 90; //walls

        //    return inclanation;
        //}

        /***************************************************/

        //public static float GetAltitudeRange(BHEE.BuildingElementPanel bHoMBuildingElementPanel)
        //{
        //    BHG.BoundingBox panelBoundingBox = BH.Engine.Geometry.Query.Bounds(bHoMBuildingElementPanel.PolyCurve);
        //    float altitudeRange = (float)panelBoundingBox.Max.Z - (float)panelBoundingBox.Min.Z;

        //    return altitudeRange;

        //}

        /***************************************************/

        //public static float GetAltitude(BHEE.BuildingElementPanel bHoMBuildingElementPanel)
        //{
        //    BHG.BoundingBox panelBoundingBox = BH.Engine.Geometry.Query.Bounds(bHoMBuildingElementPanel.PolyCurve);
        //    float altitude = (float)panelBoundingBox.Min.Z;

        //    return altitude;

        //}

        /***************************************************/

        //public static double GetVolume(BHEE.Space bHoMSpace)
        //{
        //    //This does only work for a space where all of the building element panels have the same height. We can change this later on.

        //    List<BHEE.BuildingElementPanel> bHoMPanels = bHoMSpace.BuildingElementPanel;
        //    List<BHEE.BuildingElementPanel> verticalPanels = new List<BHEE.BuildingElementPanel>();

        //    foreach (BHEE.BuildingElementPanel panel in bHoMPanels)
        //    {
        //        if (GetInclanation(bHoMSpace.BuildingElementPanel[0]) == 90) // if wall
        //        {
        //            verticalPanels.Add(panel);
        //        }
        //    }

        //    double volume = GetFloorArea(bHoMSpace) * GetAltitude(verticalPanels[0]);
        //    return volume;
        //}

        /***************************************************/
    }
}
