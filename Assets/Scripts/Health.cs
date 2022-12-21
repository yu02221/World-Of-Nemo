using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    

    private void Update()
    {
        if (hp > maxHp)
            hp = maxHp;

        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < hp / 2)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i == hp / 2) 
            {
                if (hp % 2 == 1)
                    hearts[i].sprite = halfHeart;
                else
                    hearts[i].sprite = emptyHeart;
            }
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
