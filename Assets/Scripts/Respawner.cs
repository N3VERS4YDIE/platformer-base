using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] private string targetTag;
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            other.transform.position = spawnPoint.position;
        }
    }
}
