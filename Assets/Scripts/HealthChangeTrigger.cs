using UnityEngine;

public class HealthChangeTrigger : MonoBehaviour
{
    [SerializeField] string[] targetTags;
    [SerializeField] short healthDelta;
    [SerializeField] bool destroyOnTrigger;
    [SerializeField] bool ignoreInvulnerability;

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var tag in targetTags)
        {
            if (other.CompareTag(tag) && other.TryGetComponent<Stats>(out var stats))
            {
                stats.ChangeHealth(healthDelta, ignoreInvulnerability);

                if (destroyOnTrigger)
                {
                    Destroy(gameObject);
                }

                return;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        foreach (var tag in targetTags)
        {
            if (other.CompareTag(tag) && other.TryGetComponent<Stats>(out var stats))
            {
                stats.ChangeHealth(healthDelta, ignoreInvulnerability);
                return;
            }
        }
    }
}
