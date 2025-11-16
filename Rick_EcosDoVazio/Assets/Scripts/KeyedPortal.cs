using UnityEngine;

public class KeyedPortal : Portal // Herda do seu Portal.cs
{
    [Header("Configuração de Chave")]
    [Tooltip("Se for verdadeiro, o portal precisa da chave 'hasKey' para funcionar.")]
    public bool requiresKey = true;
    
    [Tooltip("Se for verdadeiro, a chave será consumida (UseKey) após o uso.")]
    public bool consumesKey = true; 

    // Sobrescreve a lógica de colisão para incluir a verificação da chave.
    private new void OnTriggerEnter2D(Collider2D other)
    {
        Transform playerRootTransform = other.transform.root;

        // 1. Verifica se o objeto raiz é o jogador.
        if (playerRootTransform.CompareTag(playerTag))
        {
            PlayerInventory inventory = playerRootTransform.GetComponent<PlayerInventory>();

            // Se o portal não requer chave OU o jogador a possui
            if (!requiresKey || (inventory != null && inventory.hasKey))
            {
                // Se o portal consome a chave, chama UseKey() no inventário
                if (consumesKey && inventory != null)
                {
                    if (inventory.UseKey())
                    {
                        // Se a chave foi usada (consumida) com sucesso, teleporta.
                        Teleport(playerRootTransform);
                    }
                }
                else
                {
                    // Não consome chave ou não tem inventário, apenas teleporta.
                    Teleport(playerRootTransform);
                }
            }
            else if (requiresKey)
            {
                // NÃO TEM A CHAVE
                Debug.Log("Este portal está trancado. Você precisa da chave!");
                // Opcional: Adicione feedback visual ou sonoro de "trancado"
            }
        }
    }
    
    // O método Teleport (e as variáveis de destino) são herdadas do Portal.cs original.
    // Se você não quiser herdar, precisará copiar o conteúdo de Teleport() do Portal.cs para cá.

    private void Teleport(Transform player)
    {
        if (destinationPortal == null)
        {
            Debug.LogError("O Portal de Destino não está configurado para este portal: " + gameObject.name);
            return;
        }

        Vector3 destinationPos = destinationPortal.transform.position;
        Vector3 finalOffset = exitOffset;
        
        // Lógica de virar o offset se o player estiver virado para a esquerda.
        if (player.localScale.x < 0)
        {
            finalOffset = new Vector3(-exitOffset.x, exitOffset.y, exitOffset.z);
        }

        player.position = destinationPos + finalOffset;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }
}