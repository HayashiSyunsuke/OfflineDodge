using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //�e�L�X�g
    [SerializeField]
    Image timerGauge;

    //��������
    private float totalTime;

    //�ő厞��
    private float maxTime;

    //��
    [SerializeField]
    private int minutes;

    //����
    [SerializeField]
    private float seconds;

    //�O�̕b��
    private float oldSeconds = 0;

    [SerializeField]
    //�J�n�t���O
    private bool startFlag = false;

    private bool hideVSGauge;

    // Start is called before the first frame update
    void Start()
    {
        if(timerGauge==null)
        {
            timerGauge = transform.GetChild(0).GetComponent<Image>();
        }
        //�b���ɒ���
        totalTime = minutes * 60 + seconds;
        maxTime = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        //�J�E���g�_�E��
        CountDownTimer();

        timerGauge.fillAmount = NormalizeEncode(totalTime);
        ChangeColor();
    }

    //�J�E���g�_�E������
    private void CountDownTimer()
    {
        //�J�n���Ă��Ȃ���Ώ������Ȃ�
        if (startFlag == false)
            return;

        //���Ԃ��߂������V�[����ς���
        if (totalTime < 0)
        {
            //GetComponentInParent<ChangeScene>().ChangeFlag = true;
            return;
        }

        totalTime = minutes * 60 + seconds;
        totalTime -= Time.deltaTime;

        //�e���A�b�̌v�Z
        minutes = (int)totalTime / 60;
        seconds = totalTime - minutes * 60;


        oldSeconds = seconds;
        /*
        //�e�L�X�g��( OO�FOO )�̌`�ŕ`�悷��
        if (seconds != oldSeconds)
        {
            text.text = minutes.ToString() + ':' + (seconds).ToString("00");
        }

        //�\�b�ȉ��ɂȂ����當����Ԃ�����
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


    //�Z�b�^�[�A�Q�b�^�[
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
