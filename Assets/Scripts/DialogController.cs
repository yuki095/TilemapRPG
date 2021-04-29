using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private Text txtDialog = null;

    [SerializeField]
    private Text txtTitleName = null;

    [SerializeField]
    private CanvasGroup canvasGroup = null;

    // [SerializeField]
    // private string titleName = "dog";
    // [SerializeField]
    // private string dialog = "ワンワン！";

    [SerializeField]
    private EventData eventData;   // NonPlayerCharacterスクリプトから届くEventDataの情報を代入するための変数

    private void Start()
    {
        SetUpDialog();      // ダイアログの設定
    }

    /// <summary>
    /// ダイアログの設定
    /// </summary>
    private void SetUpDialog()
    {
        canvasGroup.alpha = 0.0f;           // 透明度を0にする

        // txtTitleName.text = titleName;

        eventData = null;                   // EventDataを初期化
    }

    /// <summary>
    /// ダイアログの表示
    /// </summary>
    public void DisplayDialog(EventData eventData)
    {
        this.eventData = eventData;

        canvasGroup.DOFade(1.0f, 0.5f);      // フェードイン　0.5秒かけて透明度を1に

        txtTitleName.text = this.eventData.title;

        txtDialog.DOText(this.eventData.dialog, 1.0f).SetEase(Ease.Linear);  // 1文字ずつ表示する（表示間隔を一定にする）

        // TODO 画像データ　Image型の変数にeventData.eventSpriteを代入
    }

    /// <summary>
    /// ダイアログの非表示
    /// </summary>
    public void HideDialog()
    {
        canvasGroup.DOFade(0.0f, 0.5f); 　　　// フェードアウト　0.5秒かけて透明度を0に
        txtDialog.text = "";                 // 空欄にする
    }

    /// <summary>
    /// 探索対象を獲得する
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="treasureBox"></param>
    /// <returns></returns>
    public void DisplaySearchDialog(EventData eventData, TreasureBox treasureBox)
    {
        // 会話ウインドウを表示
        canvasGroup.DOFade(1.0f, 0.5f);

        // タイトルに探索物の名称を表示
        txtTitleName.text = eventData.title;

        // アイテム獲得
        GetEventItems(eventData);

        // 獲得した宝箱の番号を GameData に追加
        GameData.instance.AddSearchEventNum(treasureBox.treasureEventNo);

        // TODO 獲得した宝箱の番号をセーブ
        // TODO 所持しているアイテムのセーブ
        // TODO お金や経験値のセーブ
    }

    /// <summary>
    /// アイテム獲得
    /// </summary>
    /// <param name="eventData"></param>
    private void GetEventItems(EventData eventData)
    {
        // 獲得したアイテムの名前と数を表示
        txtDialog.text = eventData.eventItemName.ToString() + " × " + eventData.eventItemCount + " 獲得";

        // GameDataにデータを登録（アイテム獲得の実処理）
        GameData.instance.AddItemInventryData(eventData.eventItemName, eventData.eventItemCount);
    }
}
