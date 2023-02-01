using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergyGauge : MonoBehaviour
{
    [SerializeField]
    private Image energyGauge;

    [SerializeField]
    private ThirdPersonController player;

    private PlayerCounter playerCounter;

    float maxEnergy;

    bool ones = true;

    //プレイヤーの現エネルギー
    float currentEnergy;

    //エネルギーの残量（正規化）
    float parcent;

    // Start is called before the first frame update
    void Start()
    {
        if (energyGauge == null) 
        {
            energyGauge = transform.GetChild(0).GetComponent<Image>();
        }

        if (playerCounter == null)
        {
            playerCounter = GameObject.Find("PlayerManager").GetComponent<PlayerCounter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCounter.PlayerNum < 4)
            return;

        if(ones)
        {
            if (transform.name == "UpperLeft")
                player = GameObject.Find("FemaleDummy1").GetComponent<ThirdPersonController>();
            if (transform.name == "UpperRight")
                player = GameObject.Find("FemaleDummy2").GetComponent<ThirdPersonController>();
            if (transform.name == "LowerLeft")
                player = GameObject.Find("FemaleDummy3").GetComponent<ThirdPersonController>();
            if (transform.name == "LowerRight")
                player = GameObject.Find("FemaleDummy4").GetComponent<ThirdPersonController>();

            maxEnergy = player.HP;
            ones = false;
        }

        //プレイヤーのHPを持ってくる
        currentEnergy = player.HP;
        parcent = currentEnergy / maxEnergy;

        SetFillAmount();
    }


    private void SetFillAmount()
    {
        energyGauge.fillAmount = parcent;
    }
}
