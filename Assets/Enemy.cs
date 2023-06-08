using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public Enemy_Data enemy_data;
    internal Transform TR;
    internal Rigidbody2D body;
    public Transform bullet_origin_TR;
    public int AmmoLeft = 60;
    public GameObject BulletPrefab;
    internal AudioSource ShootSFX;
    public bool stopped_shooting = true;
    public float attack_timestamp = 0f, attack_interval = 1f;
    private void Awake()
    {
        ShootSFX = GetComponent<AudioSource>();
        TR = transform;
        body = GetComponent<Rigidbody2D>();
    }
    float horizontal_direction_to_player;

    public bool facing_right = true;
    private void FixedUpdate()
    {
        Vector3 current_pos = TR.position;
        Vector3 vector_difference = current_pos - GameManager.Instance.player.TR.position;
        horizontal_direction_to_player = vector_difference.x > 0f ? -1f : 1f;

        if (stopped_shooting)
        {

            if (horizontal_direction_to_player < 0f && facing_right)
            {
                TR.Rotate(0f, 180f, 0f);
                facing_right = false;
            }
            else if (horizontal_direction_to_player > 0f && !facing_right)
            {
                TR.Rotate(0f, 180f, 0f);
                facing_right = true;
            }

            if (attack_timestamp < attack_interval)
            {
                attack_timestamp += Time.fixedDeltaTime;
            }
            else
            {
                Shoot();
            }
        }
    }
    public float ShootForce = 20f;
    void Shoot()
    {
        attack_timestamp = 0f;
        AmmoLeft--;
        ShootSFX.Play();
        var bullet = Instantiate(BulletPrefab);
        bullet.GetComponent<Bullet>().owner = gameObject;
        bullet.transform.position = bullet_origin_TR.position;
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
        if (name == "Katil")
        {
            GetComponent<Animation>().Play("katil_death");
            
        }
        else
        {
            Destroy(gameObject);
        }
        stopped_shooting = false;
        GameManager.Instance.messageBox.SetActive(true);
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
