using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.TAS.Settings;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        [Description("Create a TASSettings object for use with the TAS adapters")]
        [Input("distanceTolerance", "distanceTolerance is used as input for CleanPolyline method used for opening, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("planarTolerance", "Set tolerance for planar surfaces, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("angleTolerance", "The tolerance of the angle that defines a straight line. Default is set to the value defined by BH.oM.Geometry.Tolerance.Angle")]
        [Input("minimumSegmentLength", "The length of the smallest allowed segment.Default is set to the value defined by BH.oM.Geometry.Tolerance.Distance")]
        [Output("tasSettings", "The TAS setting to use with the TAS adapter")]
        public static TASSettings TASSettings(double planarTolerance = BH.oM.Geometry.Tolerance.Distance, double distanceTolerance = BH.oM.Geometry.Tolerance.Distance, double minimumSegmentLength = BH.oM.Geometry.Tolerance.Distance, double angleTolerance = BH.oM.Geometry.Tolerance.Angle)
        {
            return new TASSettings
            {
                PlanarTolerance = planarTolerance,
                DistanceTolerance = distanceTolerance,
                MinimumSegmentLength = minimumSegmentLength,
                AngleTolerance = angleTolerance,
            };
        }
    }
}