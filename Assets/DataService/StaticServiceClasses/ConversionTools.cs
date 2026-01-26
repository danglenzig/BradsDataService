using UnityEngine;

namespace BradsDataService
{
    public static class ConversionTools
    {
        public static bool TryConvertRuntimeDataToSaveData(RuntimeData runtimeData, out SaveData saveData)
        {
            saveData = null;

            //

            return saveData != null;
        }

        public static bool TryConvertSaveDataToRuntimeData(SaveData saveData, out RuntimeData runtimeData)
        {
            runtimeData = null;

            //


            return runtimeData != null;

        }


    }

    

}

