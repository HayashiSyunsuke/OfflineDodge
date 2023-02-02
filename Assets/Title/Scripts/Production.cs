using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Production : MonoBehaviour
{
    // 演出開始までの遅延
    [Tooltip("起動から演出開始までの遅延  1秒 = 60")]
    [SerializeField] float delay;

    // 施す演出のフラグ
    [Tooltip("拡大演出の切り替え")]
    [SerializeField] bool isExpansion;
    [Tooltip("回転演出の切り替え")]
    [SerializeField] bool isRotate;
    [Tooltip("移動演出の切り替え")]
    [SerializeField] bool isMovement;

    // 演出開始用タイマー
    float timer;

    // 拡大演出に使うスケール
    Vector3 scale;
    // 回転演出に使う角度
    float angle;
    float angle2;

    // 拡大演出によりどこまで拡大するか
    [Tooltip("どこまで拡大するか(isExpansionがtrueの場合のみ適応)")]
    [SerializeField] float scaleSize;

    // 回転演出により何度(角度)回転するか
    [Tooltip("何回転するか(isRotateがtrueの場合のみ適応)")]
    [SerializeField]　int rotateNum;

    // 生成
    private void Start()
    {
        // 初期化関数
        this.Initialize();

        // 拡大演出ONなら初期サイズを0に
        if (isExpansion)
        {
            transform.localScale = new Vector3(0, 0, 0);
        }

        // 回転演出ONなら回転する角度を算出
        if (isRotate)
        {
            angle = rotateNum * 360f;
            angle2 = 0f;
        }
    }

    // 初期化
    private void Initialize()
    {
        // タイマーの初期化
        timer = 0.0f;
        // 演出に使う一時的なスケール保存変数初期化
        scale = new Vector3(0, 0, 0);
    }

    // 更新
    private void Update()
    {
        // タイマー更新
        timer++;

        // 演出を開始するかどうか
        if (timer <= delay) { return; }

        // 拡大フラグONなら
        if (isExpansion) { this.Expansion(); }

        // 回転フラグONなら
        if (isRotate) { this.Rotate(); }
    }

    // 拡大演出関数
    private void Expansion()
    {
        scale.x += Mathf.Abs(scaleSize - scale.x) / 20f;
        scale.y += Mathf.Abs(scaleSize - scale.y) / 20f;
        scale.z += Mathf.Abs(scaleSize - scale.z) / 20f;
        // 適応
        this.transform.localScale = scale;
    }

    // 回転演出関数
    private void Rotate()
    {
        //angle2 = Mathf.Abs(angle - angle2) / 20;

        //transform.localRotation = new Quaternion(0,0,angle2,0);
    }
}
