using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonDetail : MonoBehaviour
{
    [SerializeField]
    private Text txtItemName;   // アイテムの名前

    [SerializeField]
    private Text txtItemInfo;   // アイテムの説明

    [SerializeField]
    private Text txtItemCount;  // アイテムの所持数

    [SerializeField]
    private Image imgItem;      // アイテムのImage画像

    [SerializeField]
    private Button btnItem;

    [SerializeField]
    private ItemDataSO.ItemData itemData;

    /// <summary>
    /// ItemDataDetailの設定（外部のスクリプトから呼び出される前提のメソッド）
    /// </summary>
    /// <param name="itemData">アイテムのデータ</param>
    /// <param name="count">アイテムの所持数</param>
    public void SetUpItemButtonDetail(ItemDataSO.ItemData itemData, int count)
    {
        this.itemData = itemData;
        txtItemName.text = this.itemData.itemName.ToString();
        txtItemCount.text = count.ToString();
        imgItem.sprite = this.itemData.itemSprite;
    }

    /// <summary>
    /// ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActiveItemButtonDetail(bool isSwitch)
    {
        imgItem.enabled = isSwitch;         // コンポーネントのon/offを切り替える
        btnItem.interactable = isSwitch;    // ボタンの活性化/非活性化
    }

    /// <summary>
    /// アイテムボタンが押されたときの処理
    /// </summary>
    public void OnSelected()
    {
        txtItemInfo.text = this.itemData.itemInfo;
    }

    /// <summary>
    /// アイテムボタンから移動したときの処理
    /// </summary>
    public void OnDeselected()
    {
        txtItemInfo.text = "";  // 説明文を空欄に
    }
}
