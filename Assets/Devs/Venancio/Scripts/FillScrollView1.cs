using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillScrollView1 : MonoBehaviour
{
    [SerializeField]
    private GameObject _sliderPrefab;
    [SerializeField]
    private Transform _sliderContainer;
    [SerializeField]

    private void Start()
    {
        foreach (AudioGroup audioGroup in AudioManager.Instance.AudioGroups)
        {

            GameObject newGameObject = Instantiate(_sliderPrefab, _sliderContainer);
            //Debug.Log(newGameObject.GetComponent<Slider>() == null);

            SoundSlider soundSlider = newGameObject.AddComponent<SoundSlider>();
            soundSlider.AudioGroupName = audioGroup.Name;
            soundSlider.Slider = newGameObject.GetComponent<Slider>();
            soundSlider.Slider.value = audioGroup.Volume;

            soundSlider.SetSliderFunction();
        }
    }
}
