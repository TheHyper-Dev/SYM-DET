using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.doDamage(36, GameManager.Instance.player.GetComponent<IDamagable>());
        }
        Destroy(gameObject);
    }
}
