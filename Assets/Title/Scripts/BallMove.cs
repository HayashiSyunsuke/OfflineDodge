using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    // 遅延
    [SerializeField] float delay;
    // タイマー
    float timer;

    // 計算結果保存用座標変数
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        Initailize();       
    }

    void Initailize()
    {
        // 右上からスタート
        transform.position = new Vector3(2100, 1200, 0);
        timer = 0f;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // タイマー更新
        timer++;

        // 演出を開始するかどうか
        if (timer <= delay) { return; }

        pos.x -= Mathf.Abs(pos.x - 990) / 20f;
        pos.y -= Mathf.Abs(pos.y - 740) / 20f;

        // 適応
        this.transform.position = pos;
    }
}
