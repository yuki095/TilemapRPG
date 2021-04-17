using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    [SerializeField]
    private EventDataSO eventDataSO;

    [SerializeField]
    private ItemDataSO itemDataSO;

    // ItemTypeごとに分類したList
  　[SerializeField]
    private List<ItemDataSO.ItemData> equipItemDataList = new List<ItemDataSO.ItemData>(); // ItemTypeがEquipのItemDataクラスだけを管理するList
    [SerializeField]
    private List<ItemDataSO.ItemData> saleItemDataList = new List<ItemDataSO.ItemData>();
    [SerializeField]
    private List<ItemDataSO.ItemData> valuablesItemDataList = new List<ItemDataSO.ItemData>();
    [SerializeField]
    private List<ItemDataSO.ItemData> useItemDataList = new List<ItemDataSO.ItemData>();

    // ItemTypeごとに分類したアイテムの名前のList
    [SerializeField]
    private List<string> equipItemNamesList = new List<string>();  // ItemTypeがEquipの文字列だけを管理するList
    [SerializeField]
    private List<string> saleItemNamesList = new List<string>();
    [SerializeField]
    private List<string> valuablesItemNamesList = new List<string>();
    [SerializeField]
    private List<string> useItemNamesList = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // シーン遷移しても破壊されないオブジェクト
        }
        else
        {
            Destroy(gameObject);
        }

        // アイテムの種類別のListを作成
        CreateItemTypeLists();

        // アイテムの名前をアイテムの種類ごとに分類してListを作成
        CreateItemNamesListsFromItemData();

    }

    /// <summary>
    /// NPC用のデータからEventDataを取得
    /// </summary>
    /// <param name="npcEventNo"></param>
    /// <returns></returns>
    public EventData GetEventDataFromNPCEvent(int npcEventNo)
    {
        // EventDataSOスクリプタブル・オブジェクトのeventDatasListの中身（EventData）を
        // 1つずつ順番に取り出して、eventData変数に代入
        foreach (EventData eventData in eventDataSO.eventDatasList)
        {
            Debug.Log(eventData.no);
            // eventDataの情報を判定し、EventTypeがTalkかつ、引数で取得しているnpcEventNoと同じ場合
            if (eventData.eventType == EventData.EventType.Talk && eventData.no == npcEventNo)
            {
                // 該当するEventDataであると判定し、その情報を返す
                return eventData;
            }
        }
        // 上記の処理の結果、該当するEventDataの情報がスクリプタブル・オブジェクト内にない場合はnullを返す
        return null;
    }

    /// <summary>
    /// ItemNo から ItemDataを取得する
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    public ItemDataSO.ItemData GetItemDataFromItemNo(int itemNo)
    {
        // ItemDataSOスクリプタブルオブジェクト内のItemDataの情報を一つずつ順番にitemData変数に代入
        foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList)
        {
            if (itemData.itemNo == itemNo)  // 引数で届いているitemNoの値が同じ場合
            {
                return itemData;            // itemDataの値を戻す
            }
        }
        return null;    // 該当する情報がない場合
    }

    /// <summary>
    /// /ItemNameからItemDataを取得
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public ItemDataSO.ItemData GetItemDataFromItemName (ItemName itemName)
    {
        // 条件に合った先頭の値を取得（上記のforeach文と同じ処理）
        return itemDataSO.itemDataList.FirstOrDefault(x => x.itemName == itemName);
    }

    /// <summary>
    /// ItemDataの最大要素数を取得
    /// </summary>
    /// <returns></returns>
    public int GetItemDataSoCount()
    {
        return itemDataSO.itemDataList.Count;   　// 所持数
    }

    /// <summary>
    /// アイテムの種類別のListを作成
    /// </summary>
    private void CreateItemTypeLists()
    {
        // ItemDataSOスクリプタブル・オブジェクト内のItemDataの情報を１つずつ順番に取り出して、itemData変数に代入
        foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList)
        {
            // 現在取り出しているitemData変数のItemTypeの値がどのcaseと合致するかを判定
            switch (itemData.itemType)
            {
                // itemData.itemType == ItemType.Equipの場合、equipItemDatasList変数にitemData変数の値を追加する
                case ItemType.Equip:
                    equipItemDataList.Add(itemData);
                    break;

                case ItemType.Sele:
                    saleItemDataList.Add(itemData);
                    break;

                case ItemType.Valuables:
                    valuablesItemDataList.Add(itemData);
                    break;

                case ItemType.Use:
                    useItemDataList.Add(itemData);
                    break;
            }
        }
    }

    private void CreateItemNamesListsFromItemData()
    {
        // ItemName型のenumに登録されている列挙子をすべて取り出して文字列の配列にして取得し、values変数に代入
        string[] values = Enum.GetNames(typeof(ItemName));

        // ItemDataSOスクリプタブル・オブジェクト内のItemDataの情報を１つずつ順番に取り出して、itemData変数に代入
        foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList)
        {
            // values配列変数の中を先頭から順番に検索し、現在取り出しているitemData変数のitemNameと合致した値があれば
            if (!string.IsNullOrEmpty(values.FirstOrDefault(x => x == itemData.itemName.ToString())))
            {
                // 最初に合致した値を文字列として代入
                string itemName = itemData.itemName.ToString();

                // 現在取り出しているitemData変数のItemTypeの値がどのcaseと合致するかを判定（上のswitch文と同じ分岐でも実装できる）
                switch ((int)itemData.itemType)
                {
                    // itemData.itemType == 0 (ItemType型の列挙子の最初のもの）
                    case 0:
                        equipItemNamesList.Add(itemName);
                        break;
                    case 1:
                        saleItemNamesList.Add(itemName);
                        break;
                    case 2:
                        valuablesItemNamesList.Add(itemName);
                        break;
                    case 3:
                        useItemNamesList.Add(itemName);
                        break;
                }
            }

        }
    }

}
