using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerHP;
    PlayerMove pm;
    private void Start()
    {
        pm = GetComponent<PlayerMove>();
    }
    private void Update()
    {
        CheckHP();
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
