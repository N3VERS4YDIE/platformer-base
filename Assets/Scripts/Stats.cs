using UnityEngine;
using System.Collections;
using System;

public abstract class Stats : MonoBehaviour
{
    public event Action<short> OnHealthChanged;


    [Header("Health")]
    [SerializeField] protected short maxHealth;
    [SerializeField] protected float invulnerabilityDuration;

    [Header("Blinking Effect")]
    [SerializeField] protected byte blinkTimes;
    [SerializeField] protected Color blinkColor = Color.red;

    protected SpriteRenderer spriteRenderer;
    protected short health;
    protected bool isInvulnerable = false;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    public virtual void ChangeHealth(short healthDelta, bool ignoreInvulnerability = false)
    {
        if (healthDelta < 0 && isInvulnerable && !ignoreInvulnerability)
        {
            return;
        }

        health += healthDelta;
        health = (short)Mathf.Clamp(health, 0, maxHealth);

        OnHealthChanged?.Invoke(health);

        if (healthDelta < 0 && !ignoreInvulnerability)
        {
            StartCoroutine(InvulnerabilityCooldown());
        }

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;
        float blinkInterval = invulnerabilityDuration / (blinkTimes * 2);
        Color initialColor = spriteRenderer.color;
        spriteRenderer.enabled = true;
        spriteRenderer.color = blinkColor;

        for (byte i = 0; i < blinkTimes * 2; ++i)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        spriteRenderer.color = initialColor;
        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    protected abstract void Die();
}
