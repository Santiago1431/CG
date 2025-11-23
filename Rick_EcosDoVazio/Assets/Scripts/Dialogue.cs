using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public Sprite profile;
    public string[] speechText;
    public string actorName;

    [Header("Settings")]
    private bool wasInteracted = false; // Flag para uso único

    private DialogueControl dc;

    private void Start()
    {
        // Certifique-se de que o objeto que contém este script
        // tem um Collider2D com 'Is Trigger' marcado.
        dc = FindObjectOfType<DialogueControl>();
    }

    // Acionado quando outro Collider2D entra no trigger deste objeto
    private void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController player = other.GetComponentInParent<PlayerController>();

        // Verifica se quem colidiu é o Player e se a interação ainda não ocorreu
        if (player != null && !wasInteracted)
        {
            wasInteracted = true; // Marca como interagido
            
            // Inicia o diálogo
            dc.Speech(profile, speechText, actorName);
            
            // Desativa este script para garantir que não haja mais interações de colisão,
            // ou pode-se desabilitar o próprio Collider2D se preferir.
            // enabled = false; 
        }
    }
    
    // Remova ou ignore o código de FixedUpdate, Update, Interact e OnDrawGizmosSelected
    // do seu script original, pois não são mais necessários para a lógica de colisão.
}