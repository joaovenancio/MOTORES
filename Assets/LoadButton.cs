using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    public Button Button;


    // Start is called before the first frame update
    void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(() => DataManager.Instance.LoadGame());
    }
}
