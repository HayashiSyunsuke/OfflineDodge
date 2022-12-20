using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge: MonoBehaviour
{
    [SerializeField]
    Image hp;

    //＊キャラのHPに変更する＊
    //[SerializeField]
    //private CharacterHP characterHP;

    [SerializeField]
    private DamageHPGauge damageHP;

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
        if(damageHP==null)
        {
            damageHP = GetComponentInParent<DamageHPGauge>();
        }
        //maxHP = characterHP.HP;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのHPを持ってくる
        //currentHP = characterHP.HP;
        parcent = currentHP / maxHP;

        SetFillAmount();

        ChangeColor();
    }

    private void FixedUpdate()
    {
        //if(characterHP.HP<= 0)
        //{
        //    GetComponentInParent<ChangeScene>().ChangeFlag = true;
        //    return;
        //}
    }

    //残りHPで色を変える
    private void ChangeColor()
    {
        if (damageHP.HpParcent > 0.5f)
            hp.color = new Color(69.0f/255.0f, 206.0f/255.0f, 233.0f/255.0f, 1);
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
