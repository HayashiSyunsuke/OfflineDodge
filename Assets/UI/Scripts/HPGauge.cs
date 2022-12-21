using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    [SerializeField]
    Image hp;

    [SerializeField]
    private ThirdPersonController player;

    [SerializeField]
    private DamageHPGauge damageHP;

    private PlayerCounter playerCounter;

    GameObject child;

    private bool ones = true;

    //プレイヤーの現HP
    float currentHP;

    //プレイヤーの最大HP
    float maxHP;

    //HPのパーセント
    float parcent;

    // Start is called before the first frame update
    void Start()
    {
        /*
        characterHP = GetComponentInParent<CharacterHP>();
        characterHP = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CharacterHP>();
        damageHP= GetComponentInParent<DamageHPGauge>();
        */
        if (hp == null)
        {
            hp = this.GetComponent<Image>();
        }
        //if (characterHP == null)
        //{
        //    var players = GameObject.FindGameObjectsWithTag("user");
        //    characterHP = players[1].GetComponent<CharacterHP>();
        //}
        if (damageHP == null)
        {
            damageHP = GetComponentInParent<DamageHPGauge>();
        }
        //maxHP = characterHP.HP;
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

            maxHP = player.HP;
            ones = false;
        }

        //プレイヤーのHPを持ってくる
        currentHP = player.HP;
        parcent = currentHP / maxHP;

        SetFillAmount();

        ChangeColor();
    }

    private void FixedUpdate()
    {
        if (playerCounter.PlayerNum < 2)
            return;

        if (player.HP <= 0)
        {
            //GetComponentInParent<ChangeScene>().ChangeFlag = true;
            return;
        }
    }

    //残りHPで色を変える
    private void ChangeColor()
    {
        if (damageHP.HpParcent > 0.5f)
            hp.color = new Color(69.0f / 255.0f, 206.0f / 255.0f, 233.0f / 255.0f, 1);
        else if (damageHP.HpParcent > 0.2f)
            hp.color = Color.yellow;
        else
            hp.color = Color.red;
    }

    //ゲージの減り
    private void SetFillAmount()
    {
        hp.fillAmount = parcent;
    }

    public float Parcent
    {
        get
        {
            return parcent;
        }
    }
}
