using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Button btnBattleEnd;

    private void Start()
    {
        // ボタンのOnClickイベントにOnClickBattleEndメソッドを追加する
        // ボタンを押下したときに実行するメソッド（この時点では実行されない）
        btnBattleEnd.onClick.AddListener(onClickBattleEnd);
    }

    /// <summary>
    /// バトル終了ボタン押下時の処理
    /// </summary>
    private void onClickBattleEnd()
    {
        SceneStateManager.instance.NextScene(SceneStateManager.SceneType.Main);
    }
}
