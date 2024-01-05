using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        //TODO - make method separators
        public static bool ICloseTASDocument(ITASFile document, bool save)
        {
            return CloseTASDocument(document as dynamic, save);
        }

        public static void CloseTASDocument(T3DDocument document, bool save)
        {
            if (save)
                document.Document.Save(document.FilePath);

            document.Document.Close();
            ClearCOMObject(document.Document);
        }

        public static void CloseTASDocument(TBDDocument document, bool save)
        {
            if (save)
                document.Document.save();

            document.Document.close();
            ClearCOMObject(document.Document);
        }

        public static void CloseTASDocument(TSDDocument document, bool save)
        {
            if (save)
                document.Document.save();

            document.Document.close();
            ClearCOMObject(document.Document);
        }
    }
}
