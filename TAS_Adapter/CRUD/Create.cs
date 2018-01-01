using System.Collections.Generic;
using BH.oM.Base;


namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;


            if (typeof(BHoMObject).IsAssignableFrom(typeof(T)))
            {
                
                foreach (T obj in objects)
                {
                    Engine.TAS.Convert.ToTas(obj as dynamic);
                }
                                
            }

            return success;
        }

        /***************************************************/
    }
}
