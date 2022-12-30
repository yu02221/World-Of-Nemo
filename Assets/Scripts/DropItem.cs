using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item item;
    private int count;
    public Inventory hotInven;
    public Transform player;

    private void Update()
    {
        Floating();
        CheckDistanc();
    }

    private void Floating()
    {

    }

    private void CheckDistanc()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            hotInven.AddItem(item, count);
        }
    }
}
