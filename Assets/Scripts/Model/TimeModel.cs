using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TimeModel
{
    public event Action<DateTimeOffset> OnTimeUpdated;

    private DateTimeOffset _currentTime;
    private bool _isPlaying = true;

    private readonly string _timeUrl = "https://yandex.com/time/sync.json";

    public async void StartTimeSync()
    {
        await GetTimeFromServer();
        await UpdateTime();
    }

    public async void SetTimeManually(DateTimeOffset time)
    {
        _currentTime = time;
        _isPlaying = true;
        OnTimeUpdated?.Invoke(_currentTime);
        await UpdateTime();
    }

    public async void SetTimeFromServer()
    {
        _isPlaying = true;
        await GetTimeFromServer();
        await UpdateTime();
    }

    public void StopTimer()
    {
        _isPlaying = false;
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
    }

    private async Task UpdateTime()
    {
        while (true)
        {
            if (!_isPlaying)
            {
                break;
            }
            _currentTime = _currentTime.AddSeconds(1);
            OnTimeUpdated?.Invoke(_currentTime);
            await Task.Delay(1000);

            if (_currentTime.Minute == 0 && _currentTime.Second == 0)
            {
                await GetTimeFromServer();
            }
        }
    }

    [System.Serializable]
    private class TimeData
    {
        public long time;
    }
}
