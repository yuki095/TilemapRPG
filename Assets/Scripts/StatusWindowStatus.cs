using System.Collections;
using UnityEngine;

public class StatusWindowStatus : MonoBehaviour
{
    // アイテムを持っているかのフラグ
    [SerializeField]
    private bool[] itemFlags = new bool[6];

    private StatusWindowItemDataBase statusWindowItemDataBase;

    void Start()
    {
        statusWindowItemDataBase = GetComponent<StatusWindowItemDataBase>();
        SetItemData("懐中電灯");
        SetItemData("ハンドガン");
    }

    // アイテムを持っているかどうか
    public bool GetItemFlag(int num)
    {
        return itemFlags[num];
    }

    // アイテムをセット
    public void SetItemData(string name)
    {
        var itemDatas = statusWindowItemDataBase.GetItemData();
        for(int i = 0; i < itemDatas.Length; i++)
        {
            if (itemDatas[i].GetItemName() == name)
            {
                itemFlags[i] = true;
            }
        }
    }
}
