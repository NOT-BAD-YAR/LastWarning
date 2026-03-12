using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip background;

    [Header("Music Settings")]
    [SerializeField] private string[] scenesWithMusic;

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        foreach (var sceneName in scenesWithMusic)
        {
            if (sceneName == currentScene)
            {
                PlayBackgroundMusic();
                return;
            }
        }
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource == null || background == null)
        {
            return;
        }

        musicSource.clip = background;
        musicSource.loop = true;
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}
