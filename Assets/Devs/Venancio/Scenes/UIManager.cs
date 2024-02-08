using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _soundOptions;
    [SerializeField]
    private GameObject _saveLoadOptions;

    private bool _isOpen = false;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isOpen)
            {
                CloseAll();
                _isOpen = false;
            } else
            {
                OpenSoundOptions();
                _isOpen = true;
            }
        }
    }

    public void OpenSoundOptions ()
    {
        _pauseMenu.SetActive(true);
        _soundOptions.SetActive(true);
        _saveLoadOptions.SetActive(false);
    }

    public void OpenSaveLoadOptions()
    {
        _pauseMenu.SetActive(true);
        _soundOptions.SetActive(false);
        _saveLoadOptions.SetActive(true);
    }

    public void CloseAll()
    {
        _pauseMenu.SetActive(false);
        _soundOptions.SetActive(false);
        _saveLoadOptions.SetActive(false);
    }
}
