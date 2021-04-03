using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour
{
    // インフォメーションテキストに表示する文字列
    [SerializeField]
    private string informationString;

    // インフォメーションテキスト
    [SerializeField]
    private Text informationText;

    private CanvasGroup canvasGroup;

    // 前の画面に戻るボタン　
    private GameObject returnButton;

    void Start()
    {
        // 自身の親のCanvasGroup
        canvasGroup = GetComponentInParent<CanvasGroup>();
        // Exitという名前のゲームオブジェクトを取得して代入
        returnButton = transform.parent.Find("Exit").gameObject;
    }

    // ウィンドウがアクティブになる度に呼び出されるメソッド
    private void OnEnable()
    {
        // 立ち上げ時にボタンを有効にする（interactableがfalseだとボタンが押せなくなる）
        GetComponent<Button>().interactable = true;
    }

    // オンマウス、またはキー操作で該当のボタンに移動したとき
    public void OnSelected()
    {
        if (canvasGroup == null || canvasGroup.interactable)
        {
            // イベントシステムのフォーカス（？）がこのゲームオブジェクトにない時
            if (EventSystem.current.currentSelectedGameObject != gameObject)
            {
                // このゲームオブジェクトにフォーカスさせる
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
            // インフォメーションテキストに文字列を表示
            informationText.text = informationString;
        }
    }

    // ボタンから移動したらテキストの情報を削除
    public void OnDeselected()
    {
        informationText.text = "";
    }

    public void DisableWindow()
    {
        if (canvasGroup == null || canvasGroup.interactable)
        {
            // ウィンドウを非アクティブにする
            transform.root.gameObject.SetActive(false);
        }
    }
    // 他の画面を表示する
    public void WindowOnOff(GameObject window)
    {
        if (canvasGroup == null || canvasGroup.interactable)
        {
            Camera.main.GetComponent<OperationStatusWindow>().ChangeWindow(window);
        }
    }
    // 前の画面に戻るボタンを選択状態にする
    public void SelectReturnButton()
    {
        EventSystem.current.SetSelectedGameObject(returnButton);
    }
}
