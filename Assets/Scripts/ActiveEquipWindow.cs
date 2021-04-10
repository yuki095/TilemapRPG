using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateEquipWindow : MonoBehaviour
{
	// 装備画面を操作中にステータス画面を閉じた時のため

	// 装備画面がアクティブになった時に初期化処理をする
	void OnEnable()
	{
		// EquipAreaを有効化
		transform.Find("EquipArea").GetComponent<CanvasGroup>().interactable = true;

		// MenuAreaを有効化
		transform.Find("MenuArea").GetComponent<CanvasGroup>().interactable = true;

		// アイテムボタンを無効化
		transform.Find("EquipItemArea").GetComponent<CanvasGroup>().interactable = false;
	}
}