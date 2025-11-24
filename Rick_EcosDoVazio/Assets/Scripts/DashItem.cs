using UnityEngine;

public class DashItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Busca o Inventário no objeto pai do colisor (o jogador).
        PlayerInventory inventory = collision.GetComponentInParent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.UnlockDash(); // Chama a função específica
            Destroy(gameObject);
        }
    }
}
