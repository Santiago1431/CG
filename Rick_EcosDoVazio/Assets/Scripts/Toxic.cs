using UnityEngine;

public class Toxic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLife playerLife = collision.GetComponentInParent<PlayerLife>();
        if (playerLife != null)
        {
            playerLife.TakeDamage(5); // Causa 5 pontos de dano ao jogador
        }
    }
}
