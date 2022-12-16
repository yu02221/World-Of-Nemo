using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform slotParent;
    public Slot[] slots;


    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
    
    public void AddItem(Item _item)
    {
        int addIdx = -1;
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item && slots[i].itemCount < _item.maxStorageCount)
            {
                addIdx = i;
                break;
            }
        }

        if (addIdx == -1)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    addIdx = i;
                    break;
                }
            }
            if (addIdx == -1)
            {
                print("인벤토리가 가득 찼습니다.");
                return;
            }
        }
        if (slots[addIdx].item == null)
            slots[addIdx].item = _item;
        slots[addIdx].itemCount++;
        slots[addIdx].SetItemCountText();
    }
}
