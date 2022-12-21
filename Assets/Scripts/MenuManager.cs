using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject gameMenuWindow;

    public GameObject inventoryWindow;
    public GameObject craftingTableWindow;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (craftingTableWindow.activeSelf)
                CloseCraftingTable();
            else if (inventoryWindow.activeSelf)
                CloseInventory();
            else if(gameMenuWindow.activeSelf)
                CloseGameMenu();
            else
                OpenGameMenu();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (craftingTableWindow.activeSelf)
                CloseCraftingTable();
            else if (inventoryWindow.activeSelf)
                CloseInventory();
            else if(!gameMenuWindow.activeSelf)
                OpenInventory();
        }
    }
    public void QuitToTitle()
    {
        SceneManager.LoadScene("Menu");
    }

    public void CloseGameMenu()
    {
        gameMenuWindow.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void OpenGameMenu()
    {
        gameMenuWindow.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void CloseInventory()
    {
        inventoryWindow.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
    public void CloseCraftingTable()
    {
        craftingTableWindow.SetActive(false);
        CloseInventory();
    }

    public void OpenInventory()
    {
        inventoryWindow.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }
}
