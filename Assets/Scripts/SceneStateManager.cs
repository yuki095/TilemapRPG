using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;    // シーン遷移の管理

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    public enum SceneType   // シーンの種類
    {
        Main,
        Battle,
        // TODO 新しいシーンを作成したらここに追加
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 引数で指定したシーンへ遷移
    /// </summary>
    /// <param name="nextSceneType"></param>
    public void NextScene(SceneType nextSceneType)
    {
        // シーン名を指定する引数には、enumであるSceneTypeの列挙子を、ToStringメソッドを使ってString型へキャストして利用？
        SceneManager.LoadScene(nextSceneType.ToString());
    }
}