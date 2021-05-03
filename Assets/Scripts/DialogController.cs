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

    private int currentTalkCount;   // 会話回数（bool型でもOK）

    // ？？
    private EventData.EventDataDetail eventDataDetail;
    private bool isClick;

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

        // ノーマル会話イベントの場合（進行形でない）
        if (this.eventData.eventDataDetailsList.Exists(x => x.eventProgressType == EventProgressType.None))
        {
            // TODO 画像データがある場合
            // Image型の変数を宣言フィールドで用意しておいて、EventDataDetailクラスにあるeventSpriteを代入する

            // メッセージを表示
            yield return StartCoroutine(PlayTalkEventProgress(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.None).dialogs));

            // 進行型の会話イベントの場合
        }
        else
        {
            // 会話イベントをクリア済みか確認
            if (GameData.instance.CheckClearTalkEventNum(this.eventData.no))
            {
                // TODO 画像

                // クリア後の会話イベント
                yield return StartCoroutine(PlayTalkEventProgress(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.Cleard).dialogs));

            }
            // まだクリアしていない場合
            else
            {
                // イベントの種類を特定
                // 消費するタイプか、持っているだけでよいタイプか判定するために、EventDataDetailを取得
                EventData.EventDataDetail talkEventDataDetail = this.eventData.eventDataDetailsList.Find(x => (x.eventProgressType == EventProgressType.Need || x.eventProgressType == EventProgressType.Permission));

                // 初めての会話の場合（会話数が0の場合）
                if (currentTalkCount == 0)
                {
                    // TODO 画像

                    // クリア前の会話イベント
                    yield return StartCoroutine(PlayTalkEventProgress(talkEventDataDetail.dialogs));

                    currentTalkCount++; // 会話数を増やす　＋１

                    // ２回目以降の場合
                }
                else
                {
                    // 会話イベントを達成しているかどうか判定する（最初はfalse）
                    bool isNeedItems = false;

                    // 必要なアイテムを持っているかどうかを１つずつ確認
                    for (int i = 0; i < talkEventDataDetail.eventItemNames.Length; i++)
                    {
                        // 条件をクリアしていない場合
                        if (!GameData.instance.CheckTalkEventItemFromItemInvenry(talkEventDataDetail.eventItemNames[i], talkEventDataDetail.eventItemCounts[i]))
                        {
                            // チェックを終了して、持っていない判定にする
                            break;
                        }
                        else
                        {
                            // 条件をクリアしている場合、かつ最後の確認
                            if (i == talkEventDataDetail.eventItemNames.Length - 1)
                            {
                                // クリアに必要なすべてのアイテムを持っている判定にする
                                isNeedItems = true;
                            }
                        }
                    }

                    // クリアアイテムが必要数だけあるか確認
                    if (isNeedItems)
                    {
                        // すべて揃っていればクリア判定し、会話イベントを進める
                        // 初回クリアなら
                        if (!GameData.instance.CheckClearTalkEventNum(this.eventData.no))
                        {
                            // TODO 画像

                            // クリア達成時の会話イベント
                            yield return StartCoroutine(PlayTalkEventProgress(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.Get).dialogs));

                            // アイテム獲得
                            yield return StartCoroutine(GetEventItems(this.eventData.eventDataDetailsList.Find(x => x.eventProgressType == EventProgressType.Get)));

                            // 会話イベントのクリア状態を保存
                            GameData.instance.AddClearTalkEventNum(this.eventData.no);

                            // アイテムを消耗するイベントの場合
                            if (talkEventDataDetail.eventProgressType == EventProgressType.Need)
                            {
                                // 消耗対象をすべて確認
                                for (int i = 0; i < talkEventDataDetail.eventItemNames.Length; i++)
                                {
                                    // TODO 分岐を作成し、お金か、アイテムを減算するようにする
                                    if (talkEventDataDetail.eventItemNames[i] == ItemName.お金)
                                    {
                                        // お金を減算
                                        GameData.instance.CalculateMoney(-talkEventDataDetail.eventItemCounts[i]);
                                    }
                                    else
                                    {
                                        // アイテムを減算
                                        GameData.instance.RemoveItemInventryData(talkEventDataDetail.eventItemNames[i], talkEventDataDetail.eventItemCounts[i]);
                                    }
                                }
                            }

                            // クリアした会話イベントをセーブ 
                            GameData.instance.SaveClearTalkEventNum(this.eventData.no);

                            // インベントリの状態をセーブ
                            GameData.instance.SaveItemInventryDatas();

                            // TODO お金や経験値のセーブ

                        }

                        // クリアアイテムがない場合
                    }
                    else
                    {

                        // TODO 画像

                        // クリア前の会話イベント
                        yield return StartCoroutine(PlayTalkEventProgress(talkEventDataDetail.dialogs));
                    }
                }
            }
        }

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
    public IEnumerator DisplaySearchDialog(EventData eventData, TreasureBox treasureBox)
    {
        // 会話ウインドウを表示
        canvasGroup.DOFade(1.0f, 0.5f);

        // タイトルに探索物の名称を表示
        txtTitleName.text = eventData.title;

        // アイテム獲得
        yield return StartCoroutine(GetEventItems(eventData.eventDataDetailsList[0]));

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
    private IEnumerator GetEventItems(EventData.EventDataDetail eventData)
    {
        // 獲得したアイテムの種類分だけ繰り返す
        for (int i = 0; i < eventDataDetail.eventItemNames.Length; i++)
        {
            // 獲得したアイテムの名前と数を表示
            txtDialog.DOText(eventDataDetail.eventItemNames[i].ToString() + " × " + eventDataDetail.eventItemCounts[i] + " 獲得", 1.0f).SetEase(Ease.Linear).OnComplete(() => { isClick = true; });

            // 獲得した種類で分岐
            if (eventDataDetail.eventItemNames[i] == ItemName.お金)
            {
                // TODO お金の加算処理
                GameData.instance.CalculateMoney(eventDataDetail.eventItemCounts[i]);
            }
            else if (eventDataDetail.eventItemNames[i] == ItemName.経験値)
            {
                // TODO 経験値の加算処理

            }
            else
            {
                // アイテム獲得
                GameData.instance.AddItemInventryData(eventDataDetail.eventItemNames[i], eventDataDetail.eventItemCounts[i]);
            }

            // アクションボタンを押すと次のメッセージ表示
            yield return new WaitUntil(() => Input.GetButtonDown("Action") && isClick);
            txtDialog.text = "";
            yield return null;
        }
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
