using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool checkpointAtivado = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerLife player = other.GetComponentInParent<PlayerLife>();
        // Verifica se é o jogador e se o checkpoint ainda não foi ativado
        if (player != null && !checkpointAtivado)
        {
            AtivarCheckpoint();
        }
    }

    private void AtivarCheckpoint()
    {
        Debug.Log("Checkpoint salvo na posição: " + transform.position);
        
        // 1. Marca como ativado para não salvar de novo
        checkpointAtivado = true; 
        
        // 2. Salva a posição deste checkpoint no script PlayerLife
        PlayerLife.checkpointReached = true;
        PlayerLife.respawnPoint = transform.position;
        
        // 3. Desativa o colisor para não verificar de novo
         GetComponent<Collider2D>().enabled = false;
    }
}