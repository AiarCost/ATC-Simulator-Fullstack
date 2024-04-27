using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeCounter : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float TimeSpeedMultiplier = 1f;
    public float elapsedTime = 0f;
    public int Timer = 0;

    void Update()
    {
        // Increment elapsed time
        elapsedTime += Time.deltaTime * TimeSpeedMultiplier;

        // Calculate hours and minutes
        int hours = Mathf.FloorToInt(elapsedTime / 60);
        int minutes = Mathf.FloorToInt((elapsedTime % 60));

        // Update UI text
        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);

        hours *= 100;
        Timer = hours + minutes;
    }
}
