using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    

    private void Update()
    {
        if (health > numOfHearts)
            health = numOfHearts;

        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < health / 2)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i == health / 2) 
            {
                if (health % 2 == 1)
                    hearts[i].sprite = halfHeart;
                else
                    hearts[i].sprite = emptyHeart;
            }
            else
                hearts[i].sprite = emptyHeart;
            /*
            if(i < numOfHearts)
                hearts[i].enabled = true;
            else if (i > numOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
            */
        }
    }
}
