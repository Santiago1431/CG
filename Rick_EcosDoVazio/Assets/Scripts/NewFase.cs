using UnityEngine;
using UnityEngine.SceneManagement;



public class NewFase : MonoBehaviour
{

    public string Fase;

    //se o boss estiver morto quando o jogador colidir com esse objeto, a fase muda
    public TorizoController torizo;

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player != null && torizo.morto)
        {
            //Carrega a nova fase
           SceneManager.LoadScene(Fase);
        }else
        {
            Debug.Log("Boss ainda está vivo! Derrote-o para avançar.");
        }
    }
}
