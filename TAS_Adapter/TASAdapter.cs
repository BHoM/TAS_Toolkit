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
            m_TBDFilePath = tBDFilePath;

            AdapterId = BH.Engine.TAS.Convert.AdapterID;
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            GetTBDDocument();

            bool success = true;
            MethodInfo miToList = typeof(Enumerable).GetMethod("Cast");
            foreach (var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo miListObject = miToList.MakeGenericMethod(new[] { typeGroup.Key });

                var list = miListObject.Invoke(typeGroup, new object[] { typeGroup });

                success &= Create(list as dynamic, false);
            }

            CloseTBDDocument();
            return success ? objects.ToList() : new List<IObject>();

        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TBD.TBDDocument m_TBDDocument = null;
        private string m_TBDFilePath = null;

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private TBD.TBDDocument GetTBDDocument()
        {
            m_TBDDocument = new TBD.TBDDocument();
            if (!String.IsNullOrEmpty(m_TBDFilePath) && System.IO.File.Exists(m_TBDFilePath))
                m_TBDDocument.open(m_TBDFilePath);

            else if (!String.IsNullOrEmpty(m_TBDFilePath))
                m_TBDDocument.create(m_TBDFilePath); //TODO: what if an existing file has the same name? 

            else
                ErrorLog.Add("The TBD file does not exist");
            return m_TBDDocument;
        }

        // we close and save TBD
        private void CloseTBDDocument(bool save = true)
        {
            if (m_TBDDocument != null)
            {
                if (save == true)
                    m_TBDDocument.save();

                m_TBDDocument.close();

                if (m_TBDDocument != null)
                {
                    // issue with closing files and not closing 
                    ClearCOMObject(m_TBDDocument);
                    m_TBDDocument = null;
                }

            }

        }
    }

    /***************************************************/
}


