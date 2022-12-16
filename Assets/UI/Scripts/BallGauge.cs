using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallGauge : MonoBehaviour
{

    [SerializeField]
    private ThirdPersonController player;

    [SerializeField]
    private Image image;

    private bool ones = true;

    private float totalTime;
    private float oldSeconds;

    float maxCount = 10.0f;
    float timer = 10.0f;

    //�Q�[�W�̃p�[�Z���g
    float parcent;

    private void Start()
    {
        // �ύX�ӏ��@user�[���������
        var players = GameObject.FindGameObjectsWithTag("user");
        player = players[1].GetComponent<ThirdPersonController>();

        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        totalTime = Time.time;

        if (player.IsBallHaving)
        {
            //��񂾂�
            if (ones)
            {
                oldSeconds = Time.time;
                ones = false;
            }

            if (timer > 0)
            {
                timer = maxCount - (totalTime - oldSeconds);
                //timer = (timer / 4) * 3;
                parcent = ((timer / maxCount) / 4) * 3;
                SetFillAmount();
            }
        }
        else
        {
            ones = true;
            parcent = Mathf.Lerp(image.fillAmount, (maxCount / 4) * 3, Time.deltaTime);
            //parcent = Mathf.SmoothStep(image.fillAmount, (maxCount / 4) * 3, Time.deltaTime * 15);
            SetFillAmount();
            timer = 10.0f;
        }
    }


    //�Q�[�W�̌���
    private void SetFillAmount()
    {
        image.fillAmount = parcent;
    }
}
