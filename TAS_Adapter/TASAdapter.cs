using System;
using System.Collections.Generic;
using BH.oM.Queries;

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

  
        
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private TBD.TBDDocumentClass m_TBDDocumentInstance = new TBD.TBDDocumentClass();


        /***************************************************/
    }
}

