using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Block,      //��
        Equipment,  //���
        Ingredient, //���
        Food,
    }

    public ItemType itemType;
    public BlockType blockType;
    public string itemName;
    public Sprite itemImage;
    public int maxStorageCount;
}
