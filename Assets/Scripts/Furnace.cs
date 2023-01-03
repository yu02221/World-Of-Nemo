using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : CraftingInventory
{
    public Slider arrow;
    public Slider fire;
    public FuelSlot fuelSolt;

    private float burnTime;
    private float bakeTime;
    private bool fuelIn;
    private bool itemIn;

    private void OnValidate()
    {
        slots = transform.GetComponentsInChildren<Slot>();
    }

    private void Start()
    {
        items = new string[slots.Length];
        recipes = CSVReader.Read("Furnace");
    }

    private void Update()
    {
        FuelCheck();
        ItemCheck();


        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                items[i] = slots[i].item.itemName;
        }
        
        if (itemIn && burnTime > 0)
        {
            print(burnTime);
            burnTime -= Time.deltaTime;
            fire.value = burnTime / fuelSolt.item.burningTime;
        }
        else
        {
            fuelSolt.itemCount--;
            if (fuelSolt.itemCount == 0)
            {
                fire.value = 0;
                fuelSolt.item = null;
                fuelIn = false;
            }
            fuelSolt.SetItemCountText();
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

    private void FuelCheck()
    {
        if (!fuelIn && fuelSolt.item != null)
        {
            fuelIn = true;
            burnTime = fuelSolt.item.burningTime;
            fire.value = 1;
        }
        if (fuelSolt.item == null)
        {
            fuelIn = false;
            burnTime = 0;
            fire.value = 0;
        }
    }

    private void ItemCheck()
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            if (items[0] == recipes[i]["item"].ToString())
            {
                itemIn = true;
                return;
            }
        }
        itemIn = false;
        return;
    }

    protected new void GetResultItem()
    {
        int i = 0;
        for (; i < recipes.Count; i++)
        {
            if (items[0] == recipes[i]["Item"].ToString())
            {
                resultItem = itemset.iSet[recipes[i]["ResultItem"].ToString()];
                resultItemCount++;
                break;
            }
        }
        if (i == recipes.Count)
            resultItem = null;
    }
}