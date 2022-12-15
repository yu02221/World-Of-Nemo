using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int playerHP;
    PlayerMove pm;

    public Text lifeText;
    private void Start()
    {
        pm = GetComponent<PlayerMove>();

        lifeText.text = $"Life = {playerHP}";
    }
    private void Update()
    {
        CheckHP();
        lifeText.text = $"Life = {playerHP}";
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
