using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float health;
  

    void Start()
    {
        health = maxHealth;
    }

    public virtual void Heal(float heal) {
        health += heal;
        health = Mathf.Max(health, maxHealth);
    }

    public virtual void FullyHeal()
    {
        Heal(maxHealth);
    }

    public virtual void Hit(float damage, MonoBehaviour attacker) {

        if (PollingStation.Get<TeamsManager>().AreInTheSameTeam(gameObject, attacker.gameObject)) return;

        health -= damage;
        health = Mathf.Max(health, 0);

        if (health == 0.0f)
            OnDeath(attacker);

       // Debug.Log($"{gameObject.name} took {damage} damage!");
    }

    public virtual void OnDeath(MonoBehaviour attacker) {

    }
}
