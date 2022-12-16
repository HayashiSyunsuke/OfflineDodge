using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHPGauge : MonoBehaviour
{
    [SerializeField]
    Image hp;
    [SerializeField]
    private HPGauge hPGauge;

    private float hpParcent=1.0f;

    private float abc=1.0f; 
    void Start()
    {
        if (hp == null) 
        hp = this.GetComponent<Image>();
        if (hPGauge == null) 
        hPGauge = GetComponentInChildren<HPGauge>();
    }

    // Update is called once per frame
    void Update()
    {
        var t = Mathf.PingPong(Time.deltaTime, 1.0f);

        //hpParcent = Mathf.Lerp(hp.fillAmount, hPGauge.Parcent, t);
        //hpParcent = Mathf.Lerp(abc, hPGauge.Parcent, t);
        hpParcent = Mathf.SmoothStep(hp.fillAmount, hPGauge.Parcent, Time.deltaTime*15);
        hp.fillAmount = hpParcent;   
    }

    public float HpParcent
    {
        get
        {
            return hpParcent;
        }
    }
}
