using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Usa o GetComponentInParent para encontrar o script de inventário no objeto pai do colisor.
        PlayerInventory inventory = collision.GetComponentInParent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.CollectKey();
            // Desativa ou destrói o objeto da chave no mundo
            Destroy(gameObject);
        }
    }
}