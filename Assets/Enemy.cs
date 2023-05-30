using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public Enemy_Data enemy_data;
    internal Transform TR;
    [SerializeField] bool spotted_player = false;

    public int AmmoLeft = 60;
    public float FireRate = 1f;
    public float ShootTimeStamp = 0f;
    public bool canShootNow = true;
    public GameObject BulletPrefab;
    internal AudioSource ShootSFX;

    private void Awake()
    {
        ShootSFX = GetComponent<AudioSource>();
        TR = transform;
    }
    private void FixedUpdate()
    {
        Vector3 current_pos = transform.position;
        Vector3 added_range = new Vector3(0f, 0f, enemy_data.spotting_range);
        var hitColliders = Physics2D.OverlapAreaAll(current_pos - added_range, current_pos + added_range);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && hitColliders[i].gameObject.GetComponent<Player>() != null)
            {
                spotted_player = true;
            }
            else
            {
                spotted_player = false;
            }

        }
        if (spotted_player)
        {
            ShootTimer();
        }
    }
    void ShootTimer()
    {
        if (ShootTimeStamp < FireRate && !canShootNow)
        {
            ShootTimeStamp += Time.deltaTime;
        }
        else if (ShootTimeStamp >= FireRate && !canShootNow)
        {
            ShootTimeStamp = 0f;
            Shoot();
            canShootNow = true;
        }
    }
    public float ShootForce = 20f;
    void Shoot()
    {
        canShootNow = false;
        AmmoLeft--;
        ShootSFX.Play();
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = TR.position + (TR.right * 0.5f);
        bullet.GetComponent<Rigidbody2D>().velocity = ShootForce * TR.right;
    }
    #region IDamagable Stuff
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
        //no death animation, destroying for now
    }

    public void GivingDamage(int damage, IDamagable Receiver)
    {
        Debug.Log("done " + damage + "damage to " + Receiver.TR.name);
    }

    public void TakingDamage(int damage, IDamagable Attacker)
    {
        //play the hurt animation here
    }
    #endregion
}
