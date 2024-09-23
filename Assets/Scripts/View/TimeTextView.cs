using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextView : MonoBehaviour
{
    [SerializeField] private Text _timeText;

    public void UpdateTimeText(DateTimeOffset currentTime)
    {
        _timeText.text = currentTime.ToString("HH:mm:ss");
    }
}
