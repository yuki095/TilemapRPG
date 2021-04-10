﻿using System.Collections;
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

    [Header("会話イベント判定用")]
    public bool isTalking;     // 会話中かどうかの判定用

    private EncountManager encountManager;   // EncountManagerクラスの情報を代入するための変数

    [SerializeField]
    private GameObject propertyWindow;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   // Rigidbody2Dコンポーネントの情報を取得して代入
        anim = GetComponent<Animator>();    // Animatorコンポーネントの情報を取得して代入

        // エンカウント後なら（ゲーム開始時には実行しない）
        if (GameData.instance.isEncounting)
        {
            // エンカウント時のプレイヤーの位置に戻す
            transform.position = GameData.instance.GetCurrentPlayerPos();

            // エンカウント状態を解除
            GameData.instance.isEncounting = false;

            // 向きを取得してアニメを再現
            lookDirection = GameData.instance.GetCurrentLookDirection();
            anim.SetFloat("Look X", lookDirection.x);
            anim.SetFloat("Look Y",lookDirection.y);
        }
    }

    void Update()
    {
        Action();  　// アクション

        if (isTalking)     // 会話中の場合は 
        {
            return;        // ここで処理を終わらせる（移動キーの入力を受け付けない）
        }

        if (propertyWindow.activeSelf == true) // ステータス画面が開いている場合
        {
            return;
        }

        // InputManagerに登録してあるキーが入力されたら、x／y方向の入力値として代入
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        SyncMoveAnimation();       // キャラの向きと移動アニメの同期メソッド
    }

    void FixedUpdate()
    {
        if (propertyWindow.activeSelf == true)
        {
            rb.velocity = Vector2.zero;   // 速度を０にする
            return;
        }

        Move();           // 移動
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        // velocity（速度）に新しい値を代入してゲームオブジェクトを移動
        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);

        // プレイヤーのmagnitude（ベクトルの長さ）が0.5よりも大きく（プレイヤーが移動していて）、
        // encountManager変数にEncountManagerの情報が代入されている場合
        if (rb.velocity.magnitude > 0.5f && encountManager)
        {
            // ランダムエンカウントが発生しているかどうか判定
            encountManager.JudgeRondomEncount();
        }
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

    /// <summary>
    /// 行動ボタンを押したときの処理
    /// </summary>
    private void Action()
    {
        if (Input.GetButtonDown("Action"))
        {
            // プレイヤーの位置を起点として【rb.position】
            // プレイヤーの向いている方向【lookDirection】に1.0f分だけRayを発射
            // NPCレイヤーを持つオブジェクトに接触するか判定
            // その情報をhit変数に代入する
            RaycastHit2D hit = Physics2D.Raycast(rb.position, lookDirection, 1.0f, LayerMask.GetMask("NPC"));

            // SceneビューにてRayの可視化
            Debug.DrawRay(rb.position, lookDirection, Color.blue, 1.0f);

            // hit変数に、コライダーを持つオブジェクトの情報が代入されている場合
            if (hit.collider != null)
            {
                // そのオブジェクトにアタッチされているNonPlayerCharacterクラスが取得できた場合（trueならnpc変数にクラスの情報が代入される）
                if (hit.collider.TryGetComponent(out NonPlayerCharacter npc))
                {
                    // 取得したNonPlayerCharacterクラスをもつオブジェクトと会話中ではない場合
                    if (!npc.isTalking)  //!は否定？
                    {
                        npc.PlayTalk(transform.position);   // NPCとの会話イベント発生
                        isTalking = true;                   // プレイヤーを会話中にする
                    }
                    // 会話中である場合
                    else
                    {
                        npc.StopTalk();         // NPCとの会話イベント終了
                        isTalking = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// PlayerControllerに必要な外部のクラス情報を設定
    /// </summary>
    /// <param name="encountManager"></param>
    public void SetUpPlayerController(EncountManager encountManager)
    {
        // EncountManagerクラスの情報を取得して代入する
        this.encountManager = encountManager;
    }

    /// <summary>
    /// 向いている方向を戻す
    /// </summary>
    /// <returns></returns>
    public Vector2 GetLookDirection()
    {
        // 現在のキャラの向きの情報を渡す
        return lookDirection;
    }
}