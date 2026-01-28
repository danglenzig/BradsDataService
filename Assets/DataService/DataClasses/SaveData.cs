using UnityEngine;
using System.Collections.Generic;

namespace BradsDataService
{
    [System.Serializable]
    // A utility class for serializing lists to JSON
    public class StringListWrapper
    {
        public List<string> strings;
    }

    [System.Serializable]
    public class WrappedStringDict
    {
        public StringListWrapper wrappedKeys;
        public StringListWrapper wrappedValues;

        public WrappedStringDict(StringListWrapper _wrappedKeys,StringListWrapper _wrappedValues)
        {
            wrappedKeys = _wrappedKeys;
            wrappedValues = _wrappedValues;
        }

    }


    [System.Serializable]
    public class SavedDatumValue
    {
        public string fieldID;
        public string fieldName;
        public EnumFieldType fieldType;

        public string stringValue;
        public float floatValue;
        public int intValue;
        public bool boolValue;

        public float vector2X;
        public float vector2Y;
        
        public float vector3X;
        public float vector3Y;
        public float vector3Z;
        
        public float vector4W;
        public float vector4X;
        public float vector4Y;
        public float vector4Z;

        public string listValue;

        public string dictKeysValue;
        public string dictValuesValue;

        //public StringListWrapper stringListWrapperValue;
        //public WrappedStringDict wrappedStringDict;
    }

    [System.Serializable]
    public class SaveData
    {
        public List<SavedDatumValue> Data = new();
    }
}


