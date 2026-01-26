using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace BradsDataService
{

    

    public static class SingleSaveService
    {
        private static string saveFilePath => Path.Combine(Application.persistentDataPath, "save.json");
        
        /////////
        // API //
        /////////
        
        public static void ClearSave()
        {
            if (SaveExists()) { File.Delete(saveFilePath); }
        }
        public static bool SaveExists()
        {
            return File.Exists(saveFilePath);
        }

        public static bool TrySave(RuntimeData runtimeData)
        {
            if (ConversionTools.TryConvertRuntimeDataToSaveData(runtimeData, out SaveData saveData))
            {
                ClearSave();
                string json = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(saveFilePath, json);
                return true;
            }
            return false;
        }
        public static bool TryLoad(out RuntimeData runtimeData)
        {
            runtimeData = null;
            if (TryGetSaveData(out SaveData saveData))
            {
                if (ConversionTools.TryConvertSaveDataToRuntimeData(saveData, out RuntimeData _runtimeData))
                {
                    runtimeData = _runtimeData;
                }
            }
            return runtimeData != null;
        }


        ///////////////////
        // Local Helpers //
        ///////////////////
        
        private static bool TryGetSaveData(out SaveData saveData)
        {
            saveData = null;
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
            return saveData != null;
        }

    }
}

