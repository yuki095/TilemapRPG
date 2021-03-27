using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventDataSO", menuName = "Create EventDataSO")]
public class EventDataSO : ScriptableObject
{
    // リストの初期化
    public List<EventData> eventDatasList = new List<EventData>();　// 複数のEventDataを1つの変数内でまとめて管理
}
