using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    private void Start()
    {
        // PlayerControllerクラスにEncountManagerクラスの情報を渡す
        playerController.SetUpPlayerController(this);
    }

    public void JudgeRondomEncount()
    {
        if (GameData.instance.isEncounting)    // エンカウント状態の場合は
        {
            return; 　// ここで処理を終わらせる（エンカウントしない）
        }

        // 指定された範囲からランダムの値を代入する（0〜GameDataクラスで指定したエンカウント発生率）
        int encountRate = Random.Range(0, GameData.instance.randomEncountRate);

        if (encountRate == 0)   // エンカウント発生率の値が0なら（0〜100のうち0が代入された場合）
        {
            Debug.Log("エンカウント : " + encountRate);

            // GameDataクラスのisEncountingをtrue（エンカウント状態）にする
            GameData.instance.isEncounting = true;

            // プレイヤーの位置と方向の情報を保存
            GameData.instance.SetEncountPlayerPosAndDirection(playerController.transform.position, playerController.GetLookDirection());

            // バトルシーンへ遷移
            SceneStateManager.instance.NextScene(SceneStateManager.SceneType.Battle);

        }
    }

    private void Update()
    {
        // GameDataクラスのisDebugがtrueの時、LeftShitキーが入力された場合
        if (Input.GetKeyDown(KeyCode.LeftShift) && GameData.instance.isDebug){
            Debug.Log("エンカウント終了");

            GameData.instance.isEncounting = false;
        }
    }
}
