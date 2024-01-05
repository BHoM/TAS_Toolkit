using BH.Engine.Adapter;
using BH.Engine.Base;
using BH.oM.Adapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.TAS
{
    public static partial class Compute
    {
        public static ITASFile OpenTASDocument(Type type, FileSettings file)
        {
            if (type == typeof(T3DDocument))
            {
                T3DDocument document = new T3DDocument()
                {
                    FilePath = file.GetFullFileName(),
                };

                if (File.Exists(file.GetFullFileName()))
                    document.Document.Open(file.GetFullFileName());
                else
                    document.Document.Create();

                return document;
            }
            else if (type == typeof(TBDDocument))
            {
                TBDDocument document = new TBDDocument();

                if (File.Exists(file.GetFullFileName()))
                    document.Document.open(file.GetFullFileName());
                else
                    document.Document.create(file.GetFullFileName());

                return document;
            }
            else if (type == typeof(TSDDocument))
            {
                TSDDocument document = new TSDDocument();

                if (File.Exists(file.GetFullFileName()))
                    document.Document.open(file.GetFullFileName());
                else
                    document.Document.create(file.GetFullFileName());

                return document;
            }
            else
            {
                BH.Engine.Base.Compute.RecordError($"The type: {type.FullName} is not supported for retreiving TAS documents");
                return null;
            }
        }
    }
}
