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
        [Description("")]
        [Input("distanceTolerance", "distanceTolerance is used as input for CleanPolyline method used for opening, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("planarTolerance", "Set tolerance for planar surfaces, default is set to ")]
        public static TASSettings TASSettings(double planarTolerance = BH.oM.Geometry.Tolerance.Distance, double DistanceTolerance = BH.oM.Geometry.Tolerance.Distance)
        {
            return new TASSettings
            {
                PlanarTolerance = planarTolerance,
                DistanceTolerance = DistanceTolerance,
            };
        }
    }
}