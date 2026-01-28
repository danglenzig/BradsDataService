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
                if (ConversionTools.TryGetNewRuntimeDatum(f.FieldType, out IRuntimeDatum datum))
                {
                    data[f.FieldID] = datum; ///////
                }
            }
        }

        public void SetFieldNamesAndTypesByID(Dictionary<string, FieldNameAndType> inDict)
        {
            fieldNamesAndTypesByID = new Dictionary<string, FieldNameAndType>(inDict);
        }
        public void SetDataDict(Dictionary<string, IRuntimeDatum> inDict)
        {
            data = new Dictionary<string, IRuntimeDatum>(inDict);
        }
    }
}


