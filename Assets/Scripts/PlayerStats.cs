using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    [SerializeField] UnityEvent onDamaged;
    [SerializeField] UnityEvent onHealed;
    [SerializeField] UnityEvent onDeath;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        onDamaged?.Invoke();

        if (currentHealth <= 0 )
        {
            currentHealth = 0;

            onDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        onHealed?.Invoke();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }






}
