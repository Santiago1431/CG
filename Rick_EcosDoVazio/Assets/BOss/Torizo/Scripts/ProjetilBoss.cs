using UnityEngine;

public class ProjetilBoss : MonoBehaviour
{
    public float velocidade = 6f;
    public int dano = 1;
    public float tempoVida = 5f;

    public GameObject impactEffect; // efeito visual do impacto
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Calcula direção até o jogador no momento do disparo
        Vector2 direcao = (player.position - transform.position).normalized;
        rb.linearVelocity = direcao * velocidade;

        Destroy(gameObject, tempoVida); // destrói se não colidir
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisão com: " + other.name);

        // Efeito visual
        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        // Se atingir o player
        PlayerLife vida = other.GetComponentInParent<PlayerLife>();
        if (vida != null)
        {
            Debug.Log("Dano aplicado ao Player!");
            vida.TakeDamage(dano);
            Destroy(gameObject);
            return;
        }
        // Destrói o projétil em qualquer colisão
        Destroy(gameObject);
    }

}
