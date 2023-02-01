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
    GameObject characterImage;

    [SerializeField]
    float max, min;

    [SerializeField]
    private Timer timer;

    private float posy;
    private float alpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreBar == null) 
        {
            scoreBar = this.GetComponent<Image>();
        }
        if (barGauge == null) 
        {
            barGauge = transform.GetChild(0).gameObject;
        }
        if (characterImage == null) 
        {
            characterImage = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        }
        if (timer == null) 
        {
            timer = GameObject.Find("Timer").GetComponent<Timer>();
        }

        posy = NormalizeDecode(0.5f);
        if (this.name == "RedBar")
            scoreBar.color = new Color(0.8773585f, 0.2141821f, 0.1448469f, alpha);

    }

    // Update is called once per frame
    void Update()
    {

        scoreBar.fillAmount = Mathf.SmoothStep(scoreBar.fillAmount, NormalizeEncode(posy), Time.deltaTime * 15);
        barGauge.GetComponent<RectTransform>().position = new Vector3(barGauge.transform.position.x, NormalizeDecode(scoreBar.fillAmount), barGauge.transform.position.z);
        if (timer.Minutes <= 0) 
        {
            alpha -= Time.deltaTime / 2.0f;
            if (this.name == "RedBar")
                scoreBar.color = new Color(0.8773585f, 0.2141821f, 0.1448469f, alpha);
            if (this.name == "BlueBar")
                scoreBar.color = new Color(0.1863208f, 0.3843483f, 0.745283f, alpha);
            barGauge.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
            characterImage.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
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




    //セッター、ゲッター
    public float Posy
    {
        set { posy = value; }
        get { return posy; }
    }

}
