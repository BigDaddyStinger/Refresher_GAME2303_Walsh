using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class HazardDamage : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";

    [SerializeField] int damageOnEnter = 10;
    [SerializeField] int damagePerTick = 5;
    [SerializeField] float tickIntervalSeconds = 2f;

    [SerializeField] PlayerStats playerStats;

    public UnityEvent OnEntered;
    public UnityEvent OnExited;
    public UnityEvent<int> OnDamageApplied;

    Coroutine tickRoutine;
    bool playerInside;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (!playerStats) { Debug.LogWarning("[HazardDamage] PlayerStats not assigned."); return; }

        playerInside = true;

        playerStats.TakeDamage(damageOnEnter);
        OnDamageApplied?.Invoke(damageOnEnter);

        if (tickRoutine != null) StopCoroutine(tickRoutine);
        tickRoutine = StartCoroutine(DamageTicker());

        OnEntered?.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInside = false;

        if (tickRoutine != null) StopCoroutine(tickRoutine);
        tickRoutine = null;

        OnExited?.Invoke();
    }

    IEnumerator DamageTicker()
    {
        var wait = new WaitForSeconds(tickIntervalSeconds);
        while (playerInside)
        {
            yield return wait;

            if (!playerStats) yield break;

            playerStats.TakeDamage(damagePerTick);

            OnDamageApplied?.Invoke(damagePerTick);
        }
    }
}