using UnityEngine;
using System.Collections.Generic;

namespace BradsDataService
{
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
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Vector4 vector4Value;
    }

    [System.Serializable]
    public class SaveData
    {
        public List<SavedDatumValue> Data = new();
    }
}


