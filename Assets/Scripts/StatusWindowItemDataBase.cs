using System.Collections;
using UnityEngine;

public class StatusWindowItemDataBase : MonoBehaviour
{
    public enum Item
    {
        Sword,
        HandGun,
        ShotGun,
        UseItem
    };

    [SerializeField]
    private StatusWindowItemData[] itemLists = new StatusWindowItemData[4];

    private void Awake()
    {
        // アイテムの全情報を作成
        itemLists[0] = new StatusWindowItemData(Resources.Load("FlashLight", typeof(Sprite)) as Sprite, "懐中電灯", Item.Sword, "あれば便利な辺りを照らすライト");
        itemLists[1] = new StatusWindowItemData(Resources.Load("Sword", typeof(Sprite)) as Sprite, "ナイフ", Item.Sword, "切れ味するどいナイフ");
        itemLists[2] = new StatusWindowItemData(Resources.Load("Sword", typeof(Sprite)) as Sprite, "ブロードソード", Item.Sword, "大剣");
        itemLists[3] = new StatusWindowItemData(Resources.Load("Gun", typeof(Sprite)) as Sprite, "ハンドガン", Item.HandGun, "威力抜群ハンドガン");
    }

    public StatusWindowItemData[] GetItemData()
    {
        return itemLists;
    }

    public int GetItemTotal()
    {
        return itemLists.Length;
    }
}
