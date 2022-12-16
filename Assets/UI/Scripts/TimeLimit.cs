using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    //テキスト
    [SerializeField]
    Text text;

    //制限時間
    private float totalTime;

    //分
    [SerializeField]
    private int minutes;

    //時間
    [SerializeField]
    private float seconds;

    //前の秒数
    private float oldSeconds = 0;
    //開始フラグ
    private bool startFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        //秒数に直す
        totalTime = minutes * 60 + seconds;
    }

    // Update is called once per frame
    void Update()
    {
        //カウントダウン
        CountDownTimer();
    }

    //カウントダウンする
    private void CountDownTimer()
    {
        //開始していなければ処理しない
        if (startFlag == false)
            return;

        //時間が過ぎた時シーンを変える
        if (totalTime < 0)
        {
            //GetComponentInParent<ChangeScene>().ChangeFlag = true;
            return;
        }

        totalTime = minutes * 60 + seconds;
        totalTime -= Time.deltaTime;

        //各分、秒の計算
        minutes = (int)totalTime / 60;
        seconds = totalTime - minutes * 60;

        //テキストに( OO：OO )の形で描画する
        if (seconds != oldSeconds)
        {
            text.text = minutes.ToString() + ':' + (seconds).ToString("00");
        }
        oldSeconds = seconds;

        //十秒以下になったら文字を赤くする
        if (minutes == 0 && seconds < 10)
            text.color = Color.red;
    }


    public bool StartFlag
    {
        set
        {
            startFlag = value;
        }
        get
        {
            return startFlag;
        }
    }
}
