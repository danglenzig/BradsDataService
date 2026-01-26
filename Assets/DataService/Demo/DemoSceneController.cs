using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BradsDataService;

public class DemoSceneController : MonoBehaviour
{


    [SerializeField] private FieldSO playerNameField;


    private void Start()
    {


        SetTestPlayerName("Namey Nameson");


        
    }

    private void SetTestPlayerName(string pName)
    {
        if (RuntimeDataService.TryUpdateRuntimeDatum<string>(pName, playerNameField.FieldID))
        {
            DebugRuntimeData();
        }
    }

    private void DebugRuntimeData()
    {
        if (RuntimeDataService.TryGetRuntimeData(out RuntimeData data))
        {
            IReadOnlyDictionary<string, FieldNameAndType> nameTypeDict = data.FieldNamesAndTypesByID;
            IReadOnlyDictionary<string, IRuntimeDatum> valuesDict = data.Data;

            foreach (string id in data.FieldNamesAndTypesByID.Keys.ToList<string>())
            {
                string fieldName = nameTypeDict[id].fieldName;
                EnumFieldType fieldType = nameTypeDict[id].fieldType;
                IRuntimeDatum datum = valuesDict[id];

                string valStr = string.Empty;

                switch (fieldType)
                {
                    case EnumFieldType.STRING:
                        valStr = (datum as RuntimeDatum<string>).Value;
                        break;
                }
                Debug.Log($"{fieldName} -- {fieldType.ToString()}: {valStr}");
            }
        }
    }
}
