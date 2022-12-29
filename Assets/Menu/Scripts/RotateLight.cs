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

    private bool isNight;       //밤인가?


    private float elpasedTime;  //경과시간
    private float r = 1f;       //r,g,b 색깔들
    private float g = 1f;
    private float b = 1f;

    public GameObject skyDome;  //빙글빙글 돌 스카이돔
    private Material skyDomeMaterial;   //스카이돔의 메테리얼
    private float offsetValueX = 0;

    void Start()
    {
        skyDomeMaterial = skyDome.GetComponent<Renderer>().material;
        //스카이돔 메테리얼의 오프셋에 접근하는 방법
        //오프셋에서 접근해서 색을 바꿔줄거임
        skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(this.offsetValueX, 0));

        isNight = false;
        arrMaterials = new Material[arrGameObjects.Length];


        //시간테스트 코루틴 시작
        if (!this.isNight)
        {
            this.StartCoroutine(this.DayToNightImpl());
        }
    }

    //낮 -> 밤
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

    //밤
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

    //밤 -> 낮
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

    //낮
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
        //스카이돔 돌아라
        this.transform.Rotate(Vector3.up * 1 * Time.deltaTime);
    }
}