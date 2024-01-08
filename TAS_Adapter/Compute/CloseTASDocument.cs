using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Interface methods                  ****/
        /***************************************************/

        public static bool ICloseTASDocument(ITASFile document, bool save)
        {
            return CloseTASDocument(document as dynamic, save);
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/

        private static void CloseTASDocument(TBDDocument document, bool save)
        {
            if (save)
                document.Document.save();

            document.Document.close();
            ClearCOMObject(document.Document);
        }

        /***************************************************/

        private static void CloseTASDocument(TSDDocument document, bool save)
        {
            if (save)
                document.Document.save();

            document.Document.close();
            ClearCOMObject(document.Document);
        }

        /***************************************************/
        /**** Fallback methods                          ****/
        /***************************************************/

        private static void CloseTASDocument(ITASFile document, bool save)
        {
            BH.Engine.Base.Compute.RecordError("An error occurred while closing and saving the TAS document.");
        }
    }
}
