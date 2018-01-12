using System;

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

        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TBD.TBDDocumentClass m_TBDDocumentInstance = new TBD.TBDDocumentClass();


        /***************************************************/
    }
}

