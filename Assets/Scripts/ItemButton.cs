using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Text informationText;
    private StatusWindowItemDataBase itemDataBase;
    private int itemNum;

    private void Start()
    {
        itemDataBase = Camera.main.GetComponent<StatusWindowItemDataBase>();
        informationText = transform.parent.parent.parent.Find("Information/Text").GetComponent<Text>();
    }

    // アイテムボタンが選択されたら情報を表示
    public void OnSelected()
    {
        informationText.text = itemDataBase.GetItemData()[itemNum].GetItemInformation();
    }

    // アイテムボタンから移動したら情報を削除
    public void OnDeselected()
    {
        informationText.text = "";
    }

    public void SetItemNum(int number)
    {
        itemNum = number;
    }

    public int GetItemNum()
    {
        return itemNum;
    }
}
