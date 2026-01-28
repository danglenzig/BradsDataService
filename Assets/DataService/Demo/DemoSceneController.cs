using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BradsDataService;


public enum EnumQuestStatus
{
    NOT_STARTED,
    STARTED,
    FINISHED,
}

public class DemoSceneController : MonoBehaviour
{
    [SerializeField] private FieldSO playerNameField;           // string
    [SerializeField] private FieldSO playerStatusEffectField;   // List<string>
    [SerializeField] private FieldSO playerPositionField;       // Vector3
    [SerializeField] private FieldSO playerHealthField;         // int
    [SerializeField] private FieldSO secondsPlayedField;        // float
    [SerializeField] private FieldSO playerMapPosition;         // Vector2
    [SerializeField] private FieldSO playerColor;               // Vector4
    [SerializeField] private FieldSO questStatuses;             // Dictionary<string, string>
    [SerializeField] private FieldSO playerOnboarded;           // bool

    private void Start()
    {
        SetTestPlayerName("Namey Nameson");
        SetTestPlayerStatusFX();
        SetTestPlayerPosition();
        SetTestPlayerMapPos();
        SetTestPlayerHealth();
        SetTestSecondsPlayed();
        SetTestPlayerColor();
        SetTestPlayerOnboarded();
        SetTestQuestStatuses();
        DebugRuntimeData();
    }

    private void SetTestPlayerName(string pName)
    {
        RuntimeDataService.TryUpdateRuntimeDatum<string>(pName, playerNameField.FieldID);
    }
    private void SetTestPlayerColor()
    {
        Vector4 colVector = new Vector4(.2f, .3f, .4f, .5f);
        RuntimeDataService.TryUpdateRuntimeDatum<Vector4>(colVector, playerColor.FieldID);
    }

    private void SetTestPlayerStatusFX()
    {
        List<string> fxList = new();
        fxList.Add("HUNGRY");
        fxList.Add("ANGRY");
        fxList.Add("TIRED");
        RuntimeDataService.TryUpdateRuntimeDatum<List<string>>(fxList, playerStatusEffectField.FieldID);
    }

    private void SetTestPlayerPosition()
    {
        Vector3 pos = new Vector3(1f, 2f, 3f);
        RuntimeDataService.TryUpdateRuntimeDatum<Vector3>(pos, playerPositionField.FieldID);
    }
    private void SetTestPlayerMapPos()
    {
        Vector2 mapPos = new Vector2(100f, 200f);
        RuntimeDataService.TryUpdateRuntimeDatum<Vector2>(mapPos, playerMapPosition.FieldID);
    }
    private void SetTestPlayerHealth()
    {
        RuntimeDataService.TryUpdateRuntimeDatum<int>(100, playerHealthField.FieldID);
    }
    private void SetTestSecondsPlayed()
    {
        RuntimeDataService.TryUpdateRuntimeDatum<float>(10f, secondsPlayedField.FieldID);
    }
    private void SetTestPlayerOnboarded()
    {
        RuntimeDataService.TryUpdateRuntimeDatum<bool>(true, playerOnboarded.FieldID);
    }

    private void SetTestQuestStatuses()
    {
        Dictionary<string, EnumQuestStatus> questDict = new();
        questDict["Do a little dance"] = EnumQuestStatus.FINISHED;
        questDict["Make a little love"] = EnumQuestStatus.STARTED;
        questDict["Get down tonight"] = EnumQuestStatus.NOT_STARTED;
        RuntimeDataService.TryUpdateRuntimeDatum<Dictionary<string, string>>(StringDictifyQuestDict(questDict), questStatuses.FieldID);
    }
    private Dictionary<string, string> StringDictifyQuestDict(Dictionary<string, EnumQuestStatus> _questDict)
    {
        Dictionary<string, string> questStringDict = new();
        foreach(string questName in _questDict.Keys.ToList<string>())
        {
            questStringDict[questName] = _questDict[questName].ToString();
        }
        return questStringDict;
    }
    private bool TryGetQuestDictFromQuestStringDict(Dictionary<string, string> stringDict, out Dictionary<string, EnumQuestStatus> questDict)
    {
        questDict = new();

        foreach(string keyStr in stringDict.Keys.ToList<string>())
        {
            string valStr = stringDict[keyStr];
            if (!System.Enum.TryParse<EnumQuestStatus>(valStr, out EnumQuestStatus status)) return false;
            questDict[keyStr] = status;
        }
        return true;
    }

    private void DebugRuntimeData()
    {
        if (RuntimeDataService.TryGetRuntimeData(out RuntimeData data))
        {
            IReadOnlyDictionary<string, FieldNameAndType> nameTypeDict = data.FieldNamesAndTypesByID;
            IReadOnlyDictionary<string, IRuntimeDatum> valuesDict = data.Data;

            string debugStr = string.Empty;

            foreach (string id in data.FieldNamesAndTypesByID.Keys.ToList<string>())
            {
                string fieldName = nameTypeDict[id].fieldName;
                EnumFieldType fieldType = nameTypeDict[id].fieldType;
                IRuntimeDatum datum = valuesDict[id];

                string valStr = string.Empty;

                switch (fieldType)
                {
                    case EnumFieldType.STRING:
                        valStr = (datum as RuntimeDatum<string>).Value; break;
                    case EnumFieldType.INT:
                        valStr = ((datum as RuntimeDatum<int>).Value).ToString(); break;
                    case EnumFieldType.FLOAT:
                        valStr = ((datum as RuntimeDatum<float>).Value).ToString(); break;
                    case EnumFieldType.BOOL:
                        valStr = ((datum as RuntimeDatum<bool>).Value).ToString(); break;
                    case EnumFieldType.STRING_LIST:
                        valStr = StringifyList((datum as RuntimeDatum<List<string>>).Value); break;
                    case EnumFieldType.VECTOR_3:
                        valStr = StrigifyVector3((datum as RuntimeDatum<Vector3>).Value); break;
                    case EnumFieldType.VECTOR_2:
                        valStr = StrigifyVector2((datum as RuntimeDatum<Vector2>).Value); break;
                    case EnumFieldType.VECTOR_4:
                        valStr = StringifyVector4((datum as RuntimeDatum<Vector4>).Value); break;
                    case EnumFieldType.STRING_DICT:
                        valStr = StringifyStringDict((datum as RuntimeDatum<Dictionary<string, string>>).Value); break;

                }
                string datumStr = $"{fieldName} -- {fieldType.ToString()}: {valStr}\n";
                debugStr += datumStr;
            }
            Debug.Log(debugStr);
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

    private string StringifyStringDict(Dictionary<string, string> stringDict)
    {
        string outStr = string.Empty;
        foreach(string keyStr in stringDict.Keys.ToList<string>())
        {
            outStr += $"{keyStr}: {stringDict[keyStr]}, ";
        }
        return outStr;
    }

    private string StrigifyVector2(Vector2 vector2)
    {
        string outStr = $"({vector2.x}, {vector2.y})";
        return outStr;
    }
    private string StrigifyVector3(Vector3 vector3)
    {
        string outStr = $"({vector3.x}, {vector3.y}, {vector3.z})";
        return outStr;
    }
    private string StringifyVector4(Vector4 vector4)
    {
        string outStr = $"({vector4.w}, {vector4.x}, {vector4.y}, {vector4.z})";
        return outStr;
    }
}
