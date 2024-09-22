using System;
using UnityEngine;

public class TimePresenter : MonoBehaviour
{
    [SerializeField] private TimeView _timeView;
    private TimeModel _timeModel;

    private void Awake()
    {
        _timeModel = new TimeModel();

        _timeModel.OnTimeUpdated += OnTimeUpdated;
        _timeModel.StartTimeSync();
    }

    private void OnTimeUpdated(DateTimeOffset currentTime)
    {
        _timeView.UpdateTimeText(currentTime);
        _timeView.UpdateClockHands(currentTime);
    }

    private void OnDestroy()
    {
        _timeModel.OnTimeUpdated -= OnTimeUpdated;
    }
}
