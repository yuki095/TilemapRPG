using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    [SerializeField]
    private EventDataSO eventDataSO;

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
}
