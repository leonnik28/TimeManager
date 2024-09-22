using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Time : MonoBehaviour
{
    [SerializeField] private Text _timeText;
    [SerializeField] private Transform _hourHand;
    [SerializeField] private Transform _minuteHand;
    [SerializeField] private Transform _secondHand;
    private DateTimeOffset _currentTime;

    private readonly string _timeUrl = "https://yandex.com/time/sync.json";

    private void Start()
    {
        StartCoroutine(GetTimeFromServer());
    }

    private IEnumerator GetTimeFromServer()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_timeUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                long time = JsonUtility.FromJson<TimeData>(jsonResponse).time;
                SetTime(time);
            }
        }
    }

    private void SetTime(long unixTime)
    {
        _currentTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).ToLocalTime();
        StartCoroutine(UpdateTime());
    }


    private IEnumerator UpdateTime()
    {
        while (true)
        {
            _currentTime = _currentTime.AddSeconds(1);
            _timeText.text = _currentTime.ToString("HH:mm:ss");
            UpdateClockHands();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateClockHands()
    {
        float hours = (_currentTime.Hour % 12) + (_currentTime.Minute / 60f);
        float minutes = _currentTime.Minute + (_currentTime.Second / 60f);
        float seconds = _currentTime.Second + (_currentTime.Millisecond / 1000f);

        float targetHourRotation = -hours * 30;
        float targetMinuteRotation = -minutes * 6;
        float targetSecondRotation = -seconds * 6;

        _hourHand.DOLocalRotate(new Vector3(0, 0, targetHourRotation), 1f).SetEase(Ease.Linear);
        _minuteHand.DOLocalRotate(new Vector3(0, 0, targetMinuteRotation), 1f).SetEase(Ease.Linear);
        _secondHand.DOLocalRotate(new Vector3(0, 0, targetSecondRotation), 1f).SetEase(Ease.Linear);
    }

    [System.Serializable]
    private class TimeData
    {
        public long time;
    }
}
