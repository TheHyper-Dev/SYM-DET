using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamagable
{
    internal Transform TR;
    public bool Facing_Right = true;
    public float ShootDuration = 0.25f;
    public float ShootTimeStamp = 0f;
    public bool canShootNow = true;
    void ShootTimer()
    {
        if (ShootTimeStamp < ShootDuration && !canShootNow)
        {
            ShootTimeStamp += Time.deltaTime;
        }
        else if (ShootTimeStamp >= ShootDuration && !canShootNow)
        {
            ShootTimeStamp = 0f;
            canShootNow = true;
        }
    }
    #region IDamagableStuff
    [SerializeField] int Health = 150;
    public int IHealth { get => Health; set => Health = value; }
    [SerializeField] int DefaultHealth = 150;
    public int IDefaultHealth { get => DefaultHealth; set => DefaultHealth = value; }
    public Action onDamageTaken { get; set; }
    public Action onDamageGiven { get; set; }
    public Action onDeath { get; set; }

    Transform IDamagable.TR => TR;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void GivingDamage(int damage, IDamagable Receiver)
    {

    }

    public void TakingDamage(int damage, IDamagable Attacker)
    {
        Debug.Log(((Transform)Attacker).name);
    }
    #endregion
    void Start()
    {
        TR = transform;
    }

    void Update()
    {
        if (GameManager.Instance.player.TR.position.x >= TR.position.x && !Facing_Right)
        {
            TR.Rotate(0f, 180f, 0f);
            Facing_Right = true;
        }
        else if (GameManager.Instance.player.TR.position.x < TR.position.x && Facing_Right)
        {
            TR.Rotate(0f, 180f, 0f);
            Facing_Right = false;
        }
        ShootTimer();
    }
}
