﻿using System.Collections;
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

        /// <summary>
        /// ItemInventryDataクラスのコンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="num"></param>
        public ItemInventryData(ItemInventryData name, int value, int num)
        {
            itemName = name;
            count = value;
            number = num;
        }
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

    /// <summary>
    /// 所持アイテムのセーブ
    /// </summary>
    public void SaveItemInventryDatas()
    {
        // 所持しているアイテムの数だけ処理を行う
        // 所持数が0より大きい場合、iの値を1ずつ増やす＝アイテム1個ごとに下記の情報をセーブする）
        for (int i = 0; i < itemInventryDatasList.Count; i++)
        {
            // 所持しているアイテムの情報（名前、所持数、通し番号）を１つの文字列としてセーブするための準備
            PlayerPrefs.SetString(itemInventryDatasList[i].itemName.ToString(), itemInventryDatasList[i].itemName.ToString() + "," + itemInventryDatasList[i].count.ToString() + "," + i.ToString());
        }

        // セーブ（SetStringメソッドで準備された情報をセーブする）
        PlayerPrefs.Save();

        Debug.Log("ItemInventry セーブ完了");
    }

    /// <summary>
    /// 所持アイテムのロード
    /// </summary>
    public void LoadItemInventryDatas()
    {
        // アイテムの所持数分だけ繰り返す
        for (int i = 0; i < DataBaseManager.instance.GetItemDataSoCount(); i++)
        {
            // ItemNameでセーブしてあるデータがPlayerPrefs内にない場合（HasKeyメソッドで判定）
            if (!PlayerPrefs.HasKey(DataBaseManager.instance.GetItemDataFromItemNo(i).itemName.ToString()))
            {
                // ここで処理を終了し、次のセーブデータを確認する処理へ映る
                continue;
            }

            // セーブされているデータを読み込んで配列に代入（Split＝,で情報を区切る）
            string[] stringArray = PlayerPrefs.GetString(DataBaseManager.instance.GetItemDataFromItemNo(i).itemName.ToString()).Split(',');

            // セーブデータからアイテムのデータをコンストラクタ・メソッドを利用して復元
            itemInventryDatasList.Add(new ItemInventryData((ItemName)Enum.Parse(typeof(ItemName),stringArray[0]),int.Parse(stringArray[1]),int.Parse(stringArray[2])));
        }
        // 以前所持していた番号順で所持アイテムの並びをソート
        itemInventryDatasList = itemInventryDatasList.OrderBy(x => x.number).ToList();

        Debug.Log("ItemTventry ロード完了");
    }

    // デバッグ用
    private void Update()
    {
        // デバッグ用セーブ
        if (Input.GetKeyDown(KeyCode.K) && isDebug)
        {
            SaveItemInventryDatas();
        }

        // デバッグ用ロード
        if (Input.GetKeyDown(KeyCode.L) && isDebug)
        {
            LoadItemInventryDatas();
        }
    }
}
