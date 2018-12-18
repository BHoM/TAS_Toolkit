using System.ComponentModel;

using BH.oM.DataManipulation.Queries;
using BH.oM.Environment.Elements;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.TAS
{
    [Description("BH.Engine.TAS.Create IsExternalFilterQuery => gets all exteral BuildingElements so we have shell")]
    public static partial class Create
    {
        [Description("Creates FilterQuery which filters all Building.")]
        [Input("TBC", "TBC")]
        [Output("FilterQuery")]
        public static FilterQuery IsExternalFilterQuery()
        {
            FilterQuery aFilterQuery = new FilterQuery();
            //aFilterQuery.Type = typeof(Space);
            aFilterQuery.Equalities[Convert.FilterQuery.QueryType] = BH.oM.Adapters.TAS.Enums.QueryType.IsExternal;
            return aFilterQuery;
        }
    }
}
