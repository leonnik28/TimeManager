using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TimeModel
{
    private DateTimeOffset _currentTime;
    private readonly string _timeUrl = "https://yandex.com/time/sync.json";

    public event Action<DateTimeOffset> OnTimeUpdated;

    public async void StartTimeSync()
    {
        await GetTimeFromServer();
        await CheckTimeEveryHour();
    }

    private async Task GetTimeFromServer()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_timeUrl))
        {
            var asyncOp = webRequest.SendWebRequest();
            while (!asyncOp.isDone)
            {
                await Task.Yield();
            }

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
        OnTimeUpdated?.Invoke(_currentTime);
        _ = UpdateTime();
    }

    private async Task UpdateTime()
    {
        while (true)
        {
            _currentTime = _currentTime.AddSeconds(1);
            OnTimeUpdated?.Invoke(_currentTime);
            await Task.Delay(1000);
        }
    }

    private async Task CheckTimeEveryHour()
    {
        while (true)
        {
            await Task.Delay(3600000);
            await GetTimeFromServer();
        }
    }

    [System.Serializable]
    private class TimeData
    {
        public long time;
    }
}
