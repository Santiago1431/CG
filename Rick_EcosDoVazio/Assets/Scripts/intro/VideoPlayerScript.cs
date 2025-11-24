using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class VideoPlayerScript : MonoBehaviour
{
    // Arraste o componente VideoPlayer do seu GameObject para este campo no Inspector
    public VideoPlayer videoPlayer;
    
    // Define a cena a ser carregada após o vídeo
    public string nextSceneName = "Fase1"; 

    private bool isPlaying = false;


    void Start()
    {
        // Certifica-se que a biblioteca UnityEngine.Video está no seu projeto.
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer não está configurado. Atribua o componente VideoPlayer no Inspector.");
            return;
        }

        // Assina o evento para quando o vídeo terminar
        videoPlayer.loopPointReached += OnVideoEnd;
        
        // Inicia a reprodução do vídeo
        videoPlayer.Play();

        isPlaying = true;
        //mantenha precionado para pular o vídeo
        StartCoroutine(CheckSkipInput());
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Vídeo de introdução terminou. Carregando a cena: " + nextSceneName);
        
        // Carrega a próxima cena (Fase1)
        SceneManager.LoadScene(nextSceneName);

    }

    private IEnumerator CheckSkipInput()
    {
        while (isPlaying == true)
        {
            // Verifica se a tecla "Space" está sendo pressionada
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Vídeo pulado pelo jogador. Carregando a cena: " + nextSceneName);
                
                // Para o vídeo e carrega a próxima cena
                videoPlayer.Stop();
                SceneManager.LoadScene(nextSceneName);
                yield break; // Sai da coroutine
            }
            yield return null; // Espera até o próximo frame
        }
    }
}