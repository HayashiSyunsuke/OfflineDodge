using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSScoreGauge : MonoBehaviour
{

    [SerializeField]
    GameObject redBar, blueBar;

    float scorePoint = 10.0f;

    [SerializeField]
    bool hitFlag;


    // Start is called before the first frame update
    void Start()
    {
        if(redBar==null)
        {
            redBar = GameObject.Find("RedBar");
        }
        if(blueBar==null)
        {
            blueBar = GameObject.Find("BlueBar");
        }

        redBar.GetComponent<Image>().fillAmount = 0.5f;
        blueBar.GetComponent<Image>().fillAmount = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitFlag == true)
        {
            //���������`�[���ɂ���ď�����ς���
            //if('�t���O' == Red){}
            //if('�t���O' == Blue){}

            //���ŐԂ��オ��悤�ɂ��Ă���
            redBar.GetComponent<BarGauge>().Posy += scorePoint;
            blueBar.GetComponent<BarGauge>().Posy += scorePoint;

            hitFlag = false;
        }
    }

    public void Damage(float value, LayerMask layer)
    {
        scorePoint = value;

        if (layer == 19)         //Red�`�[��
        {
            redBar.GetComponent<BarGauge>().Posy += scorePoint;
            blueBar.GetComponent<BarGauge>().Posy += scorePoint;
        }
        else if (layer == 20)    //Blue�`�[��
        {
            redBar.GetComponent<BarGauge>().Posy -= scorePoint;
            blueBar.GetComponent<BarGauge>().Posy -= scorePoint;
        }
    }
}
