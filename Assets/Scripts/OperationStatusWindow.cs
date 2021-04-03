using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OperationStatusWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject propertyWindow;

    // ステータスウィンドウの全部の画面
    [SerializeField]
    private GameObject[] windowLists;

    private void Update()
    {
        // ステータス画面のon/off
        if (Input.GetButtonDown("Start"))
        {
            propertyWindow.SetActive(!propertyWindow.activeSelf);
            // MainWindowをセット
            ChangeWindow(windowLists[0]);
        }
    }

    // ステータス画面のウィンドウのon/offメソッド
    public void ChangeWindow(GameObject window)
    {
        foreach(var item in windowLists)
        {
            if(item == window)
            {
                item.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                item.SetActive(false);
            }
            // それぞれのウィンドウのMenuAreaの最初の子要素をアクティブ状態にする
            EventSystem.current.SetSelectedGameObject(window.transform.Find("MenuArea").GetChild(0).gameObject);
        }
    }
}
