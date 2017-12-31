using System;
using TBDFile;
using T3DFile;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public const string ID = "TAS_id";

        public string AdapterName = "TAS_name";


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public TasAdapter(string tBDFilePath = "", string t3DFilePath = "")
        {
            AdapterId = ID;

            //TBD application
            if (!String.IsNullOrEmpty(tBDFilePath) && System.IO.File.Exists(tBDFilePath))
               m_TBDDocumentInstance.open(tBDFilePath);
            
            else if (!String.IsNullOrEmpty(tBDFilePath))
               m_TBDDocumentInstance.create(tBDFilePath); //TODO: what if an existing file has the same name? 
            
            else
                ErrorLog.Add("The TBD file does not exist");


            //T3D application
            if (!String.IsNullOrEmpty(t3DFilePath) && System.IO.File.Exists(t3DFilePath))
               m_TAS3DDocumentInstance.Open(t3DFilePath);

            else if (!String.IsNullOrEmpty(t3DFilePath))
                m_TAS3DDocumentInstance.Create(); //TODO: what if an existing file has the same name?
            
            else
                ErrorLog.Add("The T3D file does not exist");
            
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TAS3D.T3DDocumentClass m_TAS3DDocumentInstance = new TAS3D.T3DDocumentClass();  //TODO: Those should be either public properties or private fields

        private TBD.TBDDocumentClass m_TBDDocumentInstance = new TBD.TBDDocumentClass();


        /***************************************************/
    }
}

