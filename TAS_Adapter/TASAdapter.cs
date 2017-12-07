using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BH.Adapter.Queries;
using System.Diagnostics;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/
              

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public TasAdapter (string TasFilePath, bool save)
        {
                try
                {
                    TBD.TBDDocument Document = new TBD.TBDDocument();
                    Document.open(TasFilePath);
                    if (save == true)
                    {
                        Document.save();
                    }
                }
                catch
                {
                    Console.WriteLine("Cannot load TAS, check that TAS is installed and a license is available");
                }
        }

    

        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/
               

        /***************************************************/
        /**** Public Getter Methods                     ****/
        /***************************************************/

        
        /***************************************************/
        /**** Private Fileds                            ****/
        /***************************************************/
    }
}

