using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventDataSO", menuName = "Create EventDataSO")]
public class EventDataSO : ScriptableObject
{
    // リストの初期化（箱を作る）
    public List<EventData> eventDatasList = new List<EventData>();　// 複数のEventDataを1つの変数内でまとめて管理
}
