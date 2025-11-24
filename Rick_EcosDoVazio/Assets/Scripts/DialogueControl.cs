using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueControl : MonoBehaviour
{
    [Header("components")]
    public GameObject dialogueObj;
    public Image profile;
    public Text speechText;
    public Text actorNameText;
    
    // Componente do Player para bloquear o movimento
    [Header("Player components")]
    // Substitua 'PlayerMovement' pelo nome real do script de movimento do seu jogador
    public PlayerController playerMovementScript; 
    private Rigidbody2D playerRigidbody;
    public GameObject playerAnimator;
    private Animator playerAnimatorComponent;

    [Header("settings")]
    public float typingSpeed;
    private string[] sentences;
    private int index;

    private void Awake()
    {
        // Tenta encontrar o script de movimento do jogador no início da cena
        // Assumindo que o jogador tem a tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRigidbody = player.GetComponentInParent<Rigidbody2D>();
            
            playerMovementScript = player.GetComponentInParent<PlayerController>();
            if (playerMovementScript == null)
            {
                
                Debug.LogError("PlayerMovement script not found on the Player object!");
            }
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene!");
        }
    }

    public void Speech(Sprite p, string[] txt, string actorName)
    {
        // 1. BLOQUEAR MOVIMENTO DO JOGADOR
        if (playerMovementScript != null && playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = new Vector2(0f, playerRigidbody.linearVelocity.y);
            playerMovementScript.enabled = false; // Para o movimento atual
            playerAnimatorComponent = playerAnimator.GetComponent<Animator>();
            playerAnimatorComponent.SetFloat("speed", 0f); // Define o estado de corrida como falso
        }

        dialogueObj.SetActive(true);
        profile.sprite = p;
        sentences = txt;
        actorNameText.text = actorName;
        index = 0; // Garante que o índice começa do primeiro texto
        speechText.text = ""; // Garante que o texto está limpo
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        // ... (seu código atual do TypeSentence permanece o mesmo)
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
       if (speechText.text == sentences[index])
       {
            //ainda há textos
            if (index < sentences.Length - 1)
            {
                index++;
                speechText.text = "";
                StartCoroutine(TypeSentence());
            }
            //lido quando acaba os textos
            else
            {
                speechText.text = "";
                index = 0;
                dialogueObj.SetActive(false);
                
                // 2. DESBLOQUEAR MOVIMENTO DO JOGADOR
                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = true;
                }
            }
       }
       // Adiciona lógica para pular o texto se ainda estiver digitando
       else
       {
           StopAllCoroutines();
           speechText.text = sentences[index];
       }
    }
}