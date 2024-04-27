using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ValueChange : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public bool isTimeStopped = false;
    public void Start()
    {
        valueChange();
    }
    public void valueChange()
    {
        Time.timeScale = GetComponent<Slider>().value;
        textMeshPro.text = GetComponent<Slider>().value.ToString();
    }

    public void StopTime()
    {
        if(isTimeStopped)
        {
            valueChange();
        }
        else
        {
            Time.timeScale = 0;
        }

        isTimeStopped = !isTimeStopped;
    }
}
