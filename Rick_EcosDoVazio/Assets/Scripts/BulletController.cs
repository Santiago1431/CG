using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D theRB;

    public Vector2 moveDir;

    public GameObject impactEffect;

    public int damageBullet;

    void Update()
    {
        theRB.linearVelocity = moveDir * bulletSpeed;
        damageBullet = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);

        if (other.CompareTag("Enemy"))
        {
            PatrulEnemi enemy = other.GetComponent<PatrulEnemi>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damageBullet);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
