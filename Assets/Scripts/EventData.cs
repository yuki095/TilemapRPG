using UnityEngine;

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
    public string dialog;           // NPCのメッセージ、対象物のメッセージなど

    public Sprite eventSprite;      // イベントの画像データ

    public ItemName eventItemName; // イベントで獲得できるアイテム
    public int eventItemCount;     // イベントで獲得できるアイテムの個数

    // TODO　その他
}
