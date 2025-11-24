using UnityEngine;
using UnityEngine.SceneManagement;



public class NewFase1 : MonoBehaviour
{


    //se o boss estiver morto quando o jogador colidir com esse objeto, a fase muda
    public BossController DemonBoss;

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player != null && DemonBoss.morto)
        {
            //Carrega a nova fase
           SceneManager.LoadScene("CutsceneFinal");
        }else
        {
            Debug.Log("Boss ainda está vivo! Derrote-o para avançar.");
        }
    }
}
