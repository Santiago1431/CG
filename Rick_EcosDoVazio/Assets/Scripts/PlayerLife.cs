using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PlayerLife : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int maxLife;
    public int currentLife;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    //Checkpoint variables
    public static bool checkpointReached = false; 
    public static Vector3 respawnPoint;

    private Rigidbody2D rb;


    void Start()
    {
        currentLife = maxLife;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>(); 

        // Se NENHUM checkpoint foi pego AINDA, o ponto de respawn
        // é a posição inicial do jogador.
        if (!checkpointReached)
        {
            respawnPoint = transform.position;
        }
    }


    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        StartCoroutine(FlashRed());
        if (currentLife <= 0)
        {
            // Verifica SE um checkpoint foi alcançado
            if (checkpointReached)
            {
                // **Respawn no Checkpoint**
                Debug.Log("Respawn no checkpoint!");
                
                // 1. Move o jogador para o ponto salvo
                transform.position = respawnPoint; 
                
                // 2. Restaura a vida
                currentLife = maxLife; 
                
                // 3. Para qualquer movimento que o jogador tinha (ex: caindo)
                rb.linearVelocity = Vector2.zero; 
                
                // 4. Garante que ele não esteja mais vermelho
                spriteRenderer.color = Color.white; 
            }
            else
            {
                // **Game Over**
                // Nenhum checkpoint foi salvo, então é Game Over.
                Debug.Log("Nenhum checkpoint. Game over.");
                SceneManager.LoadScene("EndGame");
            }
        }
    }
    
    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        // Garante que a cor volte ao normal mesmo se o jogador morrer
        if(currentLife > 0)
        {
            spriteRenderer.color = Color.white;
        }
    }
}
