using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateItemButton : MonoBehaviour
{	
	private StatusWindowStatus statusWindowStatus;	// 主人公キャラクターのステータス
    private StatusWindowItemDataBase statusWindowItemDataBase; // アイテムデータベース
	public GameObject itemPrefab;	// アイテムボタンのプレハブ
	private GameObject[] item;	// アイテムボタンを入れておくゲームオブジェクト

	// ゲームオブジェクトがアクティブになったとき実行
	void OnEnable()
	{
		statusWindowStatus = Camera.main.GetComponent<StatusWindowStatus>();
		statusWindowItemDataBase = Camera.main.GetComponent<StatusWindowItemDataBase>();
		item = new GameObject[statusWindowItemDataBase.GetItemTotal()];

		// アイテム総数分アイテムボタンを作成
		for (var i = 0; i < statusWindowItemDataBase.GetItemTotal(); i++)
		{
			item[i] = GameObject.Instantiate(itemPrefab) as GameObject;
			item[i].name = "Item" + i;

			// アイテムボタンの親要素をこのスクリプトが設定されているゲームオブジェクトにする
			item[i].transform.SetParent(transform);

			// アイテムを持っているかどうか
			if (statusWindowStatus.GetItemFlag(i))
			{
				// アイテムデータベースの情報からスプライトを取得しアイテムボタンのスプライトに設定
				item[i].transform.GetChild(0).GetComponent<Image>().sprite = statusWindowItemDataBase.GetItemData()[i].GetItemSprite();
			}
			else
			{
				// アイテムボタンのUI.Imageを不可視にし、マウスやキー操作で移動しないようにする
				item[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
				item[i].transform.GetChild(0).GetComponent<Button>().interactable = false;
			}

			// ボタンにユニークな番号を設定（アイテムデータベース番号と対応）
			item[i].transform.GetChild(0).GetComponent<ItemButton>().SetItemNum(i);
		}
	}

	void OnDisable()
	{
		// ゲームオブジェクトが非アクティブになる時に作成したアイテムボタンインスタンスを削除
		for (var i = 0; i < statusWindowItemDataBase.GetItemTotal(); i++)
		{
			Destroy(item[i]);
		}
	}
}