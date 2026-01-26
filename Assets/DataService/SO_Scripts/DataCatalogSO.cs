using UnityEngine;
using System.Collections.Generic;


namespace BradsDataService
{
    [CreateAssetMenu(fileName = "DataCatalogSO", menuName = "Data Service/Data Catalog")]
    public class DataCatalogSO : ScriptableObject
    {
        [SerializeField] private List<FieldSO> fields;
        public IReadOnlyList<FieldSO> Fields { get => fields; }
    }
}


