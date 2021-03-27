using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventDataSO", menuName = "Create EventDataSO")]
public class EventDataSO : ScriptableObject
{
    public List<EventData> eventDataList = new List<EventData>();
}
