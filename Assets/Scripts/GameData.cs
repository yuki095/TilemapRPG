using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;      // 統合言語クエリ

public class GameData : MonoBehaviour
{
    public static GameData instance;    // シングルトンデザインパターンにするための変数

    public int randomEncountRate;       // エンカウントの発生率

    public bool isEncounting;           // エンカウントしているかの判定

    public bool isDebug;                // デバッグ用の変数（Trueならエンカウント状態をリセットできる）

    private Vector3 currentPlayerPos;       // エンカウント時のプライヤーの位置

    private Vector2 currentLookDirection;   // エンカウント時のプレイヤーの向き

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // シーン遷移しても破壊されないオブジェクト
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// エンカウント時のプレイヤー位置と向きの情報を保持
    /// </summary>
    /// <param name="encountPlayerPos"></param>
    /// <param name="encountLookDirection"></param>
    public void SetEncountPlayerPosAndDirection(Vector3 encountPlayerPos,Vector2 encountLookDirection){
        // エンカウント時のプレイヤー位置と向きを変数に保持
        currentPlayerPos = encountPlayerPos;
        currentLookDirection = encountLookDirection;

        Debug.Log("プレイヤーのエンカウント位置情報更新");
    }

    public Vector3 GetCurrentPlayerPos()
    {
        // 保持している情報を戻す
        return currentPlayerPos;
    }

    public Vector2 GetCurrentLookDirection()
    {
        return currentLookDirection;
    }
}
