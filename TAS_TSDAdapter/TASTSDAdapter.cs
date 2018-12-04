using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using BH.oM.DataManipulation.Queries;
using BH.oM.Base;
using BH.Engine;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        public TasTSDAdapter(string tSDFilePath = "")
        {
            //TSD application
            tsdFilePath = tSDFilePath;

            AdapterId = BH.Engine.TAS.Convert.TSDAdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            GetTsdDocument();
            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic, false);
            }

            CloseTsdDocument();
            
            return success ? objects.ToList() : new List<IObject>();
        }

        public override IEnumerable<object> Pull(IQuery query, Dictionary<string, object> config = null)
        {
            List<IBHoMObject> returnObjs = new List<IBHoMObject>();

            GetTsdDocument(); //Open the TSD Document for pulling data from

            FilterQuery aFilterQuery = query as FilterQuery;
            switch (BH.Engine.Adapters.TAS.Query.QueryType(aFilterQuery))
            {
                case oM.Adapters.TAS.Enums.QueryType.IsExternal:
                    break;
                default:
                    //modified to allow filtering element we need
                    returnObjs.AddRange(Read(aFilterQuery));
                    break;
            }

            CloseTsdDocument();

            //Return the package
            return returnObjs;
        }

        private TSD.TSDDocument tsdDocument=null;
        private string tsdFilePath = null;
        
        //To get the TSD Document
        private TSD.TSDDocument GetTsdDocument()
        {
            TSD.TSDDocument tsdDocument = new TSD.TSDDocument();
            if (!String.IsNullOrEmpty(tsdFilePath) && System.IO.File.Exists(tsdFilePath))
                tsdDocument.open(tsdFilePath);
            else if (!String.IsNullOrEmpty(tsdFilePath))
                tsdDocument.create(tsdFilePath); //What if an existing file has the same name?
            else
                ErrorLog.Add("The TSD file does not exist");
            return tsdDocument;
        }

        //Close and save the TSD Document
        private void CloseTsdDocument(bool save=true)
        {
            if (tsdDocument!=null)
            {
                if (save == true)
                    tsdDocument.save();
                tsdDocument.close();
                if(tsdDocument!=null)
                {
                    ClearCOMObject(tsdDocument);
                    tsdDocument = null;
                }
            }
        }
    }
}
