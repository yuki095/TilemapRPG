using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
	// EquipButtonに保持するアイテムデータ
	private StatusWindowItemData statusWindowItemData;
	private Transform equipArea;
	private Transform menuArea;
	private Transform equipItemArea;
	private Text informationText;
	private SelectedEquipButton selectedEquipButton;
	private CanvasGroup canvasGroup;

	// この装備の番号
	[SerializeField]
	private int equipNum;

	// 戻るボタン
	private GameObject returnButton;

	void Start()
	{
		equipArea = transform.parent.parent;
		menuArea = transform.parent.parent.parent.Find("MenuArea");
		equipItemArea = transform.parent.parent.parent.Find("EquipItemArea");
		informationText = transform.parent.parent.parent.Find("Information/Text").GetComponent<Text>();
		selectedEquipButton = GetComponentInParent<SelectedEquipButton>();
		canvasGroup = equipArea.GetComponent<CanvasGroup>();
		returnButton = menuArea.Find("Exit").gameObject;
	}

	// EquipButtonを押した時
	public void OnClick()
	{
		if (canvasGroup.interactable)
		{
			// 選択中の装備スロットであることをわかるように背景色を変更する
			transform.parent.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 1f);

			// イベントシステムのセレクトをオフ
			EventSystem.current.SetSelectedGameObject(null);

			// EquipAreaを無効化
			equipArea.GetComponent<CanvasGroup>().interactable = false;

			// MenuAreaを無効化
			menuArea.GetComponent<CanvasGroup>().interactable = false;

			// アイテムボタンを有効化
			equipItemArea.GetComponent<CanvasGroup>().interactable = true;

			// 現在選択中の装備ボタンの番号をセットする
			selectedEquipButton.SetSelectedEquipButton(equipNum);

			// EquipItemAreaの最初のボタンをフォーカスする
			EventSystem.current.SetSelectedGameObject(equipItemArea.GetChild(0).GetChild(0).gameObject);
		}
	}

	// EquipButtonが選択された時
	public void OnSelected()
	{
		if (canvasGroup.interactable)
		{
			if (statusWindowItemData != null)
			{
				informationText.text = statusWindowItemData.GetItemInformation();
			}
			transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		}
	}

	// EquipButtonから移動したら情報を削除
	public void OnDeselected()
	{
		informationText.text = "";
	}

	// EquipItemButtonが押され装備スロットにアイテムをセット
	public void SetStatusWindowItemData(StatusWindowItemData itemData)
	{
		statusWindowItemData = itemData;
		GetComponent<Image>().sprite = statusWindowItemData.GetItemSprite();

		transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 100f / 255f);
	}

	void OnEnable()
	{
		GetComponent<Button>().interactable = true;
	}

	// キー操作でステータス画面を閉じた時は選択中のボタンを元に戻す
	public void OnDisable()
	{
		transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 100f / 255f);
	}

	// 前の画面に戻るボタンを選択状態にする
	public void SelectReturnButton()
	{
		EventSystem.current.SetSelectedGameObject(returnButton);
	}
}
