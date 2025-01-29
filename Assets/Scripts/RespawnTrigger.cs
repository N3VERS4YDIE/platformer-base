using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private string[] includeTags;
    [SerializeField] private string[] excludeTags;
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in excludeTags)
        {
            if (other.CompareTag(tag))
            {
                return;
            }
        }

        if (includeTags.Length == 0)
        {
            other.transform.position = spawnPoint.position;
            return;
        }

        foreach (string tag in includeTags)
        {
            if (other.CompareTag(tag))
            {
                other.transform.position = spawnPoint.position;
                return;
            }
        }
    }
}
