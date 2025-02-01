using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            Stats stats = other.GetComponent<Stats>();
            if(stats != null)
            {
                stats.TakeDamage(damage);
            }
            
        }
    }
}
