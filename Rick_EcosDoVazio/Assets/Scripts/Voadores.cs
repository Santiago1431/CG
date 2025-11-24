using UnityEngine;
using System.Collections; // Necessário para o IEnumerator

public class FlyingEnemyChase : MonoBehaviour
{
    // --- Configurações de Movimento e Perseguição ---
    private Rigidbody2D rb;
    public float speed = 5f; // Velocidade de perseguição

    public float chaseRange = 8f; // Distância para começar a perseguir
    public float stopRange = 1f;  // Distância para parar (e talvez atacar)

    private Transform player; // Referência ao Transform do jogador

    // --- Configurações de Vida e Animação ---
    public int maxLifeEnemy = 3;
    private int currentLifeEnemy;
    protected SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isChasing = false;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentLifeEnemy = maxLifeEnemy;

        // Tenta encontrar o objeto do jogador na cena pela tag "Player".
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player com a tag 'Player' não encontrado! O inimigo não perseguirá.");
        }
    }

    void Update()
    {
        if (player == null || isDead) return;

        // 1. Calcula a distância até o jogador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 2. Lógica de Perseguição
        if (distanceToPlayer <= chaseRange && distanceToPlayer > stopRange)
        {
            // Começa ou continua a perseguir
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            // Fora do alcance ou muito perto
            isChasing = false;
            StopMovement();
        }

        // 3. Orientação do Sprite
        if (isChasing)
        {
            Flip(player.position.x);
        }
    }

    // --- Funções de Movimento ---
    void ChasePlayer()
    {
        // Calcula a direção normalizada (direção unitária) para o jogador
        Vector2 direction = (player.position - transform.position).normalized;

        // Aplica a velocidade usando Rigidbody2D
        // Usamos rb.velocity em vez de rb.linearVelocity, que é a sintaxe correta no Unity.
        rb.linearVelocity = direction * speed;
    }

    void StopMovement()
    {
        // Para o movimento do Rigidbody
        rb.linearVelocity = Vector2.zero;
    }

    // --- Lógica de Colisão e Dano ---
    private void OnTriggerStay2D(Collider2D other)
    {
        // Verifica se colidiu/está sobre o jogador
        PlayerLife playerLife = other.GetComponentInParent<PlayerLife>();
        if (playerLife != null)
        {
            // Causa dano no jogador
            playerLife.TakeDamage(1); 
        }
    }

    public void DamageEnemy(int damageBullet)
    {
        if (isDead) return;

        currentLifeEnemy -= damageBullet;
        StartCoroutine(DamageFlash());

        if (currentLifeEnemy <= 0)
        {
            isDead = true;
            animator.SetBool("Dead", isDead);
            // Parar o movimento imediatamente
            StopMovement(); 
            // Destrói o objeto após a animação de morte (0.5s)
            Destroy(gameObject, 0.5f); 
        }
    }

    // --- Funções de Utilidade ---
    public void Flip(float targetX)
    {
        Vector3 scale = transform.localScale;
        
        // Se o alvo (jogador) estiver à direita, a escala X deve ser positiva.
        if (targetX > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        // Se o alvo estiver à esquerda, a escala X deve ser negativa.
        else if (targetX < transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        
        transform.localScale = scale;
    }

    // IEnumerator para fazer o inimigo piscar quando danificado
    IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    
    // Opcional: Desenha os raios de perseguição no Scene View para visualização.
    private void OnDrawGizmosSelected()
    {
        // Raio de Perseguição (Vermelho)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        // Raio de Parada/Ataque (Verde)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}