using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [Header("宝箱イベント判定用")]
    public bool isOpen;

    [SerializeField]
    private DialogController dialogController;

    private Vector3 defaultPos;

    private Vector3 offsetPos;

    private EventData.EventType eventType = EventData.EventType.Search;

    [Header("TalkWindowの使用許可")]
    private bool isFixedTalkWindowUsing;   // インスペクターで確認した後はprivate修飾子に変更してもよい

    private UIManager uiManager;　　　　　 // UIManager スクリプトの情報を代入するための変数

    [Header("宝箱イベントの通し番号")]
    public int treasureEventNo;

    [SerializeField, Header("宝箱イベントのデータ")]
    private EventData eventData;

    private SpriteRenderer spriteRenderer;

    private PlayerController playerController;

   // private void Start()
   // {
   //     SetUpTresureBox();    // 外部クラスより呼び出すのでコメントアウト
   // }

    /// <summary>
    /// 探索イベントの準備
    /// </summary>
    public void SetUpTresureBox()
    {
        isOpen = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        dialogController = GetComponentInChildren<DialogController>();

        defaultPos = dialogController.transform.position;
        offsetPos = new Vector3(dialogController.transform.position.x, dialogController.transform.position.y - 5.0f, dialogController.transform.position.z);

        // 対象物のEventDataを取得
        eventData = DataBaseManager.instance.GetEventDataFromEvent(treasureEventNo, eventType);
    }

    /// <summary>
    /// 探索開始
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="playerController"></param>
    public void OpenTresureBox(Vector3 playerPos, PlayerController playerController)
    {

        if (this.playerController == null)
        {
            this.playerController = playerController;
        }

        SwitchStateTreasureBox(true);

        if (playerPos.y < transform.position.y) // プレイヤーの位置が宝箱より下の場合
        {
            dialogController.transform.position = offsetPos;
        }
        else
        {
            dialogController.transform.position = defaultPos;
        }

        // 設定されている会話ウインドウの種類に合わせて、開く会話ウインドウを分岐させる
        // if (isFixedTalkWindowUsing)
        // {
        //    uiManager.OpenTalkWindow(eventData);　// 固定型の会話ウインドウを表示する
        // }
        // else
        // {
        //     dialogController.DisplayDialog(eventData); // 稼働型の会話イベントのウインドウを表示する
        // }

        // 探索イベント用の会話ウィンドウを開く
        // dialogController.DisplaySearchDialog(eventData, this);
        StartCoroutine(dialogController.DisplaySearchDialog(eventData, this));
    }

    /// <summary>
    /// 探索終了
    /// </summary>
    public void CloseTreasureBox()
    {
        Debug.Log("通過");

        playerController.IsTalking = false;

        // 設定されている会話ウインドウの種類に合わせて、会話イベントのウィンドウを閉じる
        if (isFixedTalkWindowUsing)
        {
            uiManager.CloseTalkWindow();
        }
        else
        {
            dialogController.HideDialog();
        }

        // 探索イベント用の会話ウィンドウを閉じる
        // dialogController.HideDialog();
    }

    /// <summary>
    /// 探索イベントの通し番号の取得
    /// </summary>
    /// <returns></returns>
    public int GetTresureEventNum()
    {
        return treasureEventNo;
    }

    /// <summary>
    /// 探索状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchStateTreasureBox(bool isSwitch)
    {
        isOpen = isSwitch;

        if (isOpen)
        {
            // 宝箱の画像を開封済にする
            spriteRenderer.sprite = eventData.eventDataDetailsList[0].eventSprite;

            // 宝箱自体を非表示にする場合
            // this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 固定型会話ウインドウを利用するための設定
    /// </summary>
    /// <param name="uiManager"></param>
    // public void SetUpFixedTalkWindow(UIManager uiManager)
    // {
    //    this.uiManager = uiManager;

    //    isFixedTalkWindowUsing = true;
    // }
}