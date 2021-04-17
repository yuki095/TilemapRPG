using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OneLine;  // 追加したアセット

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    /// <summary>
    /// アイテムのデータ
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public int itemNo;              // アイテムの通し番号
        public Sprite itemSprite;       // アイテムのImage画像
        public ItemName itemName;       // アイテムの名前
        public ItemType itemType;       // アイテムの種類
        public string itemInfo;         // アイテムの情報

        // TODO データが増えたらここに追加
    }

    [OneLineWithHeader]
    // リストの初期化
    public List<ItemData> itemDataList = new List<ItemData>();
}

