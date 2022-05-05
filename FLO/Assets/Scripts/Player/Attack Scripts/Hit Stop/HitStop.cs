using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool _waiting = false;

    public void Stop(float duration, float delay, float timeScale)
    {
        if (_waiting)
            return;
        StartCoroutine(Wait(duration, delay));
    }

    public void Stop(float duration, float delay)
    {
        Stop(duration, delay, 0.0f);
    }

    IEnumerator Wait(float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0.0f;
        _waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        _waiting = false;
    }
}