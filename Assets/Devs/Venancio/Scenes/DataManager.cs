using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    [SerializeField]
    private Data _data;

    public Dictionary<string, GameCondition> Conditions = new Dictionary<string, GameCondition>();
    
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
        foreach (GameCondition gameCondition in _data.Conditions)
        {
            Conditions.Add(gameCondition.Name, gameCondition);
        }
    }

}
