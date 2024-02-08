using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private Button loadGameButton;
    [SerializeField]
    private DataManager _dataManager;

    // Start is called before the first frame update
    void Start()
    {
        _dataManager = DataManager.Instance;

        if (_dataManager.CanLoadGame())
        {
            loadGameButton.interactable = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
