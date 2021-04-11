using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour
{
    public Text txtItemInfo;
    public Text txtItemName;

    public void DesplayItemInfos(string info, string name)
    {
        txtItemInfo.text = info;
        txtItemName.text = name;
    }
}
