using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject OptionsMenu;

    public Text valueText;

    private void start() //텍스트 가져오기
    {
        valueText = GetComponent<Text>();
    }

    public void MasterVolume (float value) //마스터 볼륨 설정
    {
        valueText.text = ($" Master Volume : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void MusicVolume(float value) //음악 볼륨 설정 
    {
        valueText.text = ($" Music : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void BLockVolume(float value) //블럭 볼륨 설정
    {
        valueText.text = ($" Block : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void FriendlyVolume(float value) //친화적생명체 볼륨 설정
    {
        valueText.text = ($" Friendly Creatures : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void HostileVolume(float value) //적대적생명체 볼륨 설정
    {
        valueText.text = ($" Ambient/Envieonment : ") + Mathf.RoundToInt(value * 100) + "%";
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
    }

    public void OptionBack()
    {
        OptionsMenu.SetActive(false);  //옵션나가기
    }

}
