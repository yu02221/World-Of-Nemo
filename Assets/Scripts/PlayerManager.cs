using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int maxHp;
    public int nowHp;
    PlayerMove pm;

    //public Text lifeText;

    Rigidbody rb;
    private void Start()
    {
        pm = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody>();
        //lifeText.text = $"Life = {nowHp}";
        
    }
    private void Update()
    {
        CheckHP();
        //lifeText.text = $"Life = {nowHp}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        //크러쉬에너미와 부딪혔을때
        if (collision.gameObject.name == "Enemy2")
        {
            Vector3 reactVec = transform.position - collision.transform.position;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);
        }
    }
    void CheckHP()
    {
        if(nowHp <= 0)
        {
            pm.playerState = PlayerState.Dead;
        }
    }
}
