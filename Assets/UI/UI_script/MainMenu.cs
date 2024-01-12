using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame ()
    {
        SceneManager.LoadScene("LevelNew");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit_DA_GAME");
    }
}
