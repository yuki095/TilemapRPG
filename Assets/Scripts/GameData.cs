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

        /// <summary>
        /// ItemInventryDataクラスのコンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="num"></param>
        public ItemInventryData(ItemName name, int value, int num)
        {
            itemName = name;
            count = value;
            number = num;
        }
    }

    [Header("所持アイテムのリスト")]
    public List<ItemInventryData> itemInventryDatasList = new List<ItemInventryData>();

    [Header("獲得済みの探索イベントの番号")]
    public List<int> getSearchEventNumsList = new List<int>();

    private const string getSearchEventNumKey = "getSearchEventNumkey_";

    /// <summary>
    /// 会話ウインドウの種類
    /// </summary>
    public enum TalkWindowType
    {
        Fixed,   // 固定型
        Movable  // 稼働型
    }

    [Header("会話ウインドウの種類を設定")]
    public TalkWindowType useTalkWindowType;

    [Header("クリア済の会話イベントの番号")]
    public List<int> clearTalkEventNumsList = new List<int>();

    private const string clearTalkEventNumKey = "getTalkEventNumKey_";

    public int money;   // お金


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
        // 所持数が0より大きい場合、iの値を1ずつ増やす＝アイテム1種類ごとに下記の情報をセーブする
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
        // 以前所持していた番号順（小さい数から）で所持アイテムの並びをソート
        itemInventryDatasList = itemInventryDatasList.OrderBy(x => x.number).ToList();

        Debug.Log("ItemInventry ロード完了");
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

        // デバッグ用ロード
        if (Input.GetKeyDown(KeyCode.I) && isDebug)
        {
            // 追加・加算したいアイテムの名前と数を引数に指定してメソッドを呼び出す
            AddItemInventryData(ItemName.ひのきの棒, 1);    // 引数を変更することで追加・加算するアイテムを指定
        }

        // デバッグ用　所持しているアイテムの減算
        if (Input.GetKeyDown(KeyCode.O) && isDebug)
        {
            // 減算したいアイテムの名前と数を引数に指定してメソッドを呼び出す（除算後の所持数が0以下になったら削除）
            RemoveItemInventryData(ItemName.ひのきの棒, 1);    // 引数を変更することで減算するアイテムを指定
        }

        if (Input.GetKeyDown(KeyCode.P) && isDebug)
        {
            LoadClearTalkEventNums();
        }
    }

    /// <summary>
    /// ItemInvetryData を追加・加算
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="amount"></param>
    public void AddItemInventryData(ItemName itemName, int amount = 1)
    {
        // List の要素を１つずつ確認して、すでに所持しているアイテムかどうか確認
        foreach (ItemInventryData itemInventryData in itemInventryDatasList)
        {
            // 所持しているアイテムの場合
            if (itemInventryData.itemName == itemName)
            {
                // 所持数を加算
                itemInventryData.count += amount;

                Debug.Log("リストに対象アイテムを加算 : " + itemName + " / 合計 : " + itemInventryData.count + " 個");

                // 処理を終了
                return;
            }
        }
        // 所持していないアイテムの場合、新しく所持アイテムとして追加
        itemInventryDatasList.Add(new ItemInventryData(itemName, amount, itemInventryDatasList.Count));

        Debug.Log("リストに対象アイテムを新規追加 : " + itemName + " / " + amount + " 個");
    }

    /// <summary>
    /// ItemInventryData を減算、0以下になったら削除
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="amount"></param>
    public void RemoveItemInventryData(ItemName itemName, int amount = 1)
    {
        // Listの要素を１つずつ確認して、すでに所持しているアイテムかどうか確認
        foreach (ItemInventryData itemInventryData in itemInventryDatasList)
        {
            // 所持しているアイテムの場合
            if (itemInventryData.itemName == itemName)
            {
                // 所持数を減算
                itemInventryData.count -= amount;

                Debug.Log("リストに対象アイテムを減算 : " + itemName + " / 合計 : " + itemInventryData.count + " 個");

                // 所持数が0以下になったら
                if (itemInventryData.count <= 0)
                {
                    // 所持アイテムから削除する
                    itemInventryDatasList.Remove(itemInventryData);

                    Debug.Log("リストから対象アイテムを削除 : " + itemName);
                }
                return;
            }
        }

        Debug.Log("リストに対象アイテムなし");
    }

    /// <summary>
    /// 獲得した探索イベントの番号を保持
    /// </summary>
    public void AddSearchEventNum(int searchEventNum)
    {
        // 引数に指定した探索イベントの番号が、getSearchEventNumsList内に存在しているかどうかチェック（重複登録を防ぐ）
        // 存在しない場合
        if (!getSearchEventNumsList.Contains(searchEventNum))
        {
            // 獲得した探索イベントの番号を追加
            getSearchEventNumsList.Add(searchEventNum);
        }
    }

    /// <summary>
    /// 獲得しているすべての探索イベントの番号をセーブ
    /// </summary>
    public void SaveAllGetSearchEventNums()
    {
        for (int i = 0; i < getSearchEventNumsList.Count; i++)
        {
            PlayerPrefs.SetInt(getSearchEventNumKey + getSearchEventNumsList[i].ToString(), getSearchEventNumsList[i]);
        }

        PlayerPrefs.Save();

        Debug.Log("獲得済のすべての探索イベント セーブ完了");
    }

    /// <summary>
    /// 獲得した探索イベントの番号をセーブ
    /// </summary>
    /// <param name="searchEventNum"></param>
    public void SaveSearchEventNum(int searchEventNum)
    {
        PlayerPrefs.SetInt(getSearchEventNumKey + searchEventNum.ToString(), searchEventNum);
        PlayerPrefs.Save();

        Debug.Log("獲得済の探索イベントの番号 : " + searchEventNum + " : セーブ完了");
    }

    /// <summary>
    /// 獲得している探索イベントの番号をロード
    /// </summary>
    public void LoadGetSearchEventNums()
    {
        for (int i = 0; i < DataBaseManager.instance.GetEventDataSOCount(); i++)
        {
            int value = PlayerPrefs.GetInt(getSearchEventNumKey + i.ToString(), -1);
            if (value != -1)
            {
                getSearchEventNumsList.Add(value);
            }
        }
    }

    /// <summary>
    /// 会話イベントに必要なアイテムを所持しているか確認
    /// </summary>
    /// <param name="checkItemName"></param>
    /// <param name="checkCount"></param>
    /// <returns></returns>
    public bool CheckTalkEventItemFromItemInvenry(ItemName checkItemName, int checkCount)
    {
        // アイテム名で判定
        var itemInventry = itemInventryDatasList.Find(x => x.itemName == checkItemName);

        // お金の場合
        if (checkItemName == ItemName.お金)
        {
            // 必要な値を超えているか判定。必要な値を超えている場合は true
            return money >= checkCount ? true : false;
        }

        // インベントリーが null ではなく、必要な数を超えている場合は true
        return itemInventry == null ? false : itemInventry.count >= checkCount ? true : false;
    }

    /// <summary>
    /// クリアした会話イベントを List に追加
    /// </summary>
    /// <param name="talkEventNum"></param>
    public void AddClearTalkEventNum(int talkEventNum)
    {
        clearTalkEventNumsList.Add(talkEventNum);
    }

    /// <summary>
    /// クリア済の会話イベントの確認
    /// </summary>
    /// <param name="talkEventNum"></param>
    /// <returns></returns>
    public bool CheckClearTalkEventNum(int talkEventNum)
    {
        return clearTalkEventNumsList.Contains(talkEventNum);
    }

    /// <summary>
    /// クリアした会話イベントの番号をセーブ
    /// </summary>
    /// <param name="talkEventNum"></param>
    public void SaveClearTalkEventNum(int talkEventNum)
    {
        PlayerPrefs.SetInt(clearTalkEventNumKey + talkEventNum.ToString(), talkEventNum);
    }

    /// <summary>
    /// クリアしている会話イベントの番号のロード
    /// </summary>
    public void LoadClearTalkEventNums()
    {
        for (int i = 0; i < DataBaseManager.instance.GetEventDataSOCount(); i++)
        {
            int value = PlayerPrefs.GetInt(clearTalkEventNumKey + i.ToString(), -1);
            if (value != -1)
            {
                clearTalkEventNumsList.Add(value);
            }
        }
    }

    /// <summary>
    /// お金を計算
    /// </summary>
    /// <param name="amount"></param>
    public void CalculateMoney(int amount)
    {
        money += amount;

        // TODO お金のセーブメソッド作成
    }
}
