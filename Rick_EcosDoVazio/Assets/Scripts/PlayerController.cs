using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rgb;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    public bool isOnGround;
    public LayerMask whatIsGround;

    public Animator animator;

    public BulletController shotToFire;
    public Transform shotPoint;

    private bool canDoubleJump;

    // Variável interna para rastrear o estado de mirar para cima
    private bool isAimingUp;
    public Transform shotPointUp; // <--- NOVO: Ponto de disparo para cima

    // --- Variáveis Novas para Rolagem ---
    [Header("Controles de Rolagem")]
    [Tooltip("O GameObject que representa o visual normal do jogador")]
    public GameObject normalPlayerModel;
   
    [Tooltip("O GameObject que representa o visual de 'bola' do jogador")]
    public GameObject ballPlayerModel;
   
    [Tooltip("Velocidade de movimento enquanto estiver rolando")]
    public float rollSpeed;

    [Tooltip("Força do pulo Rolando")]
    public float rollJumpForce;

    [Tooltip("O tempo máximo (em segundos) entre dois cliques para ser considerado um double-tap.")]
    public float doubleTapTimeWindow = 0.3f;

    // --- Variáveis Novas para Colisão ---
    [Header("Controles de Colisão")]
    [Tooltip("O colisor 2D usado no modo normal (Capsule, Box, etc)")]
    public Collider2D normalCollider;

    [Tooltip("O colisor 2D usado no modo de rolagem (Circle)")]
    public Collider2D rollCollider;

    // --- Variáveis privadas de controle ---
    private float timeOfLastPress = -1.0f; // Para o double-tap
    private bool isRolling = false; // Nossa variável de ESTADO

    public PlayerInventory inventory;

    
    // AudioManager 
    public AudioManager audioManager;

    
    void Start()
    {
        // Garante que o jogador comece no modo normal
        ExitRollMode();

        inventory = GetComponentInParent<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("PlayerInventory não encontrado no objeto pai ou neste objeto!");
        }
    }

    // Update is called once per frame
    void Update()
    {


        //// 1. Lógica de Mirar para Cima (Nova Funcionalidade)
        isAimingUp = Input.GetAxis("Vertical") > 0;


        // 1. CHECAGEM DE CHÃO 
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
        animator.SetBool("isOnGround", isOnGround);

        // 2. LÓGICA DE TRANSIÇÃO DE ESTADO
        // (Verifica se deve ENTRAR ou SAIR do modo de rolagem)

        bool canStartRoll = (inventory != null) ? inventory.canRoll : false;
        if (!isRolling)
        {
            // Se NÃO está rolando, checa por double-tap para baixo
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                float timeSinceLastPress = Time.time - timeOfLastPress;
                if (timeSinceLastPress <= doubleTapTimeWindow)
                {
                    if (canStartRoll)
                    {
                        // É um double-tap! Entra no modo de rolagem
                        EnterRollMode();
                    }
                }
                timeOfLastPress = Time.time;
            }
        }
        else // Se (isRolling == true)
        {
            // Se ESTÁ rolando, checa pela seta para cima para sair
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ExitRollMode();
            }
        }

        // 3. LÓGICA DE AÇÃO (Baseada no estado)
        // (Define o que o personagem faz em cada modo)

        float currentSpeed = 0f;

        if (isRolling)
        {
            // --- MODO ROLANDO ---

            // Usa a velocidade de rolagem (rollSpeed)
            currentSpeed = Input.GetAxis("Horizontal") * rollSpeed;
            rgb.linearVelocity = new Vector2(currentSpeed, rgb.linearVelocity.y);

            // Pulo

            if (Input.GetButtonDown("Jump") && isOnGround)
            {

                rgb.linearVelocity = new Vector2(rgb.linearVelocity.x, rollJumpForce);
                

            }
        }
        else
        {


            rgb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rgb.linearVelocity.y);

            if (rgb.linearVelocity.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (rgb.linearVelocity.x > 0)
                transform.localScale = Vector3.one;


            bool doubleJumpUnlocked = (inventory != null) ? inventory.canDoubleJump : false;
            if (Input.GetButtonDown("Jump") && (isOnGround || canDoubleJump && doubleJumpUnlocked))
            {
                if (isOnGround)
                {
                    canDoubleJump = true;
                }
                else
                {
                    canDoubleJump = false;
                    animator.SetTrigger("doubleJump");
                }

                rgb.linearVelocity = new Vector2(rgb.linearVelocity.x, jumpForce);
                audioManager.PlaySFX(audioManager.jumpSFX);
            }
        }

        // Shooting (LÓGICA MODIFICADA)
        bool shotUnlocked = (inventory != null) ? inventory.canShoot : false;

        if (Input.GetButtonDown("Fire1") && !isRolling && shotUnlocked)
        {
            Vector2 shootDirection = Vector2.zero;
            Transform currentShotPoint = shotPoint; // Ponto de disparo padrão (horizontal)

            if (isAimingUp && isOnGround)
            {
                // Se estiver mirando para cima, muda a direção e o ponto de origem.
                shootDirection = Vector2.up; // Equivalente a new Vector2(0f, 1f)

                if (shotPointUp != null && animator.GetBool("isAimingUp"))
                {
                    currentShotPoint = shotPointUp; // <--- MUDANÇA: Usa o ponto de tiro para cima
                    
                }
            }
            else
            {
                // TIRO HORIZONTAL!
                // Mantenha o tiro na direção que o personagem está virado (transform.localScale.x)
                shootDirection = new Vector2(transform.localScale.x, 0f);
            }

            // Instancia o tiro com a direção e o ponto de origem (novo) calculados
            Instantiate(shotToFire, currentShotPoint.position, currentShotPoint.rotation).moveDir = shootDirection;
            currentShotPoint.gameObject.GetComponent<AudioSource>().Play();
            animator.SetTrigger("shotFired");
        }

        // 4. ATUALIZAÇÃO DO ANIMATOR
        animator.SetBool("isAimingUp", isAimingUp);

        animator.SetFloat("speed", Mathf.Abs(currentSpeed));
        animator.SetBool("isOnGround", isOnGround);
        animator.SetFloat("speed", Mathf.Abs(rgb.linearVelocity.x));
    }



    private void EnterRollMode()
    {
        isRolling = true;

        // Troca os visuais
        if (normalPlayerModel != null) normalPlayerModel.SetActive(false);
        if (ballPlayerModel != null) ballPlayerModel.SetActive(true);

        // Troca os colisores
        if (normalCollider != null) normalCollider.enabled = false;
        if (rollCollider != null) rollCollider.enabled = true;

        // Atualiza o Animator
        animator.SetBool("isRolling", true);

        // Reseta o timer do double-tap
        timeOfLastPress = -1.0f;

        audioManager.PlaySFX(audioManager.rollSFX);

        Debug.Log("Entrou no modo ROLANDO!");
    }
    
    private void ExitRollMode()
    {
        isRolling = false;

        // Troca os visuais de volta
        //if (normalPlayerModel != null) normalPlayerModel.SetActive(true);
        //if (ballPlayerModel != null) ballPlayerModel.SetActive(false);

        // Troca os colisores de volta
        if (normalCollider != null) normalCollider.enabled = true;
        if (rollCollider != null) rollCollider.enabled = false;

        // Atualiza o Animator
        animator.SetBool("isRolling", false);

        Debug.Log("Voltou ao modo NORMAL!");
    }
}
