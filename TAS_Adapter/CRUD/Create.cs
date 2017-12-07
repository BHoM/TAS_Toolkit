using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHG = BH.oM.Geometry; 
using BHE = BH.oM.Environmental;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/


        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            if (typeof(BHoMObject).IsAssignableFrom(typeof(T)))
            {
                foreach (T obj in objects)
                {
                    Convert.ToTas(obj as dynamic);
                }
            }
        return true;
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/
    }
}
