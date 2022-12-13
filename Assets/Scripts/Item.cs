using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public BlockType blockType;
    public string itemName;
    public Sprite itemImage;
    public int maxStorageCount;
}
