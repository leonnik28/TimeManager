using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeEditView : MonoBehaviour
{
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _updateButton;
    [SerializeField] private InputField _timeInputField;

    [SerializeField] private ClockHandsView _clockHandsView;
    [SerializeField] private TimePresenter _timePresenter;

    private void Start()
    {
        _editButton.onClick.AddListener(() => ToggleEditMode(true));
        _saveButton.onClick.AddListener(SaveTimeText);
        _startButton.onClick.AddListener(SaveTimeClock);
        _updateButton.onClick.AddListener(UpdateTime);
        _saveButton.gameObject.SetActive(false);
        _startButton.gameObject.SetActive(false);
        _timeInputField.gameObject.SetActive(false);
        _updateButton.gameObject.SetActive(false);
    }

    private void ToggleEditMode(bool isEdit)
    {
        if (isEdit)
        {
            _timePresenter.StopTimer();
        }
        _saveButton.gameObject.SetActive(isEdit);
        _startButton.gameObject.SetActive(isEdit);
        _timeInputField.gameObject.SetActive(isEdit);
        _updateButton.gameObject.SetActive(isEdit);
    }

    private void SaveTimeClock()
    {
        _clockHandsView.StartClock();
        ToggleEditMode(false);
    }

    private void SaveTimeText()
    {
        if (DateTimeOffset.TryParse(_timeInputField.text, out DateTimeOffset newTime))
        {
            _timePresenter.SetTimeManually(newTime);
        }
        ToggleEditMode(false);
    }

    private void UpdateTime()
    {
        _timePresenter.UpdateTime();
        ToggleEditMode(false);
    }
}
