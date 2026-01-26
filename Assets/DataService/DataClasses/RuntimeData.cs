using UnityEngine;
using BradsEvents;
using System.Collections.Generic;
namespace BradsDataService
{

    public interface IRuntimeDatum { }
    public class RuntimeDatum<T>: IRuntimeDatum
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                EventService.TryTriggerDataUpdatedEvent();
            }
        }
        public RuntimeDatum(T _inValue)
        {
            _value = _inValue;
        }
    }
    public readonly struct FieldNameAndType
    {
        public readonly string fieldName;
        public readonly EnumFieldType fieldType;
        public FieldNameAndType(string _fieldName, EnumFieldType _fieldType)
        {
            fieldName = _fieldName;
            fieldType = _fieldType;
        }
    }

    public class RuntimeData
    {
        private Dictionary<string, IRuntimeDatum> data = new();
        private Dictionary<string, FieldNameAndType> fieldNamesAndTypesByID = new();

        public IReadOnlyDictionary<string, IRuntimeDatum> Data { get => data; }
        public IReadOnlyDictionary<string, FieldNameAndType> FieldNamesAndTypesByID { get => fieldNamesAndTypesByID; }

        public void BuildFromSO(DataCatalogSO catalog)
        {
            foreach (FieldSO f in catalog.Fields)
            {
                if (!fieldNamesAndTypesByID.ContainsKey(f.FieldID))
                {
                    fieldNamesAndTypesByID[f.FieldID] = new FieldNameAndType(f.FieldName, f.FieldType);
                }
                if (TryGetNewDatum(f.FieldType, out IRuntimeDatum datum))
                {
                    data[f.FieldID] = datum;
                }
            }
        }
        

        private bool TryGetNewDatum(EnumFieldType fieldType, out IRuntimeDatum datum)
        {
            datum = null;
            switch (fieldType)
            {
                case EnumFieldType.STRING:
                    datum = new RuntimeDatum<string>(string.Empty); break;
                case EnumFieldType.FLOAT:
                    datum = new RuntimeDatum<float>(0f); break;
                case EnumFieldType.INT:
                    datum = new RuntimeDatum<int>(0); break;
                case EnumFieldType.BOOL:
                    datum = new RuntimeDatum<bool>(false); break;
                case EnumFieldType.VECTOR_3:
                    datum = new RuntimeDatum<Vector3>(new Vector3()); break;
                case EnumFieldType.VECTOR_2:
                    datum = new RuntimeDatum<Vector2>(new Vector2()); break;
                case EnumFieldType.VECTOR_4:
                    datum = new RuntimeDatum<Vector4>(new Vector4()); break;
            }
            return (datum != null);
        }

    }
}


