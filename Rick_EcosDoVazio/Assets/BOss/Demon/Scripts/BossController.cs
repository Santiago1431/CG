// BossController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossController : MonoBehaviour
{

    // Referência à barra de saúde do chefe
    public Slider healthBar;
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    public Collider2D attackRange;

    public GameObject hitboxAtaque;

    private float healthRecoveryCooldown = 1f;
    private float healthRecoveryTimer;

    public bool morto = false;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthBar.gameObject.SetActive(false);

        animator = GetComponent<Animator>();
    }

    private void Update() {
        healthBar.value = currentHealth;
        if (healthRecoveryTimer > 0)
        {
            healthRecoveryTimer -= Time.deltaTime;
        }

    }

    // Método para causar dano ao chefe
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        StartCoroutine(FeedbackDano());

        if(currentHealth == maxHealth - 1)
        {
            FirstShot();
        }

        if(currentHealth < maxHealth)
        {
            if (currentHealth <= 0)
            {
                Die();
                morto = true;
            }
        }

    }

    // Método para lidar com a morte do chefe
    private void Die()
    {
        // Lógica de morte do chefe
        Debug.Log("Chefe derrotado!");
        animator.SetBool("Death", true);
        //espera a animação de morte terminar antes de destruir o objeto

        Destroy(gameObject,1.42f);
    }

    //verifica o primeiro tiro para ativar a barra de vida
    public void FirstShot()
    {
        healthBar.gameObject.SetActive(true);
        animator.SetTrigger("Activate");
    }
    

    //caso o player esteja no range do ataque do boss ele ataca
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
        }
    }


    //recupera vida quando o boss está em idle cooldown
    public void RecoverHealth()
    {
        if(currentHealth < maxHealth && healthRecoveryTimer <= 0)
        {
            healthRecoveryTimer = healthRecoveryCooldown;
            currentHealth = Mathf.Min(currentHealth + 5, maxHealth); 
            healthBar.value = currentHealth;
        }
    }

    IEnumerator FeedbackDano()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void AtivarHitbox()
    {
        hitboxAtaque.SetActive(true);
    }

    public void DesativarHitbox()
    {
        hitboxAtaque.SetActive(false);
    }
}