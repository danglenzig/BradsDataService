using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using BradsDataService;
using BradsEvents;
using TMPro;

public static class StatusEffects
{
    public const string HUNGRY = "HUNGRY";
    public const string ANGRY = "ANGRY";
    public const string HORNY = "HORNY";
    public const string HORNGRY = "HORNGRY";
    public static List<string> FX = new List<string> { HUNGRY, ANGRY, HUNGRY, HORNGRY };
}

public class Quests
{
    public const string QUEST_1 = "Do a little dance";
    public const string QUEST_2 = "Make a little love";
    public const string QUEST_3 = "Get down tonight";
}

public class DemoSceneController : MonoBehaviour
{

    [SerializeField] private DemoGameData gameData;
    [SerializeField] private Button randomizeDataButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button debugButton;
    [SerializeField] private TMP_Text outputText;

    [SerializeField] private EmptyEventChannel dataSavedEvent;
    [SerializeField] private EmptyEventChannel dataLoadedEvent;
    [SerializeField] private EmptyEventChannel dataClearedEvent;

    private List<string> statusFX = new();
    private Dictionary<string, EnumQuestStatus> questStatusDict = new();


    private void OnEnable()
    {
        FixButtons();

        randomizeDataButton.onClick.AddListener(OnTestButtonPressed);
        saveButton.onClick.AddListener(OnSavePressed);
        loadButton.onClick.AddListener(OnLoadPressed);
        clearButton.onClick.AddListener(OnClearPressed);
        debugButton.onClick.AddListener(OnDebugPressed);

        dataSavedEvent.OnEventTriggered += OnDataSaved;
        dataClearedEvent.OnEventTriggered += OnDataCleared;
        

    }
    private void OnDisable()
    {
        randomizeDataButton.onClick.RemoveAllListeners();
        saveButton.onClick.RemoveAllListeners();
        loadButton.onClick.RemoveAllListeners();
        clearButton.onClick.RemoveAllListeners();
        debugButton.onClick.RemoveAllListeners();

        dataSavedEvent.OnEventTriggered -= OnDataSaved;
        dataClearedEvent.OnEventTriggered -= OnDataCleared;
    }

    private void Start()
    {
        if (SingleSaveService.SaveExists())
        {
            OnLoadPressed();
            return;
        }
        InitializeGameData();
    }

    private void OnTestButtonPressed()
    {
        RandomizeGameData();
    }

    private void OnSavePressed()
    {
        if(gameData.RuntimeData != null)
        {
            if (SingleSaveService.TrySave(gameData.RuntimeData))
            {
                dataSavedEvent.TriggerEvent();
            }
        }
    }

    private void OnLoadPressed()
    {
        // get saved data from disk
        if(SingleSaveService.TryLoad(out RuntimeData data))
        {
            if (gameData.TryLoadNewRuntimeData(data))
            {
                dataLoadedEvent.TriggerEvent();
            }
        }
        FixButtons();
    }

    private void OnClearPressed()
    {
        if (gameData.TryClearSaveData())
        {
            dataClearedEvent.TriggerEvent();
        }
    }

    private void OnDebugPressed()
    {
        DebugGameData();
    }

    private void OnDataSaved()
    {
        FixButtons();
    }
    private void OnDataCleared()
    {
        InitializeGameData();
        FixButtons();
    }

    private void InitializeGameData()
    {
        gameData.PlayerName = "Namey Nameson";
        gameData.PlayerPos = new Vector3(1f, 2f, 3f);
        gameData.PlayerHealth = 100;
        gameData.SecondsPlayed = 0;
        gameData.PlayerMapPos = new Vector2(100f, 200f);
        gameData.PlayerColor = Color.aliceBlue;
        gameData.PlayerOnboarded = false;

        gameData.CurrentStatusEffects = statusFX;

        questStatusDict[Quests.QUEST_1] = EnumQuestStatus.NOT_STARTED;
        questStatusDict[Quests.QUEST_2] = EnumQuestStatus.NOT_STARTED;
        questStatusDict[Quests.QUEST_3] = EnumQuestStatus.NOT_STARTED;
        gameData.CurrentQuestStatuses = questStatusDict;
    }


    private void RandomizeGameData()
    {
        // for demo purposes
        gameData.PlayerName = System.Guid.NewGuid().ToString(); // whatever
        
        float randoFloat = UnityEngine.Random.Range(0f, 1f);
        gameData.PlayerPos = new Vector3(randoFloat, randoFloat, randoFloat) * 10f;
        gameData.PlayerMapPos = new Vector2(randoFloat, randoFloat) * 100f;

        gameData.PlayerColor = new Color(randoFloat, randoFloat, randoFloat, randoFloat);

        int randoInt = UnityEngine.Random.Range(1, 100);
        gameData.PlayerHealth = randoInt;
        gameData.PlayerOnboarded = randoInt % 2 == 0;
        int randoFxIdx_1 = randoInt % StatusEffects.FX.Count;
        int randoFXIdx_2 = (randoFxIdx_1 + 1) % StatusEffects.FX.Count;
        gameData.CurrentStatusEffects = new List<string> { StatusEffects.FX[randoFxIdx_1], StatusEffects.FX[randoFXIdx_2] };
        gameData.SecondsPlayed = Time.time;

        int questStatusIdx = randoInt % 3;
        EnumQuestStatus randoStatus = (EnumQuestStatus)Enum.GetValues(typeof(EnumQuestStatus)).GetValue(questStatusIdx);

        Dictionary<string, EnumQuestStatus> newStatusDict = new Dictionary<string, EnumQuestStatus>(gameData.CurrentQuestStatuses);
        foreach(string keyStr in newStatusDict.Keys.ToList<string>())
        {
            newStatusDict[keyStr] = randoStatus;
        }
        gameData.CurrentQuestStatuses = newStatusDict;

        DebugGameData();

    }

    private void DebugGameData()
    {
        string debugStr = string.Empty;
        debugStr += $"Player name: {gameData.PlayerName}\n";
        debugStr += $"Player pos: {gameData.PlayerPos}\n";
        debugStr += $"Player map pos: {gameData.PlayerMapPos}\n";
        debugStr += $"Player health: {gameData.PlayerHealth}\n";
        debugStr += $"Seconds played: {gameData.SecondsPlayed}\n";
        debugStr += $"Player color: {gameData.PlayerColor}\n";
        debugStr += $"Player onboarded: {gameData.PlayerOnboarded}\n";
        debugStr += $"{StringifyFXList()}\n";
        debugStr += $"{StringifyQuestDict()}\n";
        //Debug.Log(debugStr);
        outputText.text = debugStr;
    }

    private string StringifyFXList()
    {
        string outStr = string.Empty;
        foreach(string fx in gameData.CurrentStatusEffects)
        {
            outStr += $"{fx}, ";
        }
        return outStr;
    }

    private string StringifyQuestDict()
    {
        string outStr = string.Empty;
        foreach(var kvp in gameData.CurrentQuestStatuses)
        {
            string keyStr = kvp.Key;
            string valStr = kvp.Value.ToString();
            outStr += $"{keyStr}: {valStr}, ";
        }
        return outStr;
    }

    private void FixButtons()
    {
        loadButton.gameObject.SetActive(SingleSaveService.SaveExists());
        clearButton.gameObject.SetActive(SingleSaveService.SaveExists());
        DebugGameData();
    }
}
