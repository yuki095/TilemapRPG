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

    [SerializeField]
    private EventData eventData;   // NonPlayerCharacterスクリプトから届くEventDataの情報を代入するための変数

    private bool isTalk;      // 会話中である場合trueになる、会話状態を表現する変数

    private NonPlayerCharacter nonPlayerCharacter;

    private float wordSpeed = 0.1f;　　 // 1文字当たりの表示速度(小さいほど早く表示される)

    private void Start()
    {
        SetUpDialog();      // ダイアログの設定
    }

    /// <summary>
    /// ダイアログの設定
    /// </summary>
    public void SetUpDialog()
    {
        canvasGroup.alpha = 0.0f;           // 透明度を0にする

        // txtTitleName.text = titleName;

        eventData = null;                   // EventDataを初期化
    }

    /// <summary>
    /// ダイアログの表示
    /// </summary>
    public IEnumerator DisplayDialog(EventData eventData, NonPlayerCharacter nonPlayerCharacter)
    {
        if (this.nonPlayerCharacter == null)
        {　
            this.nonPlayerCharacter = nonPlayerCharacter;
        }

        if (this.eventData == null)
        {
            this.eventData = eventData;
        }

        canvasGroup.DOFade(1.0f, 0.5f);      // フェードイン　0.5秒かけて透明度を1に

        txtTitleName.text = this.eventData.title;

        // 会話イベント開始
        isTalk = true;

        // メッセージ表示
        yield return StartCoroutine(PlayTalkEventProgress(this.eventData.dialogs));

        // 会話イベント終了
        isTalk = false;

        // Dialog を閉じる
        nonPlayerCharacter.StopTalk();

        // 1ページだけのとき用
        // 1文字ずつ表示する（表示間隔を一定にする）
        // txtDialog.DOText(this.eventData.dialog, 1.0f).SetEase(Ease.Linear);  

        // TODO 画像データ　Image型の変数にeventData.eventSpriteを代入
    }

    /// <summary>
    /// ダイアログの非表示
    /// </summary>
    public void HideDialog()
    {
        if (isTalk)
        {
            return;
        }

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

        // 獲得した宝箱の番号をセーブ
        GameData.instance.SaveSearchEventNum(treasureBox.treasureEventNo);

        // 所持しているアイテムのセーブ
        GameData.instance.SaveItemInventryDatas();

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

    /// <summary>
    /// 会話イベントのメッセージ再生とページ送り
    /// </summary>
    /// <param name="dialogs"></param>
    /// <returns></returns>
    private IEnumerator PlayTalkEventProgress(string[] dialogs)
    {
        bool isClick = false;

        // 複数のメッセージを順番に表示
        foreach (string dialog in dialogs)
        {
            Debug.Log(dialog);
            isClick = false;

            // １ページ分の文字を１文字当たり0.1秒ずつかけて等速で表示
            // 表示終了後、isClick を true にして文字が全文表示された状態にする
            txtDialog.DOText(dialog, dialog.Length * wordSpeed).SetEase(Ease.Linear).OnComplete(() => { isClick = true; });

            // １ページの文字が全文表示されている場合かつ、アクションボタンを押すと次のメッセージ表示
            // それまでは処理を中断して待機する（WaitUntil）
            yield return new WaitUntil(() => Input.GetButtonDown("Action") && isClick);

            // 次のページのために、現在表示されている文字を消去
            txtDialog.text = "";

            yield return null;
        }
    }
}
