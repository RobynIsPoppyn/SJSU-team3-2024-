using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void pauseGame()
    {
        Time.timeScale = 0;
    }
    public void GotToMainMenu()
    {
        SceneManager.LoadSceneAsync("MenuTest");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}
