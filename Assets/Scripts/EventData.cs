using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventData
{
/// <summary>
/// イベントの種類
/// </summary>
public enum EventType
    {
        Talk,       // 0
        Search,     // 1
    }

    public EventType eventType;     // イベントの種類
    public int no;                  // 通し番号
    public string title;            // タイトル、NPCの名前、探す対象物の名前など

    [Multiline]                     // 複数行のテキスト
    public string[] dialogs;        // NPCのメッセージ、対象物のメッセージなど

    public Sprite eventSprite;      // イベントの画像データ

    public ItemName eventItemName; // イベントで獲得できるアイテム
    public int eventItemCount;     // イベントで獲得できるアイテムの個数


    [System.Serializable]
    public class EventDataDetail
    {
        public EventProgressType eventProgressType;

        [Multiline]
        public string[] dialogs;       　　　  // NPC のメッセージ、対象物のメッセージなど

        public Sprite eventSprite;     　　　　// イベントの画像データ

        public ItemName[] eventItemNames;      // イベントに必要なアイテム、あるいは獲得できるアイテム（配列）
        public int[] eventItemCounts;          // イベントに必要な個数、あるいは入手できる個数（配列）
    }

    public List<EventDataDetail> eventDataDetailsList = new List<EventDataDetail>();

    // TODO　その他
}
