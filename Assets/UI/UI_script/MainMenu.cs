﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame ()
    {
        SceneManager.LoadScene("LevelDesign");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit_DA_GAME");
    }
}
