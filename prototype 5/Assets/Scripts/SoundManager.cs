using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource ambianceSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip caveAmbiance;
    [SerializeField] private AudioClip batSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip keySound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayCaveAmbiance();
        PlayBatSound();
        StartCoroutine(PlayRandomBatSounds()); // 🦇 Start random bat sounds
    }

    public void PlayCaveAmbiance()
    {
        if (caveAmbiance != null)
        {
            ambianceSource.clip = caveAmbiance;
            ambianceSource.loop = true;
            ambianceSource.Play();
        }
    }

    public void PlayBatSound()
    {
        if (batSound != null)
        {
            sfxSource.clip = batSound;
            sfxSource.loop = true;
            sfxSource.Play();
        }
    }

    public void PlayWinSound()
    {
        if (winSound != null)
        {
            sfxSource.PlayOneShot(winSound);
        }
    }
    public void PlayKeySound()
    {
        if (winSound != null)
        {
            sfxSource.PlayOneShot(keySound);
        }
    }
    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            sfxSource.PlayOneShot(deathSound);
        }
    }

    private IEnumerator PlayRandomBatSounds()
    {
        while (true) // Keep playing throughout the game
        {
            float randomDelay = Random.Range(10f, 30f); // Random delay between 10-30 seconds
            yield return new WaitForSeconds(randomDelay);

            PlayBatSound(); // Play the bat sound at a random interval
        }
    }
}
