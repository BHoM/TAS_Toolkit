﻿using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;


            if (typeof(IObject).IsAssignableFrom(typeof(T)))
            {
                
                foreach (T obj in objects)
                {
                    Engine.TAS.Convert.ToTas(obj as dynamic);
                }
                                
            }

            return success;
        }

        /***************************************************/

        private bool Create(BH.oM.Environmental.Properties.BuildingElementProperties bHoMBuildingElementProperties)
        {
            bool sucess = true;
            TBD.Construction tasConstruction = BH.Engine.TAS.Convert.ToTas(bHoMBuildingElementProperties);
            return sucess;
        }

        /***************************************************/
    }
}
