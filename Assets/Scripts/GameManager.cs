using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Gameplay objects to disable on GameOver")]
    [SerializeField] private GameObject[] thingsToDisableOnGameOver;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        foreach (var go in thingsToDisableOnGameOver)
            if (go) go.SetActive(false);

        if (gameOverPanel) gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        if (musicSource) musicSource.Pause();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        if (musicSource) musicSource.UnPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
