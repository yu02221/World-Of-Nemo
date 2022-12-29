using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : CraftingInventory
{
    private void OnValidate()
    {
        slots = transform.GetComponentsInChildren<Slot>();
    }

    private void Start()
    {
        items = new string[slots.Length];
        recipes = CSVReader.Read("CraftingTable");
    }

    private void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                items[i] = slots[i].item.itemName;
            else
                items[i] = "null";
        }
        
        GetResultItem();

        if (resultItem != null)
        {
            resultSlot.item = resultItem;
            resultSlot.itemCount = resultItemCount;
        }
        else
        {
            resultSlot.item = null;
            resultSlot.itemCount = 0;
        }
        resultSlot.SetItemCountText();
    }

    protected new void GetResultItem()
    {
        int i = 0;
        for (; i < recipes.Count; i++)
        {
            if (items[0] == recipes[i]["Item0"].ToString() &&
                items[1] == recipes[i]["Item1"].ToString() &&
                items[2] == recipes[i]["Item2"].ToString() &&
                items[3] == recipes[i]["Item3"].ToString() &&
                items[4] == recipes[i]["Item4"].ToString() &&
                items[5] == recipes[i]["Item5"].ToString() &&
                items[6] == recipes[i]["Item6"].ToString() &&
                items[7] == recipes[i]["Item7"].ToString() &&
                items[8] == recipes[i]["Item8"].ToString())
            {
                resultItem = itemset.iSet[recipes[i]["ResultItem"].ToString()];
                resultItemCount = int.Parse(recipes[i]["Count"].ToString());
                break;
            }
        }
        if (i == recipes.Count)
            resultItem = null;
    }
}
