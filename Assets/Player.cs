using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    internal Transform TR;

    [Header("Locks")]
    public bool MovementEnabled = true;
    public bool LookEnabled = true;
    [Space]
    [Space]
    [SerializeField] internal Camera playerCamera;
    [SerializeField] internal PlayerUI UI;

    public Transform character_sprite;
    public Vector3 character_sprite_idle_offset, character_sprite_walking_offset;
    public Player_Data playerData;
    [SerializeField] float HorizontalMoveDirection;
    internal Rigidbody2D body;
    public Animator animator;
    Vector2 bodyVel;
    public bool Grounded = false;
    public bool Facing_Right = true;
    public List<Pickup> Inventory = new();
    [SerializeField] internal Transform GunHolder;
    [SerializeField] internal Gun gun;

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
        StartCoroutine(doDie());
    }
    IEnumerator doDie()
    {
        MovementEnabled = false;
        LookEnabled = false;
        GameManager.Instance.scene_anim.Play("scene_fade_out");
        animator.Play("joe_death");
        GunHolder.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GivingDamage(int damage, IDamagable Receiver)
    {
        Debug.Log("done " + damage + "damage to " + Receiver.TR.name);
    }

    public void TakingDamage(int damage, IDamagable Attacker)
    {

    }
    #endregion


    [SerializeField] bool started_walking_once = false;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        TR = GetComponent<Transform>();
    }

    void Update()
    {
        if (!MovementEnabled) return;
        HorizontalMoveDirection = Input.GetAxisRaw("Horizontal");

        if (HorizontalMoveDirection > 0f)
        {
            if (!Facing_Right)
            {
                TR.Rotate(0f, 180f, 0f);
                Facing_Right = true;
            }
            if (!started_walking_once)
            {
                character_sprite.localPosition = character_sprite_walking_offset;
                animator.SetBool("walking", true);
                started_walking_once = true;
            }
        }
        else if (HorizontalMoveDirection < 0f)
        {
            if (Facing_Right)
            {
                character_sprite.localPosition = character_sprite_walking_offset;
                TR.Rotate(0f, 180f, 0f);
                Facing_Right = false;
            }
            if (!started_walking_once)
            {
                character_sprite.localPosition = character_sprite_walking_offset;
                animator.SetBool("walking", true);
                started_walking_once = true;
            }
        }
        else if (started_walking_once)
        {
            character_sprite.localPosition = character_sprite_idle_offset;
            animator.SetBool("walking", false);
            started_walking_once = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            Grounded = false;
            body.AddRelativeForce(new Vector2(0f, playerData.jumpSpeed), ForceMode2D.Impulse);
            body.velocity += new Vector2(0f, playerData.jumpSpeed);
        }
    }
    void FixedUpdate()
    {
        bodyVel = body.velocity;
        bodyVel.x += HorizontalMoveDirection * playerData.Acceleration;
        bodyVel.x = Mathf.Clamp(bodyVel.x, -playerData.MoveSpeed, playerData.MoveSpeed);
        if (!Grounded)
        {
            bodyVel.y -= playerData.Gravity * Time.fixedDeltaTime;
        }
        body.velocity = bodyVel;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            Debug.Log("touched" + collision.gameObject.name + "with the angle " + collision.GetContact(i).normal);
            if (Mathf.Approximately(collision.GetContact(i).normal.y, 1f))
            {
                if (!Grounded)
                    Grounded = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Pickup>(out var pickup))
        {
            if (pickup.item is Health_Item health_Item)
            {
                if (Health <= DefaultHealth)
                    Health += health_Item.health;
            }
            else if (pickup.item is Ammo_Item ammo_Item)
            {
                gun.AmmoLeft += ammo_Item.Ammo;
            }
            Inventory.Add(pickup);
            Destroy(pickup.gameObject);
        }
    }
}
