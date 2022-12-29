using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour
{
    public int health1;
    public int numOfHunger;

    public Image[] hungers;
    public Sprite fullHunger;
    public Sprite halfHunger;
    public Sprite emptyHunger;



    private void Update()
    {
        if (health1 > numOfHunger)
            health1 = numOfHunger;

        for (int i = 0; i < hungers.Length; i++)
        {
            if (i < health1 / 2)
            {
                hungers[i].sprite = fullHunger;
            }
            else if (i == health1 / 2)
            {
                if (health1 % 2 == 1)
                    hungers[i].sprite = halfHunger;
                else
                    hungers[i].sprite = emptyHunger;
            }
            else
                hungers[i].sprite = emptyHunger;

            /* if (i < numOfHunger)
                 hungers[i].enabled = true;
             else if (i > numOfHunger)
                 hungers[i].enabled = true;
             else
                 hungers[i].enabled = false;
            */
        }
    }
}
