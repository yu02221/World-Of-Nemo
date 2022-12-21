using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventory : MonoBehaviour
{
    public int size;
    public Slot[] slots;
    string[] items;
    public ItemSet itemset;
    List<Dictionary<string, object>> recipes;
    Item resultItem;
    int resultItemCount;
    public ResultSlot resultSlot;


    private void OnValidate()
    {
        slots = transform.GetComponentsInChildren<Slot>();
    }

    private void Start()
    {
        items = new string[size];
        if (size == 4)
            recipes = CSVReader.Read("CraftingInven");
        else if (size == 9)
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
        int j = 0;
        for (; j < recipes.Count; j++)
        {
            if (items[0] == recipes[j]["Item0"].ToString() &&
                items[1] == recipes[j]["Item1"].ToString() &&
                items[2] == recipes[j]["Item2"].ToString() &&
                items[3] == recipes[j]["Item3"].ToString())
            {
                resultItem = NameToItem(recipes[j]["ResultItem"].ToString());
                resultItemCount = int.Parse(recipes[j]["Count"].ToString());
                break;
            }
        }
        if (j == recipes.Count)
            resultItem = null;

        if (resultItem != null)
        {
            resultSlot.item = resultItem;
            resultSlot.itemCount = resultItemCount;
            resultSlot.SetItemCountText();
        }
        else
        {
            resultSlot.item = null;
            resultSlot.itemCount = 0;
            resultSlot.SetItemCountText();
        }
    }

    private Item NameToItem(string name)
    {
        Item item = null;
        switch (name)
        {
            case "CraftingTable":
                item = itemset.craftingTable;
                break;
            case "OakPlanks":
                item = itemset.oakPlanks;
                break;
            default:
                item = null;
                break;
        }
        return item;
    }
}
