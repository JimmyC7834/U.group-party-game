using System.Collections;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
    [SerializeField] private Transform transformToShake = default;
    [SerializeField] private float shakeRadius = default;
    [SerializeField] private Vector3 originalPosition;

    public void StartAnimation()
    {
        StartCoroutine(ShakeProcess(999f));
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
        transformToShake.position = originalPosition;
    }

    public void StartAnimationFor(float time)
    {
        StartCoroutine(ShakeProcess(time));
    }

    private IEnumerator ShakeProcess(float time)
    {
        float timer = time;
        originalPosition = transformToShake.position;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float a = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(0, shakeRadius);
            transformToShake.position = originalPosition + new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * r;
            yield return null;
        }

        transformToShake.position = originalPosition;
    }
}
