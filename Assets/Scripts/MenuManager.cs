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
                CloseGameMenu();
            }
            else
            {
                OpenGameMenu();
            }
        }
    }
    public void QuitToTitle()
    {
        SceneManager.LoadScene("Menu");
    }

    public void CloseGameMenu()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameIsMenu = false;
    }

    public void OpenGameMenu()
    {
        menuCanvas.SetActive(true);
        Time.timeScale = 0f;
        GameIsMenu = true;
    }

    
}
