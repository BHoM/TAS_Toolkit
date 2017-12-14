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
        //private TBD.TBDDocument myTBDDocument;
        //private TAS3D.T3DDocument myTAS3DDocument;    
                
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public TasAdapter (string TBDFilePath = "", string T3DFilePath = "")
        {
            AdapterId = ID;   

            if (!String.IsNullOrEmpty(TBDFilePath) && System.IO.File.Exists(TBDFilePath))
            {                
                TASDocument.open(TBDFilePath);               
                TAS3DDocument.Open(T3DFilePath);
            }
            else
            {                
                ErrorLog.Add("Cannot load TAS, check that TAS is installed and a license is available");
            }
        }


        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public TAS3D.T3DDocumentClass TAS3DDocument = new TAS3D.T3DDocumentClass();
        public TBD.TBDDocumentClass TASDocument = new TBD.TBDDocumentClass();
        
       

        /***************************************************/
        /**** Public Getter Methods                     ****/
        /***************************************************/


        /***************************************************/
        /**** Private Fileds                            ****/
        /***************************************************/
    }
}

