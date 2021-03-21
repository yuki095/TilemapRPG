using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;

    private Rigidbody2D rb;     // Rigidbody2Dコンポーネントを取得するための変数

    private float horizontal;   // x軸方向への入力値
    private float vertical;     // y軸方向への入力値

    private Animator anim;      // Animatorコンポーネントを取得するための変数

    private Vector2 lookDirection = new Vector2(0, -1.0f);  // キャラの向きの情報の設定用

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   // Rigidbody2Dコンポーネントの情報を取得して代入
        anim = GetComponent<Animator>();    // Animatorコンポーネントの情報を取得して代入
    }

    void Update()
    {
        // InputManagerに登録してあるキーが入力されたら、x／y方向の入力値として代入
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        SyncMoveAnimation();       // キャラの向きと移動アニメの同期メソッド
    }

    void FixedUpdate()
    {
        Move();           // 移動メソッド
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        // velocity（速度）に新しい値を代入してゲームオブジェクトを移動
        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    }

    /// <summary>
    /// キャラの向きと移動アニメの同期
    /// </summary>
    private void SyncMoveAnimation()
    {
        // 移動のキー入力値を代入
        Vector2 move = new Vector2(horizontal, vertical);

        // move.x(y)の値が0.0fではないか、0.0fに近い値でない場合  = キー入力がある場合
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);      // 向きを更新（Vector2.Setメソッド）
            lookDirection.Normalize();              // 正規化 0か1にする
        }

        // キーの入力値とBlendTreeで設定した移動アニメ用の値を確認し、アニメを再生
        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);

        // 停止アニメ用
        // anim.SetFloat("Speed", move.magnitude);
    }
}
