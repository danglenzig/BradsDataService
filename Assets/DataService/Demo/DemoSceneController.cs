using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BradsDataService;

public class DemoSceneController : MonoBehaviour
{
    [SerializeField] private FieldSO playerNameField;
    [SerializeField] private FieldSO playerStatusEffectField;

    private void Start()
    {
        SetTestPlayerName("Namey Nameson");
        SetTestPlayerStatusFX();
        DebugRuntimeData();
    }

    private void SetTestPlayerName(string pName)
    {
        RuntimeDataService.TryUpdateRuntimeDatum<string>(pName, playerNameField.FieldID);
    }

    private void SetTestPlayerStatusFX()
    {
        List<string> fxList = new();
        fxList.Add("HUNGRY");
        fxList.Add("ANGRY");
        fxList.Add("TIRED");
        RuntimeDataService.TryUpdateRuntimeDatum<List<string>>(fxList, playerStatusEffectField.FieldID);
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
                    case EnumFieldType.STRING_LIST:
                        valStr = StringifyList((datum as RuntimeDatum<List<string>>).Value);
                        break;

                }
                Debug.Log($"{fieldName} -- {fieldType.ToString()}: {valStr}");
            }
        }
    }

    private string StringifyList(List<string> strList)
    {
        string outStr = string.Empty;
        foreach(string item in strList)
        {
            outStr += $"{item}, ";
        }
        return outStr;
    }



}
