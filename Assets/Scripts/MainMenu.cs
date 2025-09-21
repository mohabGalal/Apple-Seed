using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button TutorialButton;
    public Button PlayButton;
    public Button Play2;

    private void Start()
    {
        if(TutorialButton != null )
            TutorialButton.onClick.AddListener(PlayTutoria);
        if (PlayButton != null)
            PlayButton.onClick.AddListener(PlayScene);
        if (Play2 != null)
            Play2.onClick.AddListener(PlayMainLevel);
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void PlayTutoria()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void PlayMainLevel()
    {
        SceneManager.LoadScene("MainLevel");
    }
}
