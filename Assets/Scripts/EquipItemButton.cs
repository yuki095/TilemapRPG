using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipItemButton : MonoBehaviour
{
	// EquipItemButtonに保持するアイテムデータ
	private StatusWindowItemData statusWindowItemData;
	private Transform equipArea;
	private Transform menuArea;
	private Transform equipItemArea;
	private Text informationText;

	// 現在選択している装備ボタンを保持するスクリプト
	private SelectedEquipButton selectedEquipButton;

	void Start()
	{
		equipArea = transform.parent.parent.parent.Find("EquipArea");
		menuArea = transform.parent.parent.parent.Find("MenuArea");
		equipItemArea = transform.parent.parent.parent.Find("EquipItemArea");
		informationText = transform.parent.parent.parent.Find("Information/Text").GetComponent<Text>();
		selectedEquipButton = equipArea.GetComponent<SelectedEquipButton>();
	}

	// EquipButtonを押した時
	public void OnClick()
	{
		if (GetComponentInParent<CanvasGroup>().interactable)
		{
			// イベントシステムのセレクトをオフ
			EventSystem.current.SetSelectedGameObject(null);

			// EquipAreaを有効化
			equipArea.GetComponent<CanvasGroup>().interactable = true;

			// MenuAreaを有効化
			menuArea.GetComponent<CanvasGroup>().interactable = true;

			// アイテムボタンを無効化
			equipItemArea.GetComponent<CanvasGroup>().interactable = false;

			var equipButton = equipArea.transform.GetChild(selectedEquipButton.GetSelectedEquipButton()).GetComponentInChildren<EquipButton>();
			equipButton.SetStatusWindowItemData(statusWindowItemData);

			// EquipAreaの最初のボタンをフォーカスする
			EventSystem.current.SetSelectedGameObject(equipArea.GetChild(selectedEquipButton.GetSelectedEquipButton()).GetChild(0).gameObject);
		}
	}

	//　EquipButtonが選択された時
	public void OnSelected()
	{
		if (GetComponent<Button>().interactable)
		{
			if (GetComponent<Image>().sprite != null)
			{
				informationText.text = statusWindowItemData.GetItemInformation();
			}
			transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		}
	}

	// EquipItemButtonから移動したら情報を削除
	public void OnDeselected()
	{
		informationText.text = "";
	}

	public void SetStatusWindowItemData(StatusWindowItemData itemData)
	{
		statusWindowItemData = itemData;
	}
}
