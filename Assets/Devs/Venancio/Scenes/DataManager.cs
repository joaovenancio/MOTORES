using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    [SerializeField]
    [Serialize]
    private List<GameCondition> _conditions;
    public string Chapter;
    [SerializeField]
    private SaveManager _saveManager;

    public Dictionary<string, GameCondition> ConditionsDictionary = new Dictionary<string, GameCondition>();
    
    void Awake()
    {
        CreateSingleton();
        InitializeVariables();
    }

    void CreateSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void InitializeVariables ()
    {
        if (_saveManager.SaveData == null)
        {
            SaveData newSaveData = new SaveData();
            newSaveData.Chapter = Chapter;
            newSaveData.ListOfConditions = _conditions;
            
            _saveManager.SaveData = newSaveData;
        }
        _conditions = _saveManager.SaveData.ListOfConditions;
        Chapter = _saveManager.SaveData.Chapter;

        ConditionsDictionary = new Dictionary<string, GameCondition>();

        foreach (GameCondition gameCondition in _conditions)
        {
            ConditionsDictionary.Add(gameCondition.Name, gameCondition);
        }
    }

    public bool CanLoadGame()
    {
        _saveManager.Load();
        if (_saveManager.SaveData.Chapter != ""  || _saveManager.SaveData.Chapter != null &&
            _saveManager.SaveData.ListOfConditions != null)
        {
            return true;
        } else
        {
            return false;
        }

    }

    public void LoadGame()
    {
        _saveManager.Load();
        InitializeVariables();
        GameManager.Instance.ChangeScene("LocationSelect");
    }

    public void SaveGame()
    {
        _saveManager.SaveData.Chapter = Chapter;
        _saveManager.SaveData.ListOfConditions = _conditions;
        _saveManager.Save();
    }

    public void NewGame(string sceneName)
    {
        SaveData newSaveData = new SaveData();
        newSaveData.Chapter = "1";
        newSaveData.ListOfConditions = new List<GameCondition>();

        GameCondition gameCondition = new GameCondition();
        gameCondition.Name = "cityUnlocked";
        gameCondition.Value = false;
        newSaveData.ListOfConditions.Add(gameCondition);

        gameCondition = new GameCondition();
        gameCondition.Name = "industryUnlocked";
        gameCondition.Value = false;
        newSaveData.ListOfConditions.Add(gameCondition);

        _saveManager.SaveData = newSaveData;

        InitializeVariables();

        SceneManager.LoadScene(sceneName);
    }

    public GameCondition AddCondition (string conditionName, bool value)
    {
        GameCondition newGameCondition = new GameCondition();
        newGameCondition.Name = conditionName;
        newGameCondition.Value = value;

        if (!ConditionsDictionary.ContainsKey(conditionName))
        {
            _conditions.Add(newGameCondition);
            ConditionsDictionary.Add(conditionName, newGameCondition);

        } else
        {
            ConditionsDictionary[conditionName].Value = value;
        }

        return newGameCondition;
    }

    public bool GetCondition(string conditionName)
    {
        GameCondition condition;
        try
        {
            condition = ConditionsDictionary[conditionName];
        } catch (KeyNotFoundException exception)
        {
            condition = AddCondition(conditionName, false);
            Debug.Log("Data Manager: Could not find condition +" + conditionName +". Exception: " + exception.Message + ".Creating a new one...");
        }

        return condition.Value;
    }
}
