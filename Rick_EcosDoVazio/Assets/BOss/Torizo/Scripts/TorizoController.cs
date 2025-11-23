using UnityEngine;
using System.Collections;

public class TorizoController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float speed = 2f;
    public int vidaMaxima = 20;
    private int vidaAtual;

    private bool acordado = false;
    public bool morto = false;
    private bool atacando = false;

    public Transform pontoTiro;
    public GameObject projetilPrefab;
    public Transform player;

    [Header("Ataque Aleatório")]
    public float tempoMinEntreAtaques = 2f;
    public float tempoMaxEntreAtaques = 5f;
    private float proximoAtaqueEm;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        vidaAtual = vidaMaxima;
        SortearTempoAtaque();
    }

    void Update()
    {
        if (morto || !acordado) return;

        if (!atacando)
        {
            // patrulhar normalmente
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

            // contagem até o próximo ataque
            proximoAtaqueEm -= Time.deltaTime;
            if (proximoAtaqueEm <= 0)
            {
                StartCoroutine(PrepararAtaque());
            }
        }
    }

    IEnumerator PrepararAtaque()
    {
        atacando = true;
        rb.linearVelocity = Vector2.zero;

        // virar na direção do player
        if (player != null)
        {
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
                (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }
        }

        animator.SetBool("Atacar", true);

        // espera a duração da animação de preparação 
        yield return new WaitForSeconds(0.25f);

        // dispara o projétil no momento certo
        Atirar();

        // espera o final da animação
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Atacar", false);
        atacando = false;

        SortearTempoAtaque();
    }

    void Atirar()
    {
        if (pontoTiro == null || projetilPrefab == null) return;

        int tipoAtaque = Random.Range(0, 3); // 0=único, 1=rajada, 2=leque

        switch (tipoAtaque)
        {
            case 0:
                // Tiro único direto
                DispararLeque(1, 0f);
                break;

            case 1:

                // Rajada média (5 tiros, 0.1s entre eles)
                StartCoroutine(DispararRajada(5, 0f, 0.1f));
                break;
        }
    }

    IEnumerator DispararRajada(int quantidade, float anguloTotal, float atrasoEntreTiros)
    {
        float anguloInicial = -anguloTotal / 2f;

        for (int i = 0; i < quantidade; i++)
        {
            float angulo = anguloInicial + (anguloTotal / (quantidade - 1)) * i;
            Quaternion rotacao = pontoTiro.rotation * Quaternion.Euler(0, 0, angulo);
            Instantiate(projetilPrefab, pontoTiro.position, rotacao);
            yield return new WaitForSeconds(atrasoEntreTiros);
        }
    }

    void DispararLeque(int quantidadeTiros, float anguloTotal)
    {
        if (quantidadeTiros <= 0) return;

        if (quantidadeTiros == 1)
        {
            // Tiro único direto
            Instantiate(projetilPrefab, pontoTiro.position, pontoTiro.rotation);
            return;
        }

        // Rajada curta em ângulo leve (usado só se quiser variar a direção)
        float anguloInicial = -anguloTotal / 2f;
        for (int i = 0; i < quantidadeTiros; i++)
        {
            float angulo = anguloInicial + (anguloTotal / (quantidadeTiros - 1)) * i;
            Quaternion rotacao = pontoTiro.rotation * Quaternion.Euler(0, 0, angulo);
            Instantiate(projetilPrefab, pontoTiro.position, rotacao);
        }
    }



    private void SortearTempoAtaque()
    {
        proximoAtaqueEm = Random.Range(tempoMinEntreAtaques, tempoMaxEntreAtaques);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PatrolLimit"))
        {
            Flip();
        }

        PlayerLife playerLife = other.GetComponentInParent<PlayerLife>();
        if (playerLife != null)
        {
            playerLife.TakeDamage(1);
            Flip();
        }
    }

    public void Acordar()
    {
        if (acordado || morto) return;
        StartCoroutine(AcordarDelay());
    }

    IEnumerator AcordarDelay()
    {
        animator.SetBool("Acordado", true);
        yield return new WaitForSeconds(2.31f); // duração da animação "acordando"
        acordado = true;
    }

    public void LevarDano(int dano, string parte)
    {
        if (!acordado)
        {
            Acordar();
            return;
        }

        if (parte != "BossWeakPoint") return;

        vidaAtual -= dano;
        StartCoroutine(FeedbackDano());

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    IEnumerator FeedbackDano()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Morrer()
    {
        morto = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("Morto", true);
        Destroy(gameObject, 1.5f);
        //ativa passagem para proxima fase
    }

    void Flip()
    {
        speed = -speed;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
