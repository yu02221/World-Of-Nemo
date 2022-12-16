using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;

    public Item _item;

    public int itemCount;
    public Text itemCountTxt;
    public GameObject selected;
    public bool clickable;

    public SelectedItem selectedItem;

    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void SetItemCountText()
    {
        if (itemCount <= 1)
            itemCountTxt.text = "";
        else
            itemCountTxt.text = itemCount.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print(gameObject.name);
        if (clickable && eventData.button == PointerEventData.InputButton.Left)
        {
            SwitchItem();
        }
    }

    private void SwitchItem()
    {
        Item tempItem = selectedItem.item;
        int tempCount = selectedItem.itemCount;

        selectedItem.item = item;
        selectedItem.itemCount = itemCount;
        selectedItem.SetItemCountText();

        item = tempItem;
        itemCount = tempCount;
        SetItemCountText();
    }
}
