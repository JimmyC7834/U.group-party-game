using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _sec;
    [SerializeField] private event UnityAction _callback = delegate { };

    public void SetMs(int ms)
    {
        _sec = (float)ms / 1000;
    }

    public void SetSec(float s)
    {
        _sec = s;
    }

    public void SetCallBack(UnityAction callback)
    {
        _callback = callback;
    }

    public void Time()
    {
        StartCoroutine(_Time(_sec, _callback));
    }

    public void Clear()
    {
        _callback = delegate { };
    }

    private IEnumerator _Time(float sec, UnityAction callback)
    {
        yield return new WaitForSeconds(sec);
        callback.Invoke();
    }
}
