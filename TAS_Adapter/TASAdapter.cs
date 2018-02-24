using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.oM.Queries;
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
            if (!String.IsNullOrEmpty(tBDFilePath) && System.IO.File.Exists(tBDFilePath))
                m_TBDDocumentInstance.open(tBDFilePath);

            else if (!String.IsNullOrEmpty(tBDFilePath))
                m_TBDDocumentInstance.create(tBDFilePath); //TODO: what if an existing file has the same name? 

            else
                ErrorLog.Add("The TBD file does not exist");

            AdapterId = BH.Engine.TAS.Convert.AdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented

        }




        public override List<IBHoMObject> Push(IEnumerable<IBHoMObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic, false);
            }

            return success ? objects.ToList(): new List<IBHoMObject>();

        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TBD.TBDDocument m_TBDDocumentInstance = new TBD.TBDDocument();


        /***************************************************/
    }
}

