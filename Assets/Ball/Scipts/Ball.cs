using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPunCallbacks
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
    private float m_ballPower = 0;          //ボールの加速量
    [SerializeField]
    private float m_accelerationValue;      //加速値
    [SerializeField]
    private float m_decelerationValue;      //減速値
    [SerializeField]
    private float m_accelerationValueMax;   //加速値最大値


    //エフェクト関連
    private ParticleSystem m_particle;
    private TrailRenderer m_trail;          //ボールのエフェクト


    void Start()
    {
        tpc = GameObject.FindGameObjectWithTag("Player");

        m_rigidbody = GetComponent<Rigidbody>();
        m_trail = GetComponent<TrailRenderer>();
    }

    
    void Update()
    {
        //ボールに重力を加える
        if (m_useGravity)
        {
            m_rigidbody.AddForce(m_localGravity, ForceMode.Acceleration);
        }

        if (m_ballPower > 0.0f)
        {
            m_ballPower -= m_decelerationValue * Time.deltaTime;
        }
        else if(m_ballPower < 0.0f)
        {
            m_ballPower = 0.0f;
        }
    }

    /// <summary>
    /// 投げられた
    /// </summary>
    public void Straight(Vector3 vec)
    {   
        //力 ＝ 速さ × 重さ
        Vector3 force = (vec * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        //ボールに力を加える
        m_rigidbody.AddForce(force, ForceMode.Impulse);
        //ボールを回転させる
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //ボールの加速値を入れる
        if(m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //ボールの加速値が最大値を上回ったら最大値に戻す
        if(m_ballPower > m_accelerationValueMax)
        {
            m_ballPower = m_accelerationValueMax;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "RightHand")
        {
            m_useGravity = true;
            //rb.velocity *= 1.05f;

        }
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
}
