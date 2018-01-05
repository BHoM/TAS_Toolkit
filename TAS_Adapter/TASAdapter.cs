using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BH.Adapter.Queries;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

        public TasAdapter(string TBDFilePath = "")
        {
            AdapterId = ID;

            //TBD application
            if (!String.IsNullOrEmpty(TBDFilePath) && System.IO.File.Exists(TBDFilePath))
               TBDDocumentInstance.open(TBDFilePath);
            
            else if (!String.IsNullOrEmpty(TBDFilePath))
               TBDDocumentInstance.create(TBDFilePath); //TODO: what if an existing file has the same name? 
            
            else
                ErrorLog.Add("The TBD file does not exist");
            
        }


        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public TBD.TBDDocumentClass TBDDocumentInstance = new TBD.TBDDocumentClass();
        
       



        /***************************************************/
        /**** Public Getter Methods                     ****/
        /***************************************************/


        /***************************************************/
        /**** Private Fileds                            ****/
        /***************************************************/
    }
}

