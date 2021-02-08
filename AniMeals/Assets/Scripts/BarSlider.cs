using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(float val) {
        slider.maxValue = val;
        slider.value = val;
    }

    public void SetCurrentVal(float val) {
        slider.value = val;
    }
}
