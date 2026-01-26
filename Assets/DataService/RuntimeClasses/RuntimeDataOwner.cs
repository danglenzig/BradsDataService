using UnityEngine;
namespace BradsDataService
{
    public class RuntimeDataOwner : Singleton<RuntimeDataOwner>
    {
        [SerializeField] private DataCatalogSO dataCatalog;
        private RuntimeData data = null;

        private void OnEnable()
        {
            data = new RuntimeData();
            data.BuildFromSO(dataCatalog);
        }

        public bool TryUpdateRuntimeDatum<T>(T value, string fieldID)
        {
            if (!data.FieldNamesAndTypesByID.TryGetValue(fieldID, out FieldNameAndType fieldNameAndType)) { return false; }
            IRuntimeDatum datum = data.Data[fieldID];
            if (datum is RuntimeDatum<T> typedDatum)
            {
                typedDatum.Value = (T)(object)value;
                return true;
            }
            return false;
        }
        public bool TryGetRuntimeValue<T>(string fieldID, out T value)
        {
            value = default;
            if (!data.FieldNamesAndTypesByID.TryGetValue(fieldID, out FieldNameAndType fieldNameAndType)) { return false; }
            IRuntimeDatum datum = data.Data[fieldID];

            if (datum is RuntimeDatum<T> typedDatum)
            {
                value = typedDatum.Value;
                return true;
            }
            return false;
        }

        public RuntimeData GetFullRuntimeData()
        {
            return data;
        }
        public DataCatalogSO GetDataCatalog()
        {
            return dataCatalog;
        }

        
        public void SetRuntimeData(RuntimeData inData)
        {
            data = inData;
        }
    }
}

