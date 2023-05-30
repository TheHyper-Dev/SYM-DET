using System;
using UnityEngine;

public interface IDamagable
{
    int IHealth { get; set; }
    int IDefaultHealth { get; set; }
    bool isAlive => IHealth > 0;

    Transform TR { get; }

    Action onDamageTaken { get; set; }
    Action onDamageGiven { get; set; }
    Action onDeath { get; set; }
    void doDamage(int damage, IDamagable Attacker = null)
    {

        if (IHealth <= 0) return;
        Attacker ??= this;

        IHealth -= damage;

        onDamageTaken?.Invoke();
        TakingDamage(damage, Attacker);

        Attacker.onDamageGiven?.Invoke();
        Attacker.GivingDamage(damage, this);

        Debug.Log($"{Attacker} has given {damage} damage to {this}");

        if (damage >= IHealth)
        {
            IHealth = 0;
            onDeath?.Invoke();
            Die();
            Debug.Log($"{this} got killed by {Attacker}");
        }

    }
    /// <summary>
    /// Called when the object got damaged down to zero health
    /// </summary>
    void Die();
    void TakingDamage(int damage, IDamagable Attacker);
    void GivingDamage(int damage, IDamagable Receiver);
}