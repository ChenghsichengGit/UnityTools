using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float baseHealth;
    [field: SerializeField, DisplayAsString] public float maxHealth { get; private set; }
    [SerializeField, DisplayAsString] private float healthAdd;
    [field: SerializeField, DisplayAsString] public float health { get; private set; }

    private event Action OnTakeDamegeEvent;
    private event Action OnHealEvent;
    private event Action OnModifyHealthEvent;
    private event Action OnDieEvent;

    void Start()
    {
        maxHealth = baseHealth + (baseHealth * healthAdd);
        health = maxHealth;
    }

    public void ModifyHealth(float value)
    {
        health = Mathf.Max(0, health + value);

        OnModifyHealthEvent?.Invoke();

        if (value < 0)
            OnTakeDamegeEvent?.Invoke();
        else if (value > 0)
            OnHealEvent?.Invoke();

        if (health > 0)
            return;

        OnDieEvent?.Invoke();
    }

    [Button]
    public void ModifyHealthAdd(float value)
    {
        healthAdd += value;
        maxHealth = baseHealth + (baseHealth * healthAdd);
        health += baseHealth * value;
    }
}
