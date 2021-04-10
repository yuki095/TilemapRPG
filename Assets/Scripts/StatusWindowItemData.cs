using UnityEngine;
using System.Collections;

public class StatusWindowItemData : Object
{
    private Sprite itemSprite;                          // アイテムのImage画像ß
    private string itemName;                            // アイテムの名前
    private StatusWindowItemDataBase.Item itemType;     // アイテムのタイプ
    private string itemInformation;                     // アイテムの情報

    public StatusWindowItemData(Sprite image, string itemName, StatusWindowItemDataBase.Item itemType, string information)
    {
        this.itemSprite = image;
        this.itemName = itemName;
        this.itemType = itemType;
        this.itemInformation = information;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public StatusWindowItemDataBase.Item GetItemType()
    {
        return itemType;
    }

    public string GetItemInformation()
    {
        return itemInformation;
    }

}
