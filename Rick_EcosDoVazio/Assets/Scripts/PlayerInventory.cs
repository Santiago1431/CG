using UnityEngine;
using System.Collections;

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


    [Header("UI Tutorial")]
    public TMPro.TMP_Text tutorialText;
    public float tutorialDuration = 4f;

    private void Start() {
        tutorialText.gameObject.SetActive(false);
    }
    private void ShowTutorial(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TutorialRoutine(message));
    }

    private IEnumerator TutorialRoutine(string message)
    {
        tutorialText.text = message;
        tutorialText.gameObject.SetActive(true);

        yield return new WaitForSeconds(tutorialDuration);

        tutorialText.gameObject.SetActive(false);
    }

    public void UnlockDoubleJump()
    {
        if (!canDoubleJump)
        {
            canDoubleJump = true;
            Debug.Log("Habilidade desbloqueada: Salto Duplo!");
            ShowTutorial("Novo Habilidade: SALTO DUPLO\nPressione ESPAÇO no ar para pular novamente!");
        }
    }

    public void UnlockShoot()
    {
        if (!canShoot)
        {
            canShoot = true;
            Debug.Log("Habilidade desbloqueada: Tiro!");
            ShowTutorial("Nova Habilidade: TIRO\nPressione Mouse1 para disparar!");
        }
    }

    public void UnlockRoll()
    {
        if (!canRoll)
        {
            canRoll = true;
            Debug.Log("Habilidade desbloqueada: Rolagem!");
            ShowTutorial("Nova Habilidade: ROLAGEM\nUse ↓ + ↓ para rolar !");
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