using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("Fontes de Áudio")]
    [SerializeField]  AudioSource sfxAudioSource;
    [SerializeField]  AudioSource musicAudioSource;


    [Header("Clipes de Áudio")]
    public AudioClip jumpSFX;
    public AudioClip shootSFX;
    public AudioClip rollSFX;
    public AudioClip keyCollectSFX;
    public AudioClip portalInSFX;
    public AudioClip portalOutSFX;

    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
}
