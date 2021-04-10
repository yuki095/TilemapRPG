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
}
