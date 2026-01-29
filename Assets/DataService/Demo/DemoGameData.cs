using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BradsDataService;


// Defines the actual data model for this particular game, and serves up
// current values from the RuntimeDataService

public enum EnumQuestStatus
{
    NOT_STARTED,
    STARTED,
    FINISHED,
}


public class DemoGameData : MonoBehaviour
{

    

    [SerializeField] private FieldSO playerNameField;           // string
    [SerializeField] private FieldSO playerStatusEffectField;   // List<string>
    [SerializeField] private FieldSO playerPositionField;       // Vector3
    [SerializeField] private FieldSO playerHealthField;         // int
    [SerializeField] private FieldSO secondsPlayedField;        // float
    [SerializeField] private FieldSO playerMapPositionField;         // Vector2
    [SerializeField] private FieldSO playerColorField;               // Vector4
    [SerializeField] private FieldSO questStatusesField;             // Dictionary<string, string>
    [SerializeField] private FieldSO playerOnboardedField;           // bool

    private string playerName
    {
        get
        {
            if (RuntimeDataService.TryGetRuntimeValue<string>(playerNameField.FieldID, out string pName))
            {
                return pName;
            }
            return string.Empty;
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<string>(value, playerNameField.FieldID);
        }
    }
    private int playerHealth
    {
        get
        {
            if (RuntimeDataService.TryGetRuntimeValue<int>(playerHealthField.FieldID, out int pHealth))
            {
                return pHealth;
            }
            return -1;
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<int>(value, playerHealthField.FieldID);
        }
    }
    private float secondsPlayed
    {
        get
        {
            if(RuntimeDataService.TryGetRuntimeValue<float>(secondsPlayedField.FieldID, out float sPlayed))
            {
                return sPlayed;
            }
            return Mathf.Epsilon;
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<float>(value, secondsPlayedField.FieldID);
        }
    }
    private Vector2 playerMapPos
    {
        get
        {
            if(RuntimeDataService.TryGetRuntimeValue<Vector2>(playerMapPositionField.FieldID, out Vector2 pos))
            {
                return pos;
            }
            return new();
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<Vector2>(value, playerMapPositionField.FieldID);
        }
    }
    private Vector3 playerPos
    {
        get
        {
            if(RuntimeDataService.TryGetRuntimeValue<Vector3>(playerPositionField.FieldID, out Vector3 pos))
            {
                return pos;
            }
            return new();
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<Vector3>(value, playerPositionField.FieldID);
        }
    }
    private Color playerColor
    {
        get
        {
            if(RuntimeDataService.TryGetRuntimeValue<Vector4>(playerColorField.FieldID, out Vector4 pColor))
            {
                return new Color(pColor.w, pColor.x, pColor.y, pColor.z);
            }
            return new();
        }
        set
        {
            Vector4 colorVector = new Vector4(value.r, value.g, value.b, value.a);
            RuntimeDataService.TryUpdateRuntimeDatum<Vector4>(colorVector, playerColorField.FieldID);
        }
    }
    private bool playerOnboarded
    {
        get
        {
            if(RuntimeDataService.TryGetRuntimeValue<bool>(playerOnboardedField.FieldID, out bool onboarded))
            {
                return onboarded;
            }
            return false;
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<bool>(value, playerOnboardedField.FieldID);
        }
    }
    private List<string> currentStatusEffects
    {
        get
        {
            if (RuntimeDataService.TryGetRuntimeValue<List<string>>(playerStatusEffectField.FieldID, out List<string> fxList))
            {
                return fxList;
            }
            return new();
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<List<string>>(value, playerStatusEffectField.FieldID);
        }
    }
    private Dictionary<string, EnumQuestStatus> currentQuestStatuses
    {
        get
        {
            if (RuntimeDataService.TryGetRuntimeValue<Dictionary<string, string>>(questStatusesField.FieldID, out Dictionary<string, string> stringDict))
            {
                if (TryGetQuestDictFromQuestStringDict(stringDict, out Dictionary<string, EnumQuestStatus> questDict))
                {
                    return questDict;
                }
            }
            return new();
        }
        set
        {
            RuntimeDataService.TryUpdateRuntimeDatum<Dictionary<string, string>>(GetStringDictFromQuestDict(value), questStatusesField.FieldID);
        }
    }

    public RuntimeData RuntimeData
    {
        get
        {
            if (RuntimeDataService.TryGetRuntimeData(out RuntimeData data))
            {
                return data;
            }
            return null;
        }
    }
    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }
    public int PlayerHealth
    {
        get => playerHealth;
        set => playerHealth = value;
    }
    public float SecondsPlayed
    {
        get => secondsPlayed;
        set => secondsPlayed = value;
    }
    public Vector2 PlayerMapPos
    {
        get => playerMapPos;
        set => playerMapPos = value;
    }
    public Vector3 PlayerPos
    {
        get => playerPos;
        set => playerPos = value;
    }
    public Color PlayerColor
    {
        get => playerColor;
        set => playerColor = value;
    }
    public bool PlayerOnboarded
    {
        get => playerOnboarded;
        set => playerOnboarded = value;
    }
    public List<string> CurrentStatusEffects
    {
        get => currentStatusEffects;
        set => currentStatusEffects = value;
    }
    public Dictionary<string, EnumQuestStatus> CurrentQuestStatuses
    {
        get => currentQuestStatuses;
        set => currentQuestStatuses = value;
    }

    public bool TryLoadNewRuntimeData(RuntimeData newData)
    {
        if (RuntimeDataService.TryLoadNewRuntimeData(newData))
        {
            return true;
        }
        return false;
    }
    public bool TryClearSaveData()
    {
        if (RuntimeDataService.TryClearSavedData())
        {
            return true;
        }
        return false;
    }

    

    // helpers //
    private bool TryGetQuestDictFromQuestStringDict(Dictionary<string, string> stringDict, out Dictionary<string, EnumQuestStatus> questDict)
    {
        questDict = new();

        foreach (string keyStr in stringDict.Keys.ToList<string>())
        {
            string valStr = stringDict[keyStr];
            if (!System.Enum.TryParse<EnumQuestStatus>(valStr, out EnumQuestStatus status)) return false;
            questDict[keyStr] = status;
        }
        return true;
    }
    private Dictionary<string, string> GetStringDictFromQuestDict(Dictionary<string,EnumQuestStatus> questDict)
    {
        Dictionary<string, string> stringDict = new();
        foreach(var kvp in questDict)
        {
            string keyStr = kvp.Key;
            string valStr = kvp.Value.ToString();
            stringDict[keyStr] = valStr;
        }
        return stringDict;
    }
}
