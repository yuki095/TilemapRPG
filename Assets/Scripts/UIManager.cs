using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private DialogController dialogController;

    void Start()
    {
        SetUpTalkWindow();
    }

    /// <summary>
    /// 固定型の会話ウインドウの設定
    /// </summary>
    public void SetUpTalkWindow()
    {
        // キャンバスを透明化
        dialogController.SetUpDialog();
    }

    /// <summary>
    /// 固定型の会話ウインドウを表示
    /// </summary>
    /// <param name="eventData"></param>
    public void OpenTalkWindow(EventData eventData, NonPlayerCharacter nonPlayerCharacter)
    {
        StartCoroutine(dialogController.DisplayDialog(eventData, nonPlayerCharacter));
    }

    /// <summary>
    /// 固定型の会話ウインドウを非表示
    /// </summary>
    public void CloseTalkWindow()
    {
        dialogController.HideDialog();
    }

    public void OpenSearchWindow(EventData eventData, TreasureBox treasureBox)
    {
        StartCoroutine(dialogController.DisplaySearchDialog(eventData, treasureBox));
    }
}