using BH.oM.Adapters.TAS;
using BH.oM.Environment.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBD;
using TPD;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        public static bool ValidateInput(this TASTSDConfig config)
        {
            if (config.ResultQuery == TSDResultType.Undefined)
            {
                BH.Engine.Base.Compute.RecordError("Result output cannot be undefined");
                return false;
            }

            if (config.SimulationType == SimulationResultType.Undefined)
            {
                BH.Engine.Base.Compute.RecordError("Simulation type cannot be undefined");
                return false;
            }

            if (config.ResultUnit == ProfileResultUnit.Undefined)
            {
                BH.Engine.Base.Compute.RecordError("Unit type cannot be undefined");
                return false;
            }

            if (config.ResultType == ProfileResultType.Undefined)
            {
                BH.Engine.Base.Compute.RecordError("Result type cannot be undefined");
                return false;
            }

            if ((config.ResultQuery == TSDResultType.CoolingDesignDay || config.ResultQuery == TSDResultType.HeatingDesignDay) && 
                (config.SimulationType == SimulationResultType.BuildingResult || config.SimulationType == SimulationResultType.BuildingElementResult))
            {
                BH.Engine.Base.Compute.RecordError("Heating and Cooling Design Day results are only available on Space Result Types");
                return false;
            }

            if (config.ResultUnit == ProfileResultUnit.Daily && (config.Day < 1 || config.Day > 365))
            {
                BH.Engine.Base.Compute.RecordError("Please select a day between 1 and 365 inclusive for Daily Results");
                return false;
            }

            if (config.ResultUnit == ProfileResultUnit.Hourly && (config.Hour < 1 || config.Hour > 24))
            {
                BH.Engine.Base.Compute.RecordError("Please select an hour between 1 and 24 inclusive for Hourly Results");
                return false;
            }

            if (config.ResultUnit == ProfileResultUnit.Yearly && (config.Hour != -1 || config.Day != -1))
            {
                BH.Engine.Base.Compute.RecordWarning("Day and Hour inputs are not used when pulling Yearly Results");
            }

            return true;
        }
    }
}
