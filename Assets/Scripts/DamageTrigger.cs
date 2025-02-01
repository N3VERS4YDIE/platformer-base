using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Stats stats = other.GetComponent<Stats>();
            if (stats != null)
            {
                if (gameObject.CompareTag("Life"))
                {
                    stats.TakeDamage(damage);
                    Destroy(gameObject);
                }else
                {
                    stats.TakeDamage(damage);
                }
            }
        }
    }
}
