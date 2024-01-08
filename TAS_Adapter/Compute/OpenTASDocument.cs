using BH.Engine.Adapter;
using BH.Engine.Base;
using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCD;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        public static TBDDocument OpenTASDocument(this TBDDocument document, FileSettings file)
        {
            TBD.TBDDocument tempDocument = new TBD.TBDDocument();

            if (File.Exists(file.GetFullFileName()))
                tempDocument.open(file.GetFullFileName());
            else
                tempDocument.create(file.GetFullFileName());

            document.Document = tempDocument;
            return document;
        }

        public static TSDDocument OpenTASDocument(this TSDDocument document, FileSettings file)
        {
            TSD.TSDDocument tempDocument = new TSD.TSDDocument();

            if (File.Exists(file.GetFullFileName()))
                tempDocument.open(file.GetFullFileName());
            else
                tempDocument.create(file.GetFullFileName());

            document.Document = tempDocument;
            return document;
        }
    }
}
