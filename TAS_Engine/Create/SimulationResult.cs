using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environment.Results;

namespace BH.Engine.TAS
{
    public static partial class Create
    {
        public static ProfileResult ProfileResult(ProfileResultType type, ProfileResultUnits unit, List<double> results)
        {
            return new oM.Environment.Results.ProfileResult
            {
                ResultType = type,
                ResultUnit = unit,
                Results = results,
            };
        }
    }
}
