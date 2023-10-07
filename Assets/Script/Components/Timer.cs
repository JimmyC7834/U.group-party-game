using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private bool _paused = false;
    [SerializeField] private bool _loop = false;
    private event UnityAction _callback = delegate { };

    private float _timer = 0;
    private bool _started = false;

    public bool IsPaused => _paused;
    public bool Loop => _loop;
    public bool Stopped => !_started;
    public float TimeLeft => _timer;

    public void SetTime(float sec)
    {
        _time = sec;
    }

    public void SetPaused(bool value)
    {
        _paused = value;
    }

    public void SetLoop(bool value)
    {
        _loop = value;
    }

    public void Stop()
    {
        _started = false;
    }

    public void SetCallBack(UnityAction callback)
    {
        _callback = callback;
    }

    public void Start()
    {
        _started = true;
        _timer = _time;
    }

    public void ClearCallback()
    {
        _callback = delegate { };
    }

    private void FixedUpdate()
    {
        if (!_started) return;
        if (_paused) return;

        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            _timer = 0;
            _started = false;
            _callback.Invoke();
            if (_loop)
                Start();
        }
    }
}