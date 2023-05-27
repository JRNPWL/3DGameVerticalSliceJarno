using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tutorial
// https://www.youtube.com/watch?v=9A9yj8KnM8c&ab_channel=Brackeys

public class CameraShake : MonoBehaviour
{

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orginalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, orginalPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = orginalPos;
    }
}
