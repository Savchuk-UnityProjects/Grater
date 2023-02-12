using UnityEngine;

public class DestroyingWhenBecomesInvisible : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}