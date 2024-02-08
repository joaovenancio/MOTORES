using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public string AudioGroupName;
    public Slider Slider;

    //private void OnEnable()
    //{
    //    if (Slider != null)
    //        SetSliderFunction();
    //}

    //private void OnDisable()
    //{
    //    if (Slider != null)
    //        Slider.onValueChanged.RemoveAllListeners();
    //}

    public void SetSliderFunction()
    {
        this.Slider.onValueChanged.AddListener(delegate { SliderFunction(); });
    }

    public void SliderFunction ()
    {
        if (AudioGroupName != null &&
            Slider != null)
        {
            AudioManager.Instance.ChangeVolume(this.Slider.value, AudioGroupName);
        }
    }

}
