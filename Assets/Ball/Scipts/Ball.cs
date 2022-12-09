using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPunCallbacks
{
    private GameObject tpc;

    private Rigidbody m_rigidbody;          //��������
    [SerializeField]
    private Vector3 m_localGravity;         //�{�[���ɉ����d��
    [SerializeField]
    private bool m_useGravity = false;      //�d�̗͂L��
    [SerializeField]
    private float m_ballSpeed;              //�{�[���̏����X�s�[�h
    [SerializeField]
    private float m_RotationalForce;        //�{�[���̉�]�����
    [SerializeField]
    private float m_passAngle;              //�p�X������ۂ̃{�[����Ԋp�x
    [SerializeField]
    private float m_ballPower = 0;          //�{�[���̉�����
    [SerializeField]
    private float m_accelerationValue;      //�����l
    [SerializeField]
    private float m_decelerationValue;      //�����l
    [SerializeField]
    private float m_accelerationValueMax;   //�����l�ő�l


    //�G�t�F�N�g�֘A
    private ParticleSystem m_particle;
    private TrailRenderer m_trail;          //�{�[���̃G�t�F�N�g


    void Start()
    {
        tpc = GameObject.FindGameObjectWithTag("Player");

        m_rigidbody = GetComponent<Rigidbody>();
        m_trail = GetComponent<TrailRenderer>();
    }

    
    void Update()
    {
        //�{�[���ɏd�͂�������
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
    /// ������ꂽ
    /// </summary>
    public void Straight(Vector3 vec)
    {   
        //�� �� ���� �~ �d��
        Vector3 force = (vec * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        //�{�[���ɗ͂�������
        m_rigidbody.AddForce(force, ForceMode.Impulse);
        //�{�[������]������
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //�{�[���̉����l������
        if(m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //�{�[���̉����l���ő�l����������ő�l�ɖ߂�
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
