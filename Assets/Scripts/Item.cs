using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Block,      //블럭
        Equipment,  //장비
        Ingredient, //재료
        Food,
        Other,
    }

    public ItemType itemType;
    public BlockType blockType;
    public string itemName;
    public Sprite itemImage;
    public int maxStorageCount;
    public float burningTime;
}
