using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClockHandsView : MonoBehaviour
{
    [SerializeField] private Transform _hourHand;
    [SerializeField] private Transform _minuteHand;
    [SerializeField] private Transform _secondHand;

    [SerializeField] private TimePresenter _timePresenter;

    private DateTimeOffset _newTime;

    private bool _isDragging;
    private Vector3 _initialMousePosition;
    private Vector3 _initialHandPosition;
    private Transform _currentHand;

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

    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData != null && _timePresenter.IsEditing &&
            (pointerEventData.pointerCurrentRaycast.gameObject == _hourHand.gameObject ||
             pointerEventData.pointerCurrentRaycast.gameObject == _minuteHand.gameObject ||
             pointerEventData.pointerCurrentRaycast.gameObject == _secondHand.gameObject))
        {
            _isDragging = true;
            _initialMousePosition = Input.mousePosition;
            _currentHand = pointerEventData.pointerCurrentRaycast.gameObject.transform;
            _initialHandPosition = _currentHand.localEulerAngles;
        }
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (_isDragging && pointerEventData != null)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = currentMousePosition.x - _initialMousePosition.x;
            float deltaY = currentMousePosition.y - _initialMousePosition.y;
            float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

            _currentHand.localEulerAngles = new Vector3(0, 0, _initialHandPosition.z + angle);

            UpdateTimeBasedOnHandRotation();
        }
    }

    public void StartClock()
    {
        _timePresenter.SetTimeManually(_newTime);
    }

    private void UpdateTimeBasedOnHandRotation()
    {
        float hourRotation = -_hourHand.localEulerAngles.z;
        float minuteRotation = -_minuteHand.localEulerAngles.z;
        float secondRotation = -_secondHand.localEulerAngles.z;

        int hours = Mathf.FloorToInt((hourRotation + 360) % 360 / 30);
        int minutes = Mathf.FloorToInt((minuteRotation + 360) % 360 / 6);
        int seconds = Mathf.FloorToInt((secondRotation + 360) % 360 / 6);

        hours = Mathf.Clamp(hours, 0, 23);
        minutes = Mathf.Clamp(minutes, 0, 59);
        seconds = Mathf.Clamp(seconds, 0, 59);

        DateTimeOffset newTime = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds, TimeSpan.Zero);
        _newTime = newTime;
    }

}
