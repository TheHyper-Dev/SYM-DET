using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject owner;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out var receiver))
        {
            receiver.doDamage(36, owner.GetComponent<IDamagable>());
        }
        Destroy(gameObject);
    }
}
