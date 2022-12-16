using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int playerHP;
    PlayerMove pm;

    public Text lifeText;

    Rigidbody rb;
    public int isCollisionPower;

    public int enemy_2Power;

    private void Start()
    {
        pm = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody>();
        lifeText.text = $"Life = {playerHP}";
    }
    private void Update()
    {
        CheckHP();
        lifeText.text = $"Life = {playerHP}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy_2")
        {
            rb.velocity = Vector3.up * isCollisionPower;
            playerHP -= enemy_2Power;
            //Vector3 dir = transform.position - collision.gameObject.transform.position;
            //dir = dir.normalized * 1000;
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(dir);
        }
    }

    void CheckHP()
    {
        if(playerHP <= 0)
        {
            pm.playerState = PlayerState.Dead;
        }
    }
    
    public void DamageAction(int damage)
    {
        playerHP -= damage;
    }
    
}
