using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatInspectorModule : MonoBehaviour
{
    [SerializeField] private Slider valueSlider;
    [SerializeField] private TMP_Text statName, StatValue;

    public void DisplayValue(string name ,float value, float maxValue = 200)
    {
        valueSlider.maxValue = maxValue;
        valueSlider.value = value;
        StatValue.text = value.ToString("0");
        statName.text = name;
    }
}
