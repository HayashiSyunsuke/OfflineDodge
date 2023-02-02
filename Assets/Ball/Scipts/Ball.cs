using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameObject tpc;

    private Rigidbody m_rigidbody;          //物理挙動
    [SerializeField]
    private Vector3 m_localGravity;         //ボールに加わる重力
    [SerializeField]
    private bool m_useGravity = false;      //重力の有無
    [SerializeField]
    private float m_ballSpeed;              //ボールの初期スピード
    [SerializeField]
    private float m_RotationalForce;        //ボールの回転する力
    [SerializeField]
    private float m_passAngle;              //パスをする際のボール飛ぶ角度
    [SerializeField]
    private Transform m_ballSpawnPoint;         //ボールの初期位置
    [SerializeField]
    private float m_ballPower = 0;          //ボールの加速量
    [SerializeField]
    private float m_accelerationValue;      //加速値
    [SerializeField]
    private float m_decelerationValue;      //減速値
    [SerializeField]
    private float m_accelerationValueMax;   //加速値最大値
    [SerializeField]
    private int m_damage;
    [SerializeField]
    private GameObject m_throwObject;
    [SerializeField]
    private GameRule m_gameRule;        //ゲームルール

    [SerializeField]
    private VSScoreGauge scoreUI;       //スコアのUI

    //エフェクト関連
    private ParticleSystem m_particle;
    private TrailRenderer m_trail;          //ボールのエフェクト

    private EffectManager m_effectManager;  //エフェクト

    //レイヤー
    private LayerMask m_teamLayer;

    //当たり判定の有無
    [SerializeField] private bool m_hitValidity = true;
    private bool m_hit = false;

    //sound
    public AudioSource audioSource;
    public AudioClip BoundAudioClip;
    [Range(0, 1)] public float BoundSoundVolume;

    void Start()
    {
        tpc = GameObject.FindGameObjectWithTag("Player");

        m_rigidbody = GetComponent<Rigidbody>();
        m_trail = GetComponent<TrailRenderer>();
        m_effectManager = GetComponent<EffectManager>();
    }


    void Update()
    {
        //ボールに重力を加える
        if (m_useGravity)
        {
            m_rigidbody.AddForce(m_localGravity, ForceMode.Acceleration);
        }
        //ボールの加速値を減らしていく
        if (m_ballPower > 0.0f)
        {
            m_ballPower -= m_decelerationValue * Time.deltaTime;
        }
        else if (m_ballPower < 0.0f)
        {
            m_ballPower = 0.0f;
        }

        if (this.transform.parent != null)
        {
            this.GetComponent<SphereCollider>().isTrigger = true;
        }
        else
        {
            this.GetComponent<SphereCollider>().isTrigger = false;
        }
    }

    /// <summary>
    /// 投げられた
    /// </summary>
    public void Straight(Vector3 vec)
    {
        //力 ＝ 速さ × 重さ
        Vector3 force = (vec * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        m_rigidbody.AddForce(force, ForceMode.Impulse);
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //ボールの加速値を入れる
        if (m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //ボールの加速値が最大値を上回ったら最大値に戻す
        if (m_ballPower > m_accelerationValueMax)
        {
            m_ballPower = m_accelerationValueMax;
        }
    }
    public void TargetStraight(Vector3 vec)
    {
        Vector3 direction = new Vector3((vec.x - gameObject.transform.position.x),0.0f,(vec.z - gameObject.transform.position.z)).normalized;

        //Vector3 velocity = new Vector3(direction.x, 0.0f, direction.y);

        //力 ＝ 速さ × 重さ
        Vector3 force = (direction * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        m_rigidbody.AddForce(force, ForceMode.Impulse);
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //ボールの加速値を入れる
        if (m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //ボールの加速値が最大値を上回ったら最大値に戻す
        if (m_ballPower > m_accelerationValueMax)
        {
            m_ballPower = m_accelerationValueMax;
        }
    }

    public void TargetDownward(Vector3 vec)
    {
        Vector3 direction = new Vector3((vec.x - gameObject.transform.position.x), (vec.y - gameObject.transform.position.y) + 0.5f, (vec.z - gameObject.transform.position.z)).normalized;

        //Vector3 velocity = new Vector3(direction.x, 0.0f, direction.y);

        //力 ＝ 速さ × 重さ
        Vector3 force = (direction * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        m_rigidbody.AddForce(force, ForceMode.Impulse);
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //ボールの加速値を入れる
        if (m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //ボールの加速値が最大値を上回ったら最大値に戻す
        if (m_ballPower > m_accelerationValueMax)
        {
            m_ballPower = m_accelerationValueMax;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_throwObject == collision.gameObject)
            return;

        m_useGravity = true;
        ResetThrowObject();

        //audioSource.PlayOneShot(BoundAudioClip, BoundSoundVolume);

        var tpc = collision.gameObject.GetComponent<ThirdPersonController>();

        if (collision.gameObject.tag == "Floor")
        {
            m_hitValidity = false;
            ResetLayer();
        }

        if (
            (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")   //タグが"Player"もしくは"Enemy"なら
            //&& !tpc.Dieing                                                                  //死亡してなければ 
            && m_hitValidity                                                                //ボールがバウンドしてなければ
            && tpc.TeamLayer != m_teamLayer                                    //投げた人と違うチームなら
            )
        {

            tpc.HP -= m_damage;                                 //ダメージを与える
            m_gameRule.TeamTotalDamage(m_damage,m_teamLayer);   //ダメージ値をゲームルールで加算する
            scoreUI.Damage(m_damage, m_teamLayer);

            if (tpc.HP < 0)
            {
                tpc.HP = 0;
                //tpc.Dieing = true;

                //enemy.GetComponent<ThirdPersonController>().Die();
                //characterHp.DeadFlag = true;
            }

            ////ヒット時のエフェクトを表示する
            //if (tpc.Dieing)
            //{
            //    m_effectManager.DeadParticle(this.gameObject.transform.position);
            //}
            //else
            //{
            //    m_effectManager.HitParticle(this.gameObject.transform.position);
            //}
        }
    }

    public void ResetPosition()
    {
        m_rigidbody.velocity = Vector3.zero;                        //速度の初期化
        this.transform.position = m_ballSpawnPoint.position;        //位置の初期化
        this.transform.rotation = Quaternion.identity;              //回転軸の初期化
    }

    public void CheckLayer(LayerMask layer)
    {
        m_teamLayer = layer;
    }

    public void ResetLayer()
    {
        m_teamLayer = 0;
    }

    public void CheckThrowObject(GameObject obj)
    {
        m_throwObject = obj;
    }

    public void ResetThrowObject()
    {
        m_throwObject = null;
    }

    public void CollisionNullification()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    public void CollisionValidation()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    public Vector3 Gravity
    {
        get { return m_localGravity; }
        set { m_localGravity = value; }
    }

    public bool UseGravity
    {
        get { return m_useGravity; }
        set { m_useGravity = value; }
    }

    public bool HitValidity
    {
        get { return m_hitValidity; }
        set { m_hitValidity = value; }
    }

    public bool Hit
    {
        get { return m_hit; }
        set { m_hit = value; }
    }
}
