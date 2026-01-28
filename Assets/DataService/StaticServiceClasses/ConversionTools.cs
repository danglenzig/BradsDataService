using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BradsDataService
{
    

    public static class ConversionTools
    {
        public static StringListWrapper GetWrapperizedStringList(List<string> inList)
        {
            StringListWrapper wrappedStrings = new StringListWrapper();
            wrappedStrings.strings = new List<string>(inList);
            return wrappedStrings;
        }
        public static WrappedStringDict GetWrappedStringDict(Dictionary<string, string> inDict)
        {
            List<string> keysList = inDict.Keys.ToList<string>();
            List<string> valuesList = inDict.Values.ToList<string>();
            StringListWrapper keysWrapper = GetWrapperizedStringList(keysList);
            StringListWrapper valuesWrapper = GetWrapperizedStringList(valuesList);
            return new WrappedStringDict(keysWrapper, valuesWrapper);
        }
        public static bool TryGetDictFromWRappedStringDict(WrappedStringDict wrappedDict, out Dictionary<string, string> outDict)
        {
            outDict = new();
            List<string> keysList = wrappedDict.wrappedKeys.strings;
            List<string> valuesList = wrappedDict.wrappedValues.strings;
            if (keysList.Count != valuesList.Count) { return false; }
            for (int i = 0; i < keysList.Count; i++)
            {
                string keyStr = keysList[i];
                string valStr = valuesList[i];
                outDict[keyStr] = valStr;
            }
            return true;
        }

        public static bool TryConvertRuntimeDataToSaveData(RuntimeData runtimeData, out SaveData saveData)
        {
            saveData = null;
            if (runtimeData == null) { return false; }
            saveData = new SaveData();

            // iterate through the runtime data values
            foreach(var kvp in runtimeData.Data)
            {
                // get its ID and a reference to the datum
                string id = kvp.Key;
                IRuntimeDatum datum = kvp.Value;

                // get the field type
                if (!runtimeData.FieldNamesAndTypesByID.TryGetValue(id, out FieldNameAndType nameAndType))
                {
                    continue;
                }
                SavedDatumValue saved = new SavedDatumValue();
                saved.fieldID = id;
                saved.fieldType = nameAndType.fieldType;
                saved.fieldName = nameAndType.fieldName;

                // cast the savable as the appropriate type
                // and write the runtime value to the appropriate
                // SavedFieldValue field

                switch (saved.fieldType)
                {
                    case EnumFieldType.STRING:
                        saved.stringValue = (datum as RuntimeDatum<string>).Value; break;
                    case EnumFieldType.FLOAT:
                        saved.floatValue = (datum as RuntimeDatum<float>).Value; break;
                    case EnumFieldType.INT:
                        saved.intValue = (datum as RuntimeDatum<int>).Value; break;
                    case EnumFieldType.BOOL:
                        saved.boolValue = (datum as RuntimeDatum<bool>).Value; break;
                    case EnumFieldType.VECTOR_2:
                        Vector2 v2 = (datum as RuntimeDatum<Vector2>).Value;
                        saved.vector2X = v2.x;
                        saved.vector2Y = v2.y;
                        break;
                    case EnumFieldType.VECTOR_3:
                        Vector3 v3 = (datum as RuntimeDatum<Vector3>).Value;
                        saved.vector3X = v3.x;
                        saved.vector3Y = v3.y;
                        saved.vector3Z = v3.z;
                        break;
                    case EnumFieldType.VECTOR_4:
                        Vector4 v4 = (datum as RuntimeDatum<Vector4>).Value;
                        saved.vector4W = v4.w;
                        saved.vector4X = v4.x;
                        saved.vector4Y = v4.y;
                        saved.vector4Z = v4.z;
                        break;
                    case EnumFieldType.STRING_LIST:
                        List<string> liststr = (datum as RuntimeDatum<List<string>>).Value;
                        StringListWrapper listWrapper = GetWrapperizedStringList(liststr);
                        saved.listValue = JsonUtility.ToJson(listWrapper);
                        break;

                    case EnumFieldType.STRING_DICT:
                        Dictionary<string, string> dict = (datum as RuntimeDatum<Dictionary<string, string>>).Value;
                        List<string> keysList = dict.Keys.ToList<string>();
                        List<string> valsList = dict.Values.ToList<string>();
                        StringListWrapper keysWrapper = GetWrapperizedStringList(keysList);
                        StringListWrapper valsWrapper = GetWrapperizedStringList(valsList);
                        saved.dictKeysValue = JsonUtility.ToJson(keysWrapper);
                        saved.dictValuesValue = JsonUtility.ToJson(valsWrapper);
                        break;
                }
                // add it to the SaveData
                saveData.Data.Add(saved);
            }
            return true;
        }

        public static bool TryConvertSaveDataToRuntimeData(SaveData saveData, out RuntimeData runtimeData)
        {
            runtimeData = null;
            if (saveData == null) { return false; }
            runtimeData = new RuntimeData();
            Dictionary<string, FieldNameAndType> namesAndTypesDict = new();
            Dictionary<string, IRuntimeDatum> dataDict = new();

            foreach (SavedDatumValue savedDatum in saveData.Data)
            {
                string fieldName = savedDatum.fieldName;
                string fieldID = savedDatum.fieldID;
                EnumFieldType fieldType = savedDatum.fieldType;

                
                namesAndTypesDict[fieldID] = new FieldNameAndType(fieldName, fieldType);
                
                if (!TryGetNewRuntimeDatum(fieldType, out IRuntimeDatum datum)) { continue; }
                switch (fieldType)
                {
                    case EnumFieldType.STRING:
                        (datum as RuntimeDatum<string>).Value = savedDatum.stringValue; break;
                    case EnumFieldType.FLOAT:
                        (datum as RuntimeDatum<float>).Value = savedDatum.floatValue; break;
                    case EnumFieldType.INT:
                        (datum as RuntimeDatum<int>).Value = savedDatum.intValue; break;
                    case EnumFieldType.BOOL:
                        (datum as RuntimeDatum<bool>).Value = savedDatum.boolValue; break;
                    case EnumFieldType.VECTOR_2:
                        float v2x = savedDatum.vector2X;
                        float v2y = savedDatum.vector2Y;
                        (datum as RuntimeDatum<Vector2>).Value = new Vector2(v2x, v2y);
                        break;
                    case EnumFieldType.VECTOR_3:
                        float v3x = savedDatum.vector3X;
                        float v3Y = savedDatum.vector3Y;
                        float v3z = savedDatum.vector3Z;
                        (datum as RuntimeDatum<Vector3>).Value = new Vector3(v3x, v3Y, v3z);
                        break;
                    case EnumFieldType.VECTOR_4:
                        float v4w = savedDatum.vector4W;
                        float v4x = savedDatum.vector4X;
                        float v4y = savedDatum.vector4Y;
                        float v4z = savedDatum.vector4Z;
                        (datum as RuntimeDatum<Vector4>).Value = new Vector4(v4w, v4x, v4y, v4z);
                        break;

                    case EnumFieldType.STRING_LIST:
                        StringListWrapper listWrapper = JsonUtility.FromJson<StringListWrapper>(savedDatum.listValue);
                        (datum as RuntimeDatum<List<string>>).Value = new List<string>(listWrapper.strings);
                        break;
                    case EnumFieldType.STRING_DICT:
                        StringListWrapper keysWrapper = JsonUtility.FromJson<StringListWrapper>(savedDatum.dictKeysValue);
                        StringListWrapper valuesWrapper = JsonUtility.FromJson<StringListWrapper>(savedDatum.dictValuesValue);
                        WrappedStringDict wrappedDict = new WrappedStringDict(keysWrapper, valuesWrapper);
                        if (TryGetDictFromWRappedStringDict(wrappedDict, out Dictionary<string,string> outDict))
                        {
                            (datum as RuntimeDatum<Dictionary<string, string>>).Value = outDict;
                        }
                        break;
                }
                dataDict[fieldID] = datum;
            }
            runtimeData.SetFieldNamesAndTypesByID(namesAndTypesDict);
            runtimeData.SetDataDict(dataDict);
            return true;

        }

        
        

        public static List<string> GetStringListFromJson(string inString)
        {
            List<string> outList = new List<string>();
            if (string.IsNullOrEmpty(inString)) { return outList; }
            StringListWrapper wrapper = JsonUtility.FromJson<StringListWrapper>(inString);
            outList = wrapper.strings;
            return outList;
        }


        public static bool TryGetNewRuntimeDatum(EnumFieldType fType, out IRuntimeDatum runtimeDatum)
        {
            runtimeDatum = null;
            switch (fType)
            {
                case EnumFieldType.STRING:
                    runtimeDatum = new RuntimeDatum<string>(string.Empty); break;
                case EnumFieldType.FLOAT:
                    runtimeDatum = new RuntimeDatum<float>(0f); break;
                case EnumFieldType.INT:
                    runtimeDatum = new RuntimeDatum<int>(0); break;
                case EnumFieldType.BOOL:
                    runtimeDatum = new RuntimeDatum<bool>(false); break;
                case EnumFieldType.VECTOR_3:
                    runtimeDatum = new RuntimeDatum<Vector3>(new Vector3()); break;
                case EnumFieldType.VECTOR_2:
                    runtimeDatum = new RuntimeDatum<Vector2>(new Vector2()); break;
                case EnumFieldType.VECTOR_4:
                    runtimeDatum = new RuntimeDatum<Vector4>(new Vector4()); break;

                case EnumFieldType.STRING_LIST:
                    runtimeDatum = new RuntimeDatum<List<string>>(new List<string>());
                    break;
                case EnumFieldType.STRING_DICT:
                    Dictionary<string, string> dict = new();
                    runtimeDatum = new RuntimeDatum<Dictionary<string, string>>(dict);
                    break;
            }
            return (runtimeDatum != null);
        }
    }

    

}

