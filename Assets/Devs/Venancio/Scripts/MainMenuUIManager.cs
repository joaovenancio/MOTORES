using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private Button loadGameButton;
    [SerializeField]
    private IGameDataManager gameDataManager;

    // Start is called before the first frame update
    void Start()
    {
        if (gameDataManager.CanLoadAGame())
        {
            loadGameButton.interactable = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
