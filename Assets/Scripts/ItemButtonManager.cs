using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonManager : MonoBehaviour
{
    public ItemButtonDetail itemButtonDetailPrefab;
    public List<ItemButtonDetail> itemButtonDetailList = new List<ItemButtonDetail>();
    public Transform itemAreaTran;
    public Button btnItemSelectWindow;
    public Button btnExitItemWindow;

    private void Start()
    {
        // アイテム画面が開かれた時の処理
        btnItemSelectWindow.onClick.AddListener(CreateItemButtonDetails);
        // アイテム画面が閉じられた時の処理
        btnExitItemWindow.onClick.AddListener(DestroyItemButtonDetails);
    }

    /// <summary>
    /// アイテムインベントリーを作成
    /// </summary>
    public void CreateItemButtonDetails()
    {
        Debug.Log("アイテム生成");
        DestroyItemButtonDetails();

        // 所持しているアイテム分だけインスタンスする
        for (int i = 0; i < GameData.instance.GetItemInventryListCount(); i++)
        {
            ItemButtonDetail itemButtonDetail = Instantiate(itemButtonDetailPrefab, itemAreaTran, false);
            GameData.ItemInventryData itemInventryData = GameData.instance.GetItemInventryData(i);
            itemButtonDetail.SetUpItemButtonDetail(DataBaseManager.instance.GetItemDataFromItemName(itemInventryData.itemName), itemInventryData.count);
            itemButtonDetailList.Add(itemButtonDetail);
        }
    }

    public void DestroyItemButtonDetails()
    {
        if (itemButtonDetailList.Count > 0)     // 所持数が0より大きい場合
        {
            for (int i = 0; i < itemButtonDetailList.Count; i++)　// 変数を＋１する
            {
                Destroy(itemButtonDetailList[i].gameObject);
            }
        }
        itemButtonDetailList.Clear();
    }
}