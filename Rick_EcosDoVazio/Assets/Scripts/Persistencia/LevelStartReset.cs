using UnityEngine;

public class LevelStartReset : MonoBehaviour
{
    // Awake é chamado antes de qualquer método Start()
    void Awake()
    {
        // Reseta o progresso do checkpoint toda vez que a cena de jogo carrega
        PlayerLife.checkpointReached = false;
    }
}