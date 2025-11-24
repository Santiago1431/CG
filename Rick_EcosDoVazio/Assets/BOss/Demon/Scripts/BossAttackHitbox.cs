using UnityEngine;

public class BossAttackHitbox : MonoBehaviour
{
    public int dano = 1;

     private bool podeDarDano = true;

    private void OnEnable()
    {
        podeDarDano = true; // reseta sempre que o ataque começa
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!podeDarDano) return;
        PlayerLife vida = other.GetComponentInParent<PlayerLife>();
        if (vida != null)
        {
            vida.TakeDamage(dano);
            podeDarDano = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!podeDarDano) return;
        PlayerLife vida = other.GetComponentInParent<PlayerLife>();
        if (vida != null)
        {
            vida.TakeDamage(dano);
            podeDarDano = false;
        }
        
    
    }
}
