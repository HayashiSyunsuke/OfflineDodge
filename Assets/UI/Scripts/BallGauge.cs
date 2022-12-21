using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallGauge : MonoBehaviour
{

    [SerializeField]
    private ThirdPersonController player = null;

    [SerializeField]
    private Image image;

    private PlayerCounter playerCounter;

    GameObject child;

    private bool ones = true;
    private bool timerOnes = true;

    private float totalTime;
    private float oldSeconds;

    float maxCount = 10.0f;
    float timer = 10.0f;

    //ゲージのパーセント
    float parcent;

    private void Start()
    {
        // 変更箇所　userー＞合うやつ
        //var players = GameObject.FindGameObjectsWithTag("user");
        //player = players[1].GetComponent<ThirdPersonController>();

        if (image == null)
        {
            image = this.GetComponent<Image>();
        }
        if (playerCounter == null)
        {
            playerCounter = GameObject.Find("PlayerManager").GetComponent<PlayerCounter>();
        }

        child = transform.GetChild(0).gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        if (playerCounter.PlayerNum < 2)
            return;

        if (ones)
        {
            if (child.name == "Left")
                player = GameObject.Find("FemaleDummy1").GetComponent<ThirdPersonController>();
            if (child.name == "Right")
                player = GameObject.Find("FemaleDummy2").GetComponent<ThirdPersonController>();

            ones = false;
        }

        totalTime = Time.time;


        if (player.IsBallHaving)
        {
            //一回だけ
            if (timerOnes)
            {
                oldSeconds = Time.time;
                timerOnes = false;
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
            timerOnes = true;
            parcent = Mathf.Lerp(image.fillAmount, (maxCount / 4) * 3, Time.deltaTime);
            //parcent = Mathf.SmoothStep(image.fillAmount, (maxCount / 4) * 3, Time.deltaTime * 15);
            SetFillAmount();
            timer = 10.0f;
        }

    }


    //ゲージの減り
    private void SetFillAmount()
    {
        image.fillAmount = parcent;
    }
}
