using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 押した装備ボタン番号を保持するためのスクリプト

public class SelectedEquipButton : MonoBehaviour
{
    public int selectedEquipButton;

    private void Start()
    {
        selectedEquipButton = -1;
    }

    public void SetSelectedEquipButton(int select)
    {
        selectedEquipButton = select;
    }

    public int GetSelectedEquipButton()
    {
        return selectedEquipButton;
    }
}
