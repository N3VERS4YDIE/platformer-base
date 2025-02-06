using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Stats
{
    [SerializeField] TMP_Text healthText;

    protected override void Awake()
    {
        base.Awake();
        OnHealthChanged += UpdateHealthText;
        UpdateHealthText(health);
    }

    protected override void Die()
    {
        transform.parent = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateHealthText(short currentHealth)
    {
        healthText.text = $"Health: {currentHealth}";
    }
}
