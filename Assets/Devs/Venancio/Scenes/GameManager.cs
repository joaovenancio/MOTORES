using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameObjectoToPlaySound;
    [SerializeField]
    private SaveManager _saveManager;
    private Dictionary<string, GameCondition> _conditions = new Dictionary<string, GameCondition>();
    public UnityEvent<Dictionary<string, GameCondition>> OnChapterOne;
    public UnityEvent<Dictionary<string, GameCondition>> OnChapterTwo;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    protected void SingletonSetup()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Test()
    {
        AudioManager.Instance.Play("2", GameObjectoToPlaySound, "Biggies");
    }


    private void Start()
    {
        SingletonSetup();

        switch (DataManager.Instance.Chapter)
        {
            case "1":
                OnChapterOne.Invoke(_conditions);
                break;

            case "2":
                OnChapterTwo.Invoke(_conditions);
                break;
        }
    }

    public void ReloadScene ()
    {

    }

    
}
