using UnityEngine;
namespace BradsDataService
{

    public enum EnumFieldType
    {
        STRING,
        FLOAT,
        INT,
        BOOL,
        VECTOR_2,
        VECTOR_3,
        VECTOR_4,
    }

    [CreateAssetMenu(fileName = "FieldSO", menuName = "Data Service/Field")]
    public class FieldSO : ScriptableObject
    {
        [SerializeField] private string fieldName;
        [SerializeField] private EnumFieldType fieldType;
        [SerializeField] private string fieldID = string.Empty;

        public string FieldName { get => fieldName; }
        public EnumFieldType FieldType { get => fieldType; }
        public string FieldID { get => fieldID; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(fieldID))
            {
                fieldID = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }
    }
}