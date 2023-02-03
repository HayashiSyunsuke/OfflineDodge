using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //テキスト
    [SerializeField]
    Image timerGauge;

    //制限時間
    private float totalTime;

    //最大時間
    private float maxTime;

    //分
    [SerializeField]
    private int minutes;

    //時間
    [SerializeField]
    private float seconds;

    //前の秒数
    private float oldSeconds = 0;

    [SerializeField]
    //開始フラグ
    private bool startFlag = false;

    private bool hideVSGauge;

    // Start is called before the first frame update
    void Start()
    {
        if(timerGauge==null)
        {
            timerGauge = transform.GetChild(0).GetComponent<Image>();
        }
        //秒数に直す
        totalTime = minutes * 60 + seconds;
        maxTime = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        //カウントダウン
        CountDownTimer();

        timerGauge.fillAmount = NormalizeEncode(totalTime);
        ChangeColor();
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


        oldSeconds = seconds;
        /*
        //テキストに( OO：OO )の形で描画する
        if (seconds != oldSeconds)
        {
            text.text = minutes.ToString() + ':' + (seconds).ToString("00");
        }

        //十秒以下になったら文字を赤くする
        if (minutes == 0 && seconds < 10)
            text.color = Color.red;
        */
    }

    private void ChangeColor()
    {
        if (timerGauge.fillAmount > 0.5f)
            timerGauge.color = new Color(69.0f / 255.0f, 206.0f / 255.0f, 233.0f / 255.0f, 1);
        else if (timerGauge.fillAmount > 0.25f)
            timerGauge.color = Color.yellow;
        else
            timerGauge.color = Color.red;
    }

    float NormalizeEncode(float value)
    {
        return (value - 0.0f) / (maxTime - 0.0f);
    }

    void ChangeVSgauge()
    {
        if (timerGauge.fillAmount > 0.25f)
        {
            hideVSGauge = true;
        }
    }


    //セッター、ゲッター
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

    public int Minutes
    {
        get
        {
            return minutes;
        }
    }

    public float TotalTime
    {
        get 
        { 
            return totalTime; 
        }
    }
}
