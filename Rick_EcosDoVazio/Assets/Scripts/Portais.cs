using UnityEngine;

public class Portal : MonoBehaviour
{
    // ... (Variáveis de portal)
    [Tooltip("O GameObject Portal de DESTINO (para onde o player será teletransportado).")]
    public Portal destinationPortal;
    
    [Tooltip("A tag que o objeto de colisão deve ter (geralmente 'Player').")]
    public string playerTag = "Player"; 

    [Tooltip("Offset para mover o player para longe do ponto de colisão após o teletransporte.")]
    public Vector3 exitOffset = new Vector3(0.5f, 0f, 0f);

    // Se a colisão ocorrer (o player entra no trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // MODIFICAÇÃO CHAVE: Busca a Transform do objeto RAIZ (pai) do colisor que entrou no portal.
        Transform playerRootTransform = other.transform.root;

        // 1. Verifica se o objeto RAIZ do colisor é o jogador (checa a Tag).
        if (playerRootTransform.CompareTag(playerTag))
        {
            // 2. Verifica se o portal de destino foi definido (para evitar erros).
            if (destinationPortal == null)
            {
                Debug.LogError("O Portal de Destino não está configurado para este portal: " + gameObject.name);
                return;
            }

            // 3. Teletransporta o player usando a Transform do objeto RAIZ.
            Teleport(playerRootTransform);
        }
    }

    private void Teleport(Transform player)
    {
        // Pega a posição do portal de destino
        Vector3 destinationPos = destinationPortal.transform.position;

        // Se o player estiver virado para a esquerda (escala X negativa), 
        // inverte o offset para que ele saia na direção correta.
        Vector3 finalOffset = exitOffset;
        if (player.localScale.x < 0)
        {
            finalOffset = new Vector3(-exitOffset.x, exitOffset.y, exitOffset.z);
        }

        // Move o player para a posição do portal de destino, 
        // adicionando um pequeno deslocamento (offset) para ele não ficar preso no colisor.
        player.position = destinationPos + finalOffset;

        // Opcional: Se for um Rigidbody, você pode querer zerar a velocidade ou
        // mantê-la. Se o player estiver pulando, talvez você queira que ele mantenha a velocidade Y.
        // O GetComponent<Rigidbody2D>() agora é chamado no objeto ROOT correto.
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Tenta manter a velocidade horizontal, mas zera o movimento vertical 
            // (para evitar que ele pule alto demais após o teletransporte).
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
        
        Debug.Log(player.name + " teletransportado de " + gameObject.name + " para " + destinationPortal.name);
    }
}