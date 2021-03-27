using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    [Header("会話イベント判定用")]
    public bool isTalking;      // Trueの場合は会話イベント中

    private DialogController dialogController;   // DialogControllerスクリプトの情報を代入するための変数

    private Vector3 defaultPos;
    private Vector3 offsetPos;

    private EventData.EventType eventType = EventData.EventType.Talk;   // NPCとの会話イベントとして設定

    [SerializeField, Header("NPC会話イベントの通し番号")]
    private int npcTalkEventNo;

    [SerializeField, Header("NPC会話イベントのデータ")]
    private EventData eventData;

    private void Start()
    {
        // このNonPlayerCharacterスクリプトがアタッチされているオブジェクトの、子オブジェクトにアタッチされている
        // DialogControllerスクリプトを取得して代入
        dialogController = GetComponentInChildren<DialogController>();

        // 会話ウィンドウの位置を取得して代入
        defaultPos = dialogController.transform.position;

        // 会話ウィンドウのy軸を-3.0f移動させて代入
        offsetPos = new Vector3(dialogController.transform.position.x, dialogController.transform.position.y - 3.0f, dialogController.transform.position.z);

        // DataBaseManagerに登録したスクリプタブル・オブジェクトを検索し、指定した通し番号のEventDataを取得して代入
        eventData = DataBaseManager.instance.GetEventDataFromNPCEvent(npcTalkEventNo);
    }

    /// <summary>
    /// 会話開始
    /// </summary>
    /// <param name="playerPos"></param>
    public void PlayTalk(Vector3 playerPos)
    {
        // 会話イベントを行っている状態にする
        isTalking = true;

        if (playerPos.y < transform.position.y)   // プレイヤーがNPCの位置より下にいる場合
        {
            dialogController.transform.position = offsetPos;  // 会話ウィンドウを-3.0fの位置に表示
        }
        else　// NPCより上にいる場合
        {   
            dialogController.transform.position = defaultPos;  // 会話ウィンドウを元の位置に表示
        }

        // 会話イベントのウィンドウを表示
        dialogController.DisplayDialog(eventData);

        // Debug.Log("会話ウィンドウを開く");
    }

    /// <summary>
    /// 会話終了
    /// </summary>
    public void StopTalk()
    {
        // 会話イベントを行っていない状態にする
        isTalking = false;

        // 会話イベントのウィンドウを閉じる
        dialogController.HideDialog();

        // Debug.Log("会話ウィンドウを閉じる");
    }

}
