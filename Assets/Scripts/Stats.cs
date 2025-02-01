using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float health;
    [SerializeField] public float maxHealth = 100;
    public TMP_Text hpText;
    private Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.position;
        UpdateHp();
    }

    public void TakeDamage(float damage)
    {
        health += damage;
        UpdateHp();

        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            health = 1;
            Death();
        }
    }

    void Death()
    {
        transform.position = initialPosition;
        UpdateHp(); 
    }


    void UpdateHp()
    {
        hpText.text = "Health: " + health;
    }

}
