using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject OptionsMenu;

    public Text masterText;
    public Text musicText;
    public Text blockText;
    public Text friendlyText;
    public Text hostileText;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider blockSlider;
    public Slider friendlySlider;
    public Slider hostileSlider;

    public void SetSound()
    {
      
            masterText.text = ($" Master Volume : ") + Mathf.RoundToInt(masterSlider.value * 100) + "%";

            musicText.text = ($" Music : ") + Mathf.RoundToInt(musicSlider.value * 100) + "%";

            blockText.text = ($" Block : ") + Mathf.RoundToInt(blockSlider.value * 100) + "%";

            friendlyText.text = ($" Friendly Creatures : ") + Mathf.RoundToInt(friendlySlider.value * 100) + "%";
 
            hostileText.text = ($" Hostile Creatures : ") + Mathf.RoundToInt(hostileSlider.value * 100) + "%";

    }

    public void StartGame()
    {
        SceneManager.LoadScene("World"); //게임 들어가기
    }

    public void QuitGame()
    {
        Application.Quit(); //게임나가기
    }
    
    public void OnClickOptions()
    {
        OptionsMenu.SetActive(true);  //옵션들어가기
        GetOption();
        SetSound();
    }

    public void OptionBack()
    {
        OptionsMenu.SetActive(false);  //옵션나가기
        SaveOption();
    }

    public void SaveOption()
    {
        PlayerPrefs.SetFloat("Master Volume", masterSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("Block", blockSlider.value);
        PlayerPrefs.SetFloat("Friendly Creatures", friendlySlider.value);
        PlayerPrefs.SetFloat("Hostile Creatures", hostileSlider.value);
    }

    public void GetOption()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master Volume");
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        blockSlider.value = PlayerPrefs.GetFloat("Block");
        friendlySlider.value = PlayerPrefs.GetFloat("Friendly Creatures");
        hostileSlider.value = PlayerPrefs.GetFloat("Hostile Creatures");
    }
}
