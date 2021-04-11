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
    private Vector3 currentPlayerPos;       // エンカウント時のプレイヤーの位置
    private Vector2 currentLookDirection;   // エンカウント時のプレイヤーの向き

    [System.Serializable]
    public class ItemInventryData
    {
        public ItemName itemName;   // アイテムの名前
        public int count;           // 所持数
        public int number;          // 所持している通し番号
    }

    [Header("所持アイテムのリスト")]
    public List<ItemInventryData> itemInventryDatasList = new List<ItemInventryData>();

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

    /// <summary>
    /// ItemInventryDatasListの最大数を取得　（アイテムの所持数）
    /// </summary>
    /// <returns></returns>
    public int GetItemInventryListCount()
    {
        return itemInventryDatasList.Count;
    }

    /// <summary>
    /// ItemInventryDatasListの中から指定した番号のIteminventryData情報を取得
    /// </summary>
    /// <param name="no"></param>
    /// <returns></returns>
    public ItemInventryData GetItemInventryData(int no)
    {
        return itemInventryDatasList[no];
    }
}
