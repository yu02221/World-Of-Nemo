using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static bool GameIsMenu = false;
    public GameObject menuCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsMenu)
            {
                InGameMenu();
            }
            else
            {
                Pause();
            }
        }
    }
    public void QuitToTitle()
    {
        SceneManager.LoadScene("Menu");
    }

    public void InGameMenu()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 0f;
        GameIsMenu = false;
    }

    public void Pause()
    {
        menuCanvas.SetActive(true);
        Time.timeScale = 1f;
        GameIsMenu = true;
    }

    
}
