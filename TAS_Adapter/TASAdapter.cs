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

        public TasAdapter(string TBDFilePath = "", string T3DFilePath = "")
        {
            AdapterId = ID;

            //TBD application
            if (!String.IsNullOrEmpty(TBDFilePath) && System.IO.File.Exists(TBDFilePath))
               TBDDocumentInstance.open(TBDFilePath);
            
            else if (!String.IsNullOrEmpty(TBDFilePath))
               TBDDocumentInstance.create(TBDFilePath); //TODO: what if an existing file has the same name? 
            
            else
                ErrorLog.Add("The TBD file does not exist");


            //T3D application
            if (!String.IsNullOrEmpty(T3DFilePath) && System.IO.File.Exists(T3DFilePath))
               TAS3DDocumentInstance.Open(T3DFilePath);

            else if (!String.IsNullOrEmpty(T3DFilePath))
                TAS3DDocumentInstance.Create(); //TODO: what if an existing file has the same name?
            
            else
                ErrorLog.Add("The T3D file does not exist");
            
        }


        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public TAS3D.T3DDocumentClass TAS3DDocumentInstance = new TAS3D.T3DDocumentClass();
        public TBD.TBDDocumentClass TBDDocumentInstance = new TBD.TBDDocumentClass();
        
       



        /***************************************************/
        /**** Public Getter Methods                     ****/
        /***************************************************/


        /***************************************************/
        /**** Private Fileds                            ****/
        /***************************************************/
    }
}

