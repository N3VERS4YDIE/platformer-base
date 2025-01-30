using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    
    [Header("Damage")]
    [SerializeField] private int damage;

    private Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.position;
    }

    void Death()
    {

        
        if (maxHealth <= 0)
        {
            maxHealth = 1;
            transform.position = initialPosition;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            maxHealth -= damage;
            Death();
        }
    }
}
