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
    public InformationManager informationManager;

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
            // アイテムのボタンを生成
            ItemButtonDetail itemButtonDetail = Instantiate(itemButtonDetailPrefab, itemAreaTran, false);
            // 所持しているアイテムの通し番号を引数で指定して
            GameData.ItemInventryData itemInventryData = GameData.instance.GetItemInventryData(i);
            // アイテムボタンの設定（第一引数でアイテムデータを取得、第二引数で所持数を取得）
            itemButtonDetail.SetUpItemButtonDetail(DataBaseManager.instance.GetItemDataFromItemName(itemInventryData.itemName), itemInventryData.count, informationManager);
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