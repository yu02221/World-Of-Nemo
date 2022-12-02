using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject OptionsMenu;
    public void StartGame()
    {
        SceneManager.LoadScene("World");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void OnClickOptions()
    {
        OptionsMenu.SetActive(true);

    }

    public void OptionBack()
    {
        OptionsMenu.SetActive(false);
    }

}
