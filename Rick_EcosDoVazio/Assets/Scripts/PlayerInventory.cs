using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

// NOVAS VARIÁVEIS para rastrear o status de cada habilidade
    [Header("Status de Habilidades")]
    public bool canDoubleJump = false;
    public bool canShoot = false;
    public bool canRoll = false;
    
    [Header("Audio Manager")]
    public AudioManager audioManager;
    // Funções públicas para desbloquear cada habilidade separadamente
    public void UnlockDoubleJump()
    {
        if (!canDoubleJump)
        {
            canDoubleJump = true;
            Debug.Log("Habilidade desbloqueada: Salto Duplo!");
        }
    }

    public void UnlockShoot()
    {
        if (!canShoot)
        {
            canShoot = true;
            Debug.Log("Habilidade desbloqueada: Tiro!");
        }
    }
    
    public void UnlockRoll()
    {
        if (!canRoll)
        {
            canRoll = true;
            Debug.Log("Habilidade desbloqueada: Rolagem!");
        }
    }
    // Variável para rastrear se o jogador tem a chave.
    public bool hasKey = false;
    
    // Função pública para pegar a chave.
    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("Chave coletada!");
        audioManager.PlaySFX(audioManager.keyCollectSFX);
        
    }

    // Função para usar a chave (se necessário, para consumo)
    public bool UseKey()
    {
        if (hasKey)
        {
            hasKey = false;
            Debug.Log("Chave usada.");
            return true;
        }
        return false;
    }
}