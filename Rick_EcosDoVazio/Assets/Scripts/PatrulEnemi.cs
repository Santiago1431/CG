using UnityEngine;
using System.Collections;

public class PatrulEnemi : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 10;

    public int maxLifeEnemy;
    public int currentLifeEnemy;
    protected SpriteRenderer spriteRenderer;
    private Animator animator;



    private bool Dead = false;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLifeEnemy = maxLifeEnemy;
        animator = GetComponent<Animator>();

    }

    void Update()
    {

        move();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PatrolLimit"))
        {
            flip();
        }

        PlayerLife playerLife = other.GetComponentInParent<PlayerLife>();
        if (playerLife != null)
        {
            playerLife.TakeDamage(1); // Causa 1 ponto de dano ao jogador
            flip();
        }
    }
    void move()
    {
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

    }



    public void DamageEnemy(int damageBullet)
    {
        currentLifeEnemy -= damageBullet;
        StartCoroutine(Damage());
        if (currentLifeEnemy <= 0)
        {
            Dead = true;
            animator.SetBool("Dead", Dead);
            waitingDeath();
        }
    }

    public void flip()
    {
        speed = -speed;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }






    //IEnumerator to make the enemy flash when damaged
    IEnumerator Damage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    

    void waitingDeath()
    {
        Destroy(gameObject, 0.5f);
    }

}