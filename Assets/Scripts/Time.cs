using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Time : MonoBehaviour
{
    [SerializeField] private Text _timeText;
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
            yield return new WaitForSeconds(1);
        }
    }

    [System.Serializable]
    private class TimeData
    {
        public long time;
    }
}
