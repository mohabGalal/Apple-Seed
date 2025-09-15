using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource MainTheme;// Drag an AudioSource for SFX
    [SerializeField] private AudioSource GameOverSource;

    [Header("Player SFX Clips")]
    public AudioClip jumpClip;
    public AudioClip SpinJump;
    public AudioClip PowerUp;
    public AudioClip PickItem;
    public AudioClip WinScreen;
    public AudioClip GameOver;
    public AudioClip Throw;
    public AudioClip EnemyHit;
    public AudioClip PlayerHit;
    public AudioClip MainThemeClip;


    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Generic play method
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.clip = clip;
            sfxSource.Play();
        }


    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
    public void PlayJump() => PlaySFX(jumpClip);
    public void PlaySpinJump() => PlaySFX(SpinJump);
    public void PlayPowerUp() => PlaySFX(PowerUp);
    public void PlayPickItem() => PlaySFX(PickItem);
    public void PlayWinScreen() => PlaySFX(WinScreen);
    public void PlayGameOver() => GameOverSource.Play();

    public void StopGameOver() => GameOverSource.Stop();
    public void PlayThrow() => PlaySFX(Throw);
    public void PlayEnemyHit() => PlaySFX(EnemyHit);
    public void PlayPlayerHit() => PlaySFX(PlayerHit);

    public void PlayMainTheme() => MainTheme.Play();
    public void StopMainTheme() => MainTheme.Stop();


}
