// BossController.cs
using System.Collections;
using UnityEngine;

// BossState.cs
public enum BossState
{
    Inactive,      // Boss esperando o primeiro contato (seu estado inicial)
    Immune,        // Spawna inimigos e não pode sofrer dano (parado/idle)
    Attacking,     // Persegue e ataca o jogador
    Dead           // Vida chegou a zero
}

public class BossController : MonoBehaviour
{
    // === Configurações e Referências ===

    public Rigidbody2D rb;

    [Header("Geral")]
    public BossState currentState = BossState.Inactive;
    public Animator animator;
    public Collider2D bossCollider;

    [Header("Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    public float moveSpeed = 3f;

    [Header("Ataque")]
    public Transform playerTransform; // Deve ser setado na Activation
    public float attackRange = 2f;
    public float damageAmount = 10f;
    public float attackCooldown = 1.5f;
    private bool canAttack = true;

    [Header("Imunidade")]
    public GameObject minionPrefab;
    public Transform[] spawnPoints;
    public int minionsToSpawn = 3;
    public float immuneDuration = 5f; // Duração mínima do estado Immune

    // ====================================================================

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        // Tenta obter referências se não foram setadas no Inspector
        if (animator == null)
            animator = GetComponent<Animator>();
        if (bossCollider == null)
            bossCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Garante que a lógica do Boss só rode se houver um jogador para perseguir
        if (playerTransform != null || currentState == BossState.Dead)
        {
            StateLogic();
        }
    }

    // --- MÁQUINA DE ESTADOS (FSM) ---

    private void StateLogic()
    {
        switch (currentState)
        {
            case BossState.Inactive:
                // Espera por OnTriggerEnter2D para mudar para Activation
                break;

            case BossState.Immune:
                HandleImmuneState();
                break;

            case BossState.Attacking:
                HandleAttackingState();
                break;

            case BossState.Dead:
                // O boss espera o fim da animação de morte para ser destruído
                break;
        }
    }

    // --- LÓGICAS DOS ESTADOS ---

    private void HandleImmuneState()
    {
        // Animação: Fica parado
        animator.SetBool("IsWalking", false);
        
        // **Nenhuma ação de perseguição/ataque**
        // A transição será feita pela corrotina de spawn
    }

    private void HandleAttackingState()
    {
        // 1. Perseguir o Jogador
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > attackRange)
        {
            // Movimentação
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            // Animação: Andando
            animator.SetBool("IsWalking", true);
        }
        else
        {
            // 2. Atacar
            animator.SetBool("IsWalking", false);
            if (canAttack)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        
        // Lógica de virar o sprite para a direção do movimento (opcional)
        // if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
        // else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
    
    private IEnumerator AttackRoutine()
    {
        canAttack = false;
        
        // Animação: Ataque
        animator.SetTrigger("Attack");
        
        // *** Dano ao Jogador é tipicamente aplicado através de um Animation Event ***
        // Ou em um pequeno delay:
        // yield return new WaitForSeconds(0.3f); // Espera o "hit" da animação
        // PlayerHealth.Instance.TakeDamage(damageAmount); 

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        
        //  Transição para Immune após X ataques ou cooldown

        if (Random.Range(0f, 1f) < 0.3f) // 30% de chance de mudar para Immune após o ataque
        {
            Debug.Log("Boss: Iniciando fase de Imunidade após ataque.");
            ChangeState(BossState.Immune);
            // Não precisamos fazer mais nada se o estado mudou
        }
    }


    // --- GESTÃO DE DANO ---

    public void TakeDamage(float damage)
    {
        // O Boss só sofre dano no estado Attacking
        if (currentState == BossState.Immune || currentState == BossState.Dead)
        {
            // O boss está imune ou morto
            return; 
        }
        
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            ChangeState(BossState.Dead);
        }
        else
        {
            // Animação de dano (
            animator.SetTrigger("Hit");
        }
    }

    // --- TRANSIÇÕES E GESTÃO DE ESTADOS ---

    public void ChangeState(BossState newState)
    {
        if (currentState == newState) return;

        // Lógica de Saída do Estado Anterior (Cleanup)
        switch (currentState)
        {
            case BossState.Immune:
                StopAllCoroutines(); // Para a corrotina de spawn, se ainda estiver rodando
                break;
        }

        currentState = newState;
        
        // Lógica de Entrada no Novo Estado (Setup)
        switch (newState)
        {
            /*case BossState.Activation:
                animator.SetTrigger("Activate"); 
                // A transição para Immune será tratada por um Animation Event
                break;*/
            
            case BossState.Immune:
                // Fica parado, invulnerável, e spawna
                // 1. Torna o corpo estático (desliga a física/gravidade)
                rb.bodyType = RigidbodyType2D.Static;
                bossCollider.enabled = true; 
                StartCoroutine(SpawnAndImmuneRoutine()); 
                break;

            case BossState.Attacking:
                //  Volta a aplicar a física (permite andar e ser afetado pela gravidade)
                rb.bodyType = RigidbodyType2D.Dynamic;
                bossCollider.enabled = true; // Volta a ser vulnerável
                canAttack = true;
                break;

            case BossState.Dead:
                animator.SetTrigger("Death");
                bossCollider.enabled = false; 
                // Inicia a rotina de destruição após a animação de morte
                StartCoroutine(DestroyAfterDeath());
                break;
        }
    }

    // --- CORROTINAS ESPECÍFICAS ---

    // Rotina para o estado Immune
    private IEnumerator SpawnAndImmuneRoutine()
    {
        Debug.Log("Boss: Entrando em estado Imune. Spawning Minions...");
        
        // 1. Spawna os inimigos
        for (int i = 0; i < minionsToSpawn; i++)
        {
            // Garante que há pontos de spawn
            if (spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(minionPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.5f); // Pequeno delay entre spawns
        }

        // 2. Mantém o estado Immune por um tempo
        yield return new WaitForSeconds(immuneDuration);
        
        Debug.Log("Boss: Saindo do estado Imune.");
        ChangeState(BossState.Attacking); // Volta ao ataque após o tempo
    }

    // Rotina para o estado Dead
    private IEnumerator DestroyAfterDeath()
    {
        // Espera o tempo da animação de morte (ajuste este valor)
        // Alternativamente, use um Animation Event no final da animação "Death"
        yield return new WaitForSeconds(3.0f); 
        
        // Destruição ou desativação do Boss
        Destroy(gameObject); 
    }

    // --- DETECÇÃO DE ATIVAÇÃO (Trigger) ---

    // Este deve ser um BoxCollider2D ou CircleCollider2D setado como **Is Trigger**
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se é o jogador e se o boss ainda está inativo
        Transform rootObject = other.transform.root;

        PlayerController player = rootObject.GetComponent<PlayerController>();

        if (currentState == BossState.Inactive && player != null)
        {
            Debug.Log("Boss: Jogador detectado. Ativando Boss...");
            playerTransform = rootObject;
            ChangeState(BossState.Attacking); // Muda diretamente para Attacking após o contato inicial
        }
    }
    
    // --- FUNÇÃO CHAMADA POR EVENTO DE ANIMAÇÃO (Unity Animator) ---

 
    // Adicione esta função como um Evento no frame de "Hit" da sua animação "Attack"
    public void DealDamageHitFrame()
    {
        // Lógica de verificação de proximidade e aplicação de dano, 
        // para que o dano seja aplicado exatamente quando o ataque acerta
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            // Você precisará de um script PlayerHealth acessível globalmente ou via GetComponent
            // Exemplo hipotético:
            // playerTransform.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        }
    }
}