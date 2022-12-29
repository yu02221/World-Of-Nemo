using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DayAndNight : MonoBehaviour
{
    private float elpasedTime;  //경과시간
    private float r = 1f;       //r,g,b 색깔들
    private float g = 1f;
    private float b = 1f;

    public GameObject skyDome;  //스카이돔
    private Material skyDomeMaterial;   //스카이돔의 메테리얼
    private float offsetValueX = 0;

    void Start()
    {
        skyDomeMaterial = skyDome.GetComponent<Renderer>().material;
        //스카이돔 메테리얼의 오프셋에 접근하는 방법
        //오프셋에서 접근해서 색 변경
        skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(offsetValueX, 0));

        StartCoroutine(DayImpl());
    }

    //낮 -> 밤
    IEnumerator DayToNightImpl()
    {
        while (true)
        {
            elpasedTime += Time.deltaTime;

            RenderSettings.ambientLight = new Color(r, g, b, 1);
            r -= r / 1000;
            g -= g / 1000;
            b -= b / 1000;

            if (r <= 0 || g <= 0 || b <= 0)
            {
                r = 0;
                g = 0;
                b = 0;
            }

            offsetValueX += 0.0025f * Time.deltaTime;
            skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(offsetValueX, 0));

            if (elpasedTime >= 200)
            {
                offsetValueX = 0.5f;
                elpasedTime = 0;

                StartCoroutine(NightImpl());
                break;
            }

            yield return null;
        }
    }

    //밤
    IEnumerator NightImpl()
    {
        skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(offsetValueX, 0));

        while (true)
        {
            elpasedTime += Time.deltaTime;


            if (elpasedTime >= 400)
            {
                elpasedTime = 0;
                StartCoroutine(NightToDayImpl());
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
            elpasedTime += Time.deltaTime;

            RenderSettings.ambientLight = new Color(r, g, b, 1);
            r += r / 1000;
            g += g / 1000;
            b += b / 1000;

            if (r >= 1 || g >= 1 || b >= 1)
            {
                r = 1;
                g = 1;
                b = 1;
            }

            offsetValueX -= 0.0025f * Time.deltaTime;
            skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(offsetValueX, 0));

            if (elpasedTime >= 200)
            {
                offsetValueX = 0;
                elpasedTime = 0;

                StartCoroutine(DayImpl());
                break;
            }

            yield return null;
        }
    }

    //낮
    IEnumerator DayImpl()
    {
        skyDomeMaterial.SetTextureOffset("_MainTex", new Vector2(offsetValueX, 0));

        while (true)
        {
            elpasedTime += Time.deltaTime;

            if (elpasedTime >= 400)
            {
                StopAllCoroutines();
                elpasedTime = 0;
                StartCoroutine(DayToNightImpl());
                break;
            }

            yield return null;
        }
    }
}