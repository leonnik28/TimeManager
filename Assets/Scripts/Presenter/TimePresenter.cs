using System;
using System.Threading.Tasks;
using UnityEngine;

public class TimePresenter : MonoBehaviour
{
    public bool IsEditing => _isEditing;

    [SerializeField] private ClockHandsView _clockHandsView;
    [SerializeField] private TimeTextView _timeTextView;
    private TimeModel _timeModel;

    private bool _isEditing;

    private void Start()
    {
        _timeModel = new TimeModel();

        _timeModel.OnTimeUpdated += OnTimeUpdated;
        _timeModel.StartTimeSync();
    }

    private void OnDestroy()
    {
        if (_timeModel != null)
        {
            _timeModel.OnTimeUpdated -= OnTimeUpdated;
        }
    }

    public void StopTimer()
    {
        _isEditing = true; 
        _timeModel.StopTimer();
    }

    public void SetTimeManually(DateTimeOffset time)
    {
        _isEditing = false;
        _timeModel.SetTimeManually(time);
    }

    public void UpdateTime()
    {
        _isEditing = false;
        _timeModel.SetTimeFromServer();
    }

    private void OnTimeUpdated(DateTimeOffset currentTime)
    {
        _timeTextView.UpdateTimeText(currentTime);
        _clockHandsView.UpdateClockHands(currentTime);
    }
}
