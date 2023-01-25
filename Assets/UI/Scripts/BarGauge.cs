using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarGauge : MonoBehaviour
{
    [SerializeField]
    Image scoreBar;

    [SerializeField]
    GameObject barGauge;

    [SerializeField]
    float max, min;

    float posy;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreBar == null) 
        {
            scoreBar = this.GetComponent<Image>();
        }
        if(barGauge==null)
        {
            barGauge = transform.GetChild(0).gameObject;
        }

        posy = NormalizeDecode(0.5f);

    }

    // Update is called once per frame
    void Update()
    {

        //scoreBar.fillAmount = NormalizeEncode(posy);
        scoreBar.fillAmount = Mathf.SmoothStep(scoreBar.fillAmount, NormalizeEncode(posy), Time.deltaTime * 15);
        //barGauge.transform.position = new Vector3(barGauge.transform.position.x, NormalizeDecode(scoreBar.fillAmount), barGauge.transform.position.z);
        barGauge.GetComponent<RectTransform>().position = new Vector3(barGauge.transform.position.x, NormalizeDecode(scoreBar.fillAmount), barGauge.transform.position.z);
    }



    //関数
    float NormalizeEncode(float value)
    {
        return (value - min) / (max - min);
    }

    float NormalizeDecode(float parcent)
    {
        return parcent * (max - min) + min;
    }

    float NormalizeDecodeBaronly(float parcent)
    {
        float nMax = max - 440.0f;
        float nMin = min - 440.0f;

        return parcent * (nMax - nMin) + nMin;
    }


    //セッター、ゲッター
    public float Posy
    {
        set { posy = value; }
        get { return posy; }
    }

}
