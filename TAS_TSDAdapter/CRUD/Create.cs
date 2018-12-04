using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BHE = BH.oM.Environment;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;
using BH.Engine.Environment;
using TSD;
using BH.Engine.TAS;

namespace BH.Adapter.TAS
{
    public partial class TasTSDAdapter : BHoMAdapter
    {
        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;
            if (typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                success = CreateCollection(objects as dynamic);
            }
            return success;
        }
        public static void ClearCOMObject(object Object)
        {
            if (Object == null) return;
            int intrefcount = 0;
            do
            {
                intrefcount = Marshal.FinalReleaseComObject(Object);
            } while (intrefcount > 0);
            Object = null;
        }

        private bool CreateCollection(IEnumerable<IBHoMObject> objects)
        {
            bool success = true;
            foreach (IBHoMObject obj in objects)
            {
                success &= Create(obj as dynamic);
            }
            return success;
        }
        private bool CreateCollection(IEnumerable<BHE.Elements.Space> spaces)
        {
            bool success = true;
            foreach (BHE.Elements.Space space in spaces)
            {
                //success &= Create(space, spaces);
            }
            return success;
        }

        private bool Create(BHE.Results.BuildingResult buildingResult)
        {
            TSD.BuildingData tsdBuildingData = tsdDocument.SimulationData.GetBuildingData();
            TSD.CoolingDesignData tsdCoolingDesignData = tsdDocument.SimulationData.GetCoolingDesignData(0);
            TSD.HeatingDesignData tsdHeatingDesignData = tsdDocument.SimulationData.GetHeatingDesignData(0);

            tsdBuildingData.GetHourlyBuildingResult(1, (short)tsdBuildingArray.externalTemperature);
            
            return true;
        }
        /*
        private bool Create(BHE.Properties.BuildingElementProperties buildingElementProperties)
        {
            TSD.Construction tsdConstruction = tsdDocument.Building.AddConstruction(null);
            tsdConstruction.name = buildingElementProperties.Name;
            //TODO: BuildingElementProperties can not handle Thickness
            //tsdConstruction.materialWidth[0] = (float)buildingElementProperties.Thickness; //which value in the array shall we use??

            return true;
        }

        private bool Create(BHE.Elements.Panel buildingElementPanel)
        {
            throw new NotImplementedException();
        }

        private bool Create(BHE.Elements.Opening buildingElementOpening)
        {
            throw new NotImplementedException();
        }

        private bool Create(BHE.Elements.InternalCondition internalCondition)
        {
            TSD.InternalCondition tsdInternalCondition = tsDocument.Building.AddIC(null);
            tsdInternalCondition.name = internalCondition.Name;
            return true; 
        }*/

    }
}
