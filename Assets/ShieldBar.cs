using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    public Slider slider;

    public void SetValue(int v)
    {
        slider.value = v;
    }
}
