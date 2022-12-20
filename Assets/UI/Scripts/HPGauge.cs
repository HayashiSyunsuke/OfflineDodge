using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge: MonoBehaviour
{
    [SerializeField]
    Image hp;

    //���L������HP�ɕύX���遖
    //[SerializeField]
    //private CharacterHP characterHP;

    [SerializeField]
    private DamageHPGauge damageHP;

    //�v���C���[�̌�HP
    float currentHP;

    //�v���C���[�̍ő�HP
    float maxHP;

    //HP�̃p�[�Z���g
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
        //�v���C���[��HP�������Ă���
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

    //�c��HP�ŐF��ς���
    private void ChangeColor()
    {
        if (damageHP.HpParcent > 0.5f)
            hp.color = new Color(69.0f/255.0f, 206.0f/255.0f, 233.0f/255.0f, 1);
        else if (damageHP.HpParcent > 0.2f)
            hp.color = Color.yellow;
        else
            hp.color = Color.red;
    }

    //�Q�[�W�̌���
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
