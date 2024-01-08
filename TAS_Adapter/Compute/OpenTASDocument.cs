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
        public static ITASFile OpenTASDocument(Type type, FileSettings file)
        {
            if (type == typeof(T3DDocument))
            {
                TAS3D.T3DDocument document = new TAS3D.T3DDocument();

                if (File.Exists(file.GetFullFileName()))
                    document.Open(file.GetFullFileName());
                else
                    document.Create();

                return new T3DDocument() { Document = document, FilePath = file.GetFullFileName() };
            }
            else if (type == typeof(TBDDocument))
            {
                TBD.TBDDocument document = new TBD.TBDDocument();

                if (File.Exists(file.GetFullFileName()))
                    document.open(file.GetFullFileName());
                else
                    document.create(file.GetFullFileName());

                return new TBDDocument() { Document = document };
            }
            else if (type == typeof(TSDDocument))
            {
                TSD.TSDDocument document = new TSD.TSDDocument();

                if (File.Exists(file.GetFullFileName()))
                    document.open(file.GetFullFileName());
                else
                    document.create(file.GetFullFileName());

                return new TSDDocument() { Document = document };
            }
            else
            {
                BH.Engine.Base.Compute.RecordError($"The type: {type.FullName} is not supported for retreiving TAS documents");
                return null;
            }
        }
    }
}
