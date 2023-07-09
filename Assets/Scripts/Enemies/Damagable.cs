using System;
using UnityEngine;

public class Damagable : MonoBehaviour {

    public float baseMaxHealth = 100.0f;
    public float health;
    public float maxHealth { get; set; }

    public event Action<Damagable> healthUpdate;

    public virtual void Start()
    {
        maxHealth = baseMaxHealth;
        health = maxHealth;
        healthUpdate?.Invoke(this);
    }

    public virtual void Heal(float heal)
    {
        health += heal;
        health = Mathf.Min(health, maxHealth);
        healthUpdate?.Invoke(this);
    }

    public virtual void FullyHeal()
    {
        Heal(maxHealth);
    }

    public virtual void Hit(float damage, MonoBehaviour attacker)
    {

        if (PollingStation.Get<TeamsManager>().AreInTheSameTeam(gameObject, attacker.gameObject)) return;

        health -= damage;
        health = Mathf.Max(health, 0);
        healthUpdate?.Invoke(this);

        if (health == 0.0f)
            OnDeath(attacker);

        // Debug.Log($"{gameObject.name} took {damage} damage!");
    }

    public virtual void OnDeath(MonoBehaviour attacker)
    {

    }
}
