using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private int _ms;
    [SerializeField] private event UnityAction _callback = delegate { };

    public void SetTime(int ms)
    {
        _ms = ms;
    }

    public void SetCallBack(UnityAction callback)
    {
        _callback = callback;
    }

    public void Time(int ms, UnityAction callback)
    {
        StartCoroutine(_Time(ms / 1000, callback));
    }

    public void Time()
    {
        StartCoroutine(_Time(_ms / 1000, _callback));
    }

    private IEnumerator _Time(float sec, UnityAction callback)
    {
        yield return new WaitForSeconds(sec);
        callback.Invoke();
    }
}
