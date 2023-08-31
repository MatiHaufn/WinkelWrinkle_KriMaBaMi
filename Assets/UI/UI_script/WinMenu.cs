using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.tag =="Player")
        {
            SceneManager.LoadScene("WinMenu");
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit_DA_GAME");
    }
}
