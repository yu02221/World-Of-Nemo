using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RotateLight : MonoBehaviour
{
    public GameObject[] arrGameObjects;
    private Material[] arrMaterials;

    private bool isNight;       //���ΰ�?


    private float elpasedTime;  //����ð�
    private float r = 1f;       //r,g,b �����
    private float g = 1f;
    private float b = 1f;

    public GameObject skyDome;  //���ۺ��� �� ��ī�̵�
    private Material skyDomeMaterial;   //��ī�̵��� ���׸���
    private float offsetValueX = 0;

    void Start()
    {
        skyDomeMaterial = skyDome.GetComponent<Renderer>().material;
        //��ī�̵� ���׸����� �����¿� �����ϴ� ���
        //�����¿��� �����ؼ� ���� �ٲ��ٰ���
        skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));

        isNight = false;
        arrMaterials = new Material[arrGameObjects.Length];


        //�ð��׽�Ʈ �ڷ�ƾ ����
        if (!this.isNight)
        {
            this.StartCoroutine(this.DayToNightImpl());
        }
    }

    //�� -> ��
    IEnumerator DayToNightImpl()
    {
        while (true)
        {
            this.elpasedTime += Time.deltaTime;

            RenderSettings.ambientLight = new Color(this.r, this.g, this.b, 1);
            this.r -= r / 1000;
            this.g -= g / 1000;
            this.b -= b / 1000;

            if (this.r <= 0 || this.g <= 0 || this.b <= 0)
            {
                this.r = 0;
                this.g = 0;
                this.b = 0;
            }

            this.offsetValueX += 0.001f * Time.deltaTime;
            this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));

            if (this.elpasedTime >= 500)
            {
                this.offsetValueX = 0.5f;
                this.elpasedTime = 0;

                this.StartCoroutine(this.NightImpl());
                break;
            }

            yield return null;
        }
    }

    //��
    IEnumerator NightImpl()
    {
        this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));

        for (int i = 0; i < this.arrMaterials.Length; i++)
        {
            this.arrMaterials[i].SetColor("_EmissionColor", Color.white);

        }

        while (true)
        {
            this.elpasedTime += Time.deltaTime;


            if (this.elpasedTime >= 100)
            {
                this.elpasedTime = 0;
                this.StartCoroutine(this.NightToDayImpl());
                break;
            }

            yield return null;
        }
    }

    //�� -> ��
    IEnumerator NightToDayImpl()
    {


        while (true)
        {
            this.elpasedTime += Time.deltaTime;

            RenderSettings.ambientLight = new Color(this.r, this.g, this.b, 1);
            this.r += r / 1000;
            this.g += g / 1000;
            this.b += b / 1000;

            if (this.r >= 1 || this.g >= 1 || this.b >= 1)
            {
                this.r = 1;
                this.g = 1;
                this.b = 1;
            }

            this.offsetValueX -= 0.001f * Time.deltaTime;
            this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));

            if (this.elpasedTime >= 500)
            {
                this.offsetValueX = 0;
                this.elpasedTime = 0;

                this.StartCoroutine(this.DayImpl());
                break;
            }

            yield return null;
        }
    }

    //��
    IEnumerator DayImpl()
    {
        this.skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));


        for (int i = 0; i < this.arrMaterials.Length; i++)
        {
            this.arrMaterials[i].SetColor("_EmissionColor", Color.black);
        }

        while (true)
        {
            this.elpasedTime += Time.deltaTime;



            if (this.elpasedTime >= 100)
            {
                this.StopAllCoroutines();
                this.elpasedTime = 0;
                this.StartCoroutine(this.DayToNightImpl());
                break;
            }

            yield return null;
        }
    }
    private void Update()
    {
        //��ī�̵� ���ƶ�
        this.transform.Rotate(Vector3.up * 1 * Time.deltaTime);
    }
}