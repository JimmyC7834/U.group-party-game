using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimerTrigger : MonoBehaviour
{
    [SerializeField] private float timeInterval = default;
    public UnityEvent OnEarlyUpdate;
    public UnityEvent OnLateUpdate;

    private void OnEnable()
    {
        StartCoroutine(Timer(timeInterval));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Timer(float timeInterval)
    {
        if (!enabled)
            yield break;

        OnEarlyUpdate?.Invoke();
        yield return new WaitForSecondsRealtime(timeInterval);
        OnLateUpdate?.Invoke();
        StartCoroutine(Timer(timeInterval));
    }

    public void SetTimeInterval(float sec)
    {
        timeInterval = sec;
    }
}