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

    private Dictionary<string, GameCondition> _conditionsDictionary = DataManager.Instance.ConditionsDictionary;

    public void OnChapterOne(Dictionary<string, GameCondition> conditionsDictionary)
    {
        AudioManager.Instance.PlayBackgroundMusic("Journey");

        Debug.Log(_conditionsDictionary.Count);
        if (!_conditionsDictionary["cityUnlocked"].Value) _cityButton.SetActive(false);
        if (!_conditionsDictionary["industryUnlocked"].Value) _unknownIndustryButton.SetActive(false);
        
    }
}
