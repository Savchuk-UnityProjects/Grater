using System.Collections;
using UnityEngine;

public class ShakableObject : MonoBehaviour
{
    public void Shake(float Duration, float Magnitude, float Noize)
    {
        StartCoroutine(ShakingCoroutine(Duration, Magnitude, Noize));
    }

    private IEnumerator ShakingCoroutine(float Duration, float Magnitude, float Noize)
    {
        float Elapsed = 0f;
        Vector3 startPosition = transform.localPosition;
        Vector2 NoizeStartPoint0 = Random.insideUnitCircle * Noize;
        Vector2 NoizeStartPoint1 = Random.insideUnitCircle * Noize;
        while (Elapsed < Duration)
        {
            Vector2 CurrentNoizePoint0 = Vector2.Lerp(NoizeStartPoint0, Vector2.zero, Elapsed / Duration);
            Vector2 CurrentNoizePoint1 = Vector2.Lerp(NoizeStartPoint1, Vector2.zero, Elapsed / Duration);
            Vector2 PostionDelta = new Vector2(Mathf.PerlinNoise(CurrentNoizePoint0.x, CurrentNoizePoint0.y), Mathf.PerlinNoise(CurrentNoizePoint1.x, CurrentNoizePoint1.y));
            PostionDelta *= Magnitude;
            transform.localPosition = startPosition + (Vector3)PostionDelta;
            Elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPosition;
    }
}