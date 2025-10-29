using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // --- Variáveis Originais de Movimento ---
    public Rigidbody2D rgb;
    public float moveSpeed;
    public float jumpForce;
    public Transform groundPoint;
    public bool isOnGround;
    public LayerMask whatIsGround;
    public Animator animator;

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


    // Start é chamado uma vez
    void Start()
    {
        // Garante que o jogador comece no modo normal
        ExitRollMode();
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // 1. CHECAGEM DE CHÃO (Acontece em ambos os modos)
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
        animator.SetBool("isOnGround", isOnGround);

        // 2. LÓGICA DE TRANSIÇÃO DE ESTADO
        // (Verifica se deve ENTRAR ou SAIR do modo de rolagem)
       
        if (!isRolling)
        {
            // Se NÃO está rolando, checa por double-tap para baixo
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                float timeSinceLastPress = Time.time - timeOfLastPress;
                if (timeSinceLastPress <= doubleTapTimeWindow)
                {
                    // É um double-tap! Entra no modo de rolagem
                    EnterRollMode();
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
            // --- MODO NORMAL ---
            // (Seu código de movimento original)

            // Usa a velocidade normal (moveSpeed)
            currentSpeed = Input.GetAxis("Horizontal") * moveSpeed;
            rgb.linearVelocity = new Vector2(currentSpeed, rgb.linearVelocity.y);

            // Pulo (só funciona no modo normal)
            if (Input.GetButtonDown("Jump") && isOnGround)
            {
                rgb.linearVelocity = new Vector2(rgb.linearVelocity.x, jumpForce);
            }
        }

        // 4. LÓGICA COMUM (Acontece em ambos os modos)

        // Flip (Seu código original)
        if (rgb.linearVelocity.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (rgb.linearVelocity.x > 0)
            transform.localScale = Vector3.one;

        // Animação de velocidade (Seu código original)
        animator.SetFloat("speed", Mathf.Abs(currentSpeed));
    }


    // --- FUNÇÕES DE CONTROLE DE ESTADO ---

    /// <summary>
    /// Ativa o modo de rolagem.
    /// </summary>
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

        Debug.Log("Entrou no modo ROLANDO!");
    }

    /// <summary>
    /// Desativa o modo de rolagem e volta ao normal.
    /// </summary>
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