using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(PlayScene);
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
