using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeView : MonoBehaviour
{
    [SerializeField] private Text _timeText;
    [SerializeField] private Transform _hourHand;
    [SerializeField] private Transform _minuteHand;
    [SerializeField] private Transform _secondHand;

    public void UpdateTimeText(DateTimeOffset currentTime)
    {
        _timeText.text = currentTime.ToString("HH:mm:ss");
    }

    public void UpdateClockHands(DateTimeOffset currentTime)
    {
        float hours = (currentTime.Hour % 12) + (currentTime.Minute / 60f);
        float minutes = currentTime.Minute + (currentTime.Second / 60f);
        float seconds = currentTime.Second + (currentTime.Millisecond / 1000f);

        float targetHourRotation = -hours * 30;
        float targetMinuteRotation = -minutes * 6;
        float targetSecondRotation = -seconds * 6;

        _hourHand.DOLocalRotate(new Vector3(0, 0, targetHourRotation), 1f).SetEase(Ease.Linear);
        _minuteHand.DOLocalRotate(new Vector3(0, 0, targetMinuteRotation), 1f).SetEase(Ease.Linear);
        _secondHand.DOLocalRotate(new Vector3(0, 0, targetSecondRotation), 1f).SetEase(Ease.Linear);
    }
}
