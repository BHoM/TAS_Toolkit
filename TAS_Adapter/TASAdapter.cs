using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.oM.DataManipulation.Queries;
using BH.oM.Base;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public TasAdapter(string tBDFilePath = "")
        {
            //TBD application
            tbdFilePath = tBDFilePath;

            AdapterId = BH.Engine.TAS.Convert.AdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            GetTbdDocument();

            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic, false);
            }

            CloseTbdDocument();
            return success ? objects.ToList() : new List<IObject>();
        }

        public override IEnumerable<object> Pull(IQuery query, Dictionary<string, object> config = null)
        {
            List<IBHoMObject> returnObjs = new List<IBHoMObject>();

            GetTbdDocument(); //Open the TBD Document for pulling data from

            FilterQuery aFilterQuery = query as FilterQuery;
            switch (BH.Engine.Adapters.TAS.Query.QueryType(aFilterQuery))
            {
                case oM.Adapters.TAS.Enums.QueryType.IsExternal:
                    returnObjs.AddRange(ReadExternalBuildingElements());
                    break;
                default:
                    //modified to allow filtering element we need
                    returnObjs.AddRange(Read(aFilterQuery));
                    break;
            }




            CloseTbdDocument();

            //Return the package
            return returnObjs;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TBD.TBDDocument tbdDocument = null;
        private string tbdFilePath = null;

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private TBD.TBDDocument GetTbdDocument()
        {
            tbdDocument = new TBD.TBDDocument();
            if (!String.IsNullOrEmpty(tbdFilePath) && System.IO.File.Exists(tbdFilePath))
                tbdDocument.open(tbdFilePath);

            else if (!String.IsNullOrEmpty(tbdFilePath))
                tbdDocument.create(tbdFilePath); //TODO: what if an existing file has the same name? 

            else
                ErrorLog.Add("The TBD file does not exist");
            return tbdDocument;
        }

        // we close and save TBD
        private void CloseTbdDocument(bool save = true)
        {
            if (tbdDocument != null)
            {
                if (save == true)
                    tbdDocument.save();

                tbdDocument.close();

                if (tbdDocument != null)
                {
                    // issue with closing files and not closing 
                    ClearCOMObject(tbdDocument);
                    tbdDocument = null;
                }

            }

        }
    }

    /***************************************************/
}


