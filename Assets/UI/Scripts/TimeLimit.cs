using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    //�e�L�X�g
    [SerializeField]
    Text text;

    //��������
    private float totalTime;

    //��
    [SerializeField]
    private int minutes;

    //����
    [SerializeField]
    private float seconds;

    //�O�̕b��
    private float oldSeconds = 0;
    //�J�n�t���O
    private bool startFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        //�b���ɒ���
        totalTime = minutes * 60 + seconds;
    }

    // Update is called once per frame
    void Update()
    {
        //�J�E���g�_�E��
        CountDownTimer();
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

        //�e�L�X�g��( OO�FOO )�̌`�ŕ`�悷��
        if (seconds != oldSeconds)
        {
            text.text = minutes.ToString() + ':' + (seconds).ToString("00");
        }
        oldSeconds = seconds;

        //�\�b�ȉ��ɂȂ����當����Ԃ�����
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
