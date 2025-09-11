using System;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;

    [SerializeField] Image healthBar;

    float maxHealth;

    void Awake()
    {
        if (healthBar)
        {
            healthBar.type = Image.Type.Filled;
            healthBar.fillMethod = Image.FillMethod.Horizontal;
            healthBar.fillOrigin = 0;
        }
    }

    void Start()
    {
        maxHealth = playerStats.MaxHealth;

        RefreshBar(playerStats.CurrentHealth);
    }

    void Update()
    {
        RefreshBar(playerStats.CurrentHealth);
    }

    void RefreshBar(int current)
    {
        if (!healthBar) return;
        float ratio = (maxHealth <= 0) ? 0f : (float)current / maxHealth;
        healthBar.fillAmount = Mathf.Clamp01(ratio);
    }
}