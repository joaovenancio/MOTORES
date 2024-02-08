using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationSelectSceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject _cityButton;
    [SerializeField]
    private GameObject _unknownIndustryButton;

    private Dictionary<string, GameCondition> _conditionsDictionary;

    public void OnChapterOne(Dictionary<string, GameCondition> conditionsDictionary)
    {
        AudioManager.Instance.PlayBackgroundMusic("Journey");

        _conditionsDictionary = conditionsDictionary;

        _cityButton.SetActive(false);
        _unknownIndustryButton.SetActive(false);
    }
}
