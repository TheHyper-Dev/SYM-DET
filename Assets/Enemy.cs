using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public Enemy_Data enemy_data;
    internal Transform TR;
    internal Rigidbody2D body;
    [SerializeField] bool spotted_player = false;

    public float speed = 4f;
    public int AmmoLeft = 60;
    public float fire_timestamp = 0f, FireRate = 1f;
    public float ShootTimeStamp = 0f;
    public bool canShootNow = true;
    public GameObject BulletPrefab;
    internal AudioSource ShootSFX;
    public bool stopped_shooting = false;
    public float attack_timestamp = 0f, attack_interval = 1f;
    public float max_distance_to_player = 10f;
    public Vector2 gravity = new Vector2(0f, 5f);
    public enum State : byte { idle, running, attacking }
    public State state = State.idle;
    float total_attack_interval => attack_interval + FireRate + FireRate * 0.5f;
    private void Awake()
    {
        ShootSFX = GetComponent<AudioSource>();
        TR = transform;
        body = GetComponent<Rigidbody2D>();
        var temp_fire = FireRate + FireRate * 0.5f;
        if (attack_interval < temp_fire)
            attack_interval = temp_fire;
    }
    public Vector2 velocity = Vector2.zero;
    private void FixedUpdate()
    {
        Vector3 current_pos = TR.position;

        switch (state)
        {
            case State.idle:
                state = State.running;
                break;
            case State.running:
                Vector3 vector_difference = current_pos - GameManager.Instance.player.TR.position;


                if (attack_timestamp < attack_interval)
                {
                    attack_timestamp += Time.fixedDeltaTime;

                    if (vector_difference.x < max_distance_to_player)
                    {
                        velocity = -vector_difference.normalized * speed - new Vector3(gravity.x, gravity.y);
                    }
                    else
                    {
                        velocity = vector_difference.normalized * speed - new Vector3(gravity.x, gravity.y);
                    }
                }
                else
                {
                    attack_timestamp = 0f;
                    velocity.x = 0f;
                    state = State.attacking;
                }
                break;
            case State.attacking:
                {
                    velocity.y = -gravity.y;
                }
                break;
        }
        body.MovePosition(velocity * Time.fixedDeltaTime);
    }
    void decide_attack()
    {

    }

    public float ShootForce = 20f;
    void Shoot()
    {
        body.velocity = Vector3.zero;
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
