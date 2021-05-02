using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateEquipButton : MonoBehaviour
{
	// 主人公キャラクターのステータス
	public StatusWindowStatus statusWindowStatus;

	// アイテムデータベース
	public StatusWindowItemDataBase statusWindowItemDataBase;

	// Equipボタンのプレハブ
	public GameObject equipButtonPrefab;

	// アイテムボタンを入れておくゲームオブジェクト
	private GameObject[] item;

	// ゲームオブジェクトがアクティブになった時実行
	void OnEnable()
	{
		// EquipItemAreaを無効化
		GetComponent<CanvasGroup>().interactable = false;

		// statusWindowStatus = Camera.main.GetComponent<StatusWindowStatus>();
		// statusWindowItemDataBase = Camera.main.GetComponent<StatusWindowItemDataBase>();
		item = new GameObject[statusWindowItemDataBase.GetItemTotal()];

		// アイテム総数分アイテムボタンを作成
		for (var i = 0; i < statusWindowItemDataBase.GetItemTotal(); i++)
		{
			// アイテムタイプが使用アイテム、またはアイテムを持っていない時は次に進む
			if (statusWindowItemDataBase.GetItemData()[i].GetItemType() == StatusWindowItemDataBase.Item.UseItem
				|| !statusWindowStatus.GetItemFlag(i))
			{
				continue;
			}

			item[i] = GameObject.Instantiate(equipButtonPrefab) as GameObject;
			item[i].name = "EquipItem" + i;

			// アイテムボタンの親要素をこのスクリプトが設定されているゲームオブジェクトにする
			item[i].transform.SetParent(transform);

			// アイテムデータベースの情報からスプライトを取得しアイテムボタンのスプライトに設定
			item[i].transform.GetChild(0).GetComponent<Image>().sprite = statusWindowItemDataBase.GetItemData()[i].GetItemSprite();

			// EquipItemAreaを無効化してからEquipButtonを有効化（ボタンが点滅しているように見えてしまう為）
			item[i].transform.GetChild(0).GetComponent<Button>().interactable = true;

			item[i].transform.GetChild(0).GetComponent<EquipItemButton>().SetStatusWindowItemData(statusWindowItemDataBase.GetItemData()[i]);
		}
	}

	void OnDisable()
	{
		// ゲームオブジェクトが非アクティブになる時に作成したアイテムボタンインスタンスを削除する
		for (var i = 0; i < statusWindowItemDataBase.GetItemTotal(); i++)
		{
			Destroy(item[i]);
		}
	}
}