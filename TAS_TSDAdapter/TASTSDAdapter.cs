using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using BH.oM.DataManipulation.Queries;
using BH.oM.Base;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        private string tsdFilePath = null;

        public TasTSDAdapter(string tSDFilePath = "")
        {
            tsdFilePath = tSDFilePath;

            AdapterId = BH.Engine.TAS.Convert.TSDAdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            bool success = true;
            return success ? objects.ToList() : new List<IObject>();
        }

        public override IEnumerable<object> Pull(IQuery query, Dictionary<string, object> config = null)
        {
            List<IBHoMObject> returnObjs = new List<IBHoMObject>();

            //Return the package
            return returnObjs;
        }

    }
}
