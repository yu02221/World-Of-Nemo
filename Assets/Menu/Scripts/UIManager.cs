using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject OptionsMenu;

    public Text valueText;

    private void start() //�ؽ�Ʈ ��������
    {
        valueText = GetComponent<Text>();
    }

    public void MasterVolume (float value) //������ ���� ����
    {
        valueText.text = ($" Master Volume : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void MusicVolume(float value) //���� ���� ���� 
    {
        valueText.text = ($" Music : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void BLockVolume(float value) //�� ���� ����
    {
        valueText.text = ($" Block : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void FriendlyVolume(float value) //ģȭ������ü ���� ����
    {
        valueText.text = ($" Friendly Creatures : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void HostileVolume(float value) //����������ü ���� ����
    {
        valueText.text = ($" Ambient/Envieonment : ") + Mathf.RoundToInt(value * 100) + "%";
    }

    public void StartGame()
    {
        SceneManager.LoadScene("World"); //���� ����
    }

    public void QuitGame()
    {
        Application.Quit(); //���ӳ�����
    }
    
    public void OnClickOptions()
    {
        OptionsMenu.SetActive(true);  //�ɼǵ���
    }

    public void OptionBack()
    {
        OptionsMenu.SetActive(false);  //�ɼǳ�����
    }

}
