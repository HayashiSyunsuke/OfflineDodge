using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
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
    private Transform m_ballSpawnPoint;         //�{�[���̏����ʒu
    [SerializeField]
    private float m_ballPower = 0;          //�{�[���̉�����
    [SerializeField]
    private float m_accelerationValue;      //�����l
    [SerializeField]
    private float m_decelerationValue;      //�����l
    [SerializeField]
    private float m_accelerationValueMax;   //�����l�ő�l
    [SerializeField]
    private int m_damage;
    [SerializeField]
    private GameObject m_throwObject;
    [SerializeField]
    private GameRule m_gameRule;        //�Q�[�����[��

    [SerializeField]
    private VSScoreGauge scoreUI;       //�X�R�A��UI

    //�G�t�F�N�g�֘A
    private ParticleSystem m_particle;
    private TrailRenderer m_trail;          //�{�[���̃G�t�F�N�g

    private EffectManager m_effectManager;  //�G�t�F�N�g

    //���C���[
    private LayerMask m_teamLayer;

    //�����蔻��̗L��
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
        //�{�[���ɏd�͂�������
        if (m_useGravity)
        {
            m_rigidbody.AddForce(m_localGravity, ForceMode.Acceleration);
        }
        //�{�[���̉����l�����炵�Ă���
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
    /// ������ꂽ
    /// </summary>
    public void Straight(Vector3 vec)
    {
        //�� �� ���� �~ �d��
        Vector3 force = (vec * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        m_rigidbody.AddForce(force, ForceMode.Impulse);
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //�{�[���̉����l������
        if (m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //�{�[���̉����l���ő�l����������ő�l�ɖ߂�
        if (m_ballPower > m_accelerationValueMax)
        {
            m_ballPower = m_accelerationValueMax;
        }
    }
    public void TargetStraight(Vector3 vec)
    {
        Vector3 direction = new Vector3((vec.x - gameObject.transform.position.x),0.0f,(vec.z - gameObject.transform.position.z)).normalized;

        //Vector3 velocity = new Vector3(direction.x, 0.0f, direction.y);

        //�� �� ���� �~ �d��
        Vector3 force = (direction * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        m_rigidbody.AddForce(force, ForceMode.Impulse);
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //�{�[���̉����l������
        if (m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //�{�[���̉����l���ő�l����������ő�l�ɖ߂�
        if (m_ballPower > m_accelerationValueMax)
        {
            m_ballPower = m_accelerationValueMax;
        }
    }

    public void TargetDownward(Vector3 vec)
    {
        Vector3 direction = new Vector3((vec.x - gameObject.transform.position.x), (vec.y - gameObject.transform.position.y) + 0.5f, (vec.z - gameObject.transform.position.z)).normalized;

        //Vector3 velocity = new Vector3(direction.x, 0.0f, direction.y);

        //�� �� ���� �~ �d��
        Vector3 force = (direction * (m_ballSpeed + m_ballPower)) * m_rigidbody.mass;

        m_rigidbody.AddForce(force, ForceMode.Impulse);
        m_rigidbody.AddTorque(transform.right * (force.magnitude * m_RotationalForce), ForceMode.Impulse);

        //�{�[���̉����l������
        if (m_ballPower < m_accelerationValueMax)
        {
            m_ballPower += m_accelerationValue;
        }

        //�{�[���̉����l���ő�l����������ő�l�ɖ߂�
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
            (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")   //�^�O��"Player"��������"Enemy"�Ȃ�
            //&& !tpc.Dieing                                                                  //���S���ĂȂ���� 
            && m_hitValidity                                                                //�{�[�����o�E���h���ĂȂ����
            && tpc.TeamLayer != m_teamLayer                                    //�������l�ƈႤ�`�[���Ȃ�
            )
        {

            tpc.HP -= m_damage;                                 //�_���[�W��^����
            m_gameRule.TeamTotalDamage(m_damage,m_teamLayer);   //�_���[�W�l���Q�[�����[���ŉ��Z����
            scoreUI.Damage(m_damage, m_teamLayer);

            if (tpc.HP < 0)
            {
                tpc.HP = 0;
                //tpc.Dieing = true;

                //enemy.GetComponent<ThirdPersonController>().Die();
                //characterHp.DeadFlag = true;
            }

            ////�q�b�g���̃G�t�F�N�g��\������
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
        m_rigidbody.velocity = Vector3.zero;                        //���x�̏�����
        this.transform.position = m_ballSpawnPoint.position;        //�ʒu�̏�����
        this.transform.rotation = Quaternion.identity;              //��]���̏�����
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
