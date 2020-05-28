using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetValue(float v)
    {
        slider.value = v;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
