using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;
    public Inventory nextInventory;


    private void OnValidate()
    {
        slots = transform.GetComponentsInChildren<Slot>();
    }
    
    public void AddItem(Item _item, int count)
    {
        int addIdx = -1;
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item)
            {
                if (slots[i].itemCount + count <= _item.maxStorageCount)
                {
                    addIdx = i;
                    break;
                }
                else
                {
                    count -= _item.maxStorageCount - slots[i].itemCount;
                    slots[i].itemCount = _item.maxStorageCount;
                }
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
                if (nextInventory != null)
                    nextInventory.AddItem(_item, count);
                else
                    print("모든 인벤토리가 가득 찼습니다.");
                return;
            }
        }
        if (slots[addIdx].item == null)
            slots[addIdx].item = _item;
        slots[addIdx].itemCount += count;
        slots[addIdx].SetItemCountText();
    }
}
