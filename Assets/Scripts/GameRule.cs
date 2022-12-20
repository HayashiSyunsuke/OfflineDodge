using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameRule : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> m_listPlayerData;
    [SerializeField]
    private List<GameObject> m_listSpawnPoints;
    [SerializeField]
    private PlayerCounter m_playerCounter;

    [SerializeField]
    private int m_roundNum;                 //���E���h��
    [SerializeField]
    private int m_redTeamCurrentRounds;     //���b�h�`�[���̌��݂̃��E���h������
    [SerializeField]
    private int m_blueTeamCurrentRounds;    //�u���[�`�[���̌��݂̃��E���h������

    private bool m_redTeamWin = false;      //���b�h�`�[���̏���
    private bool m_blueTeamWin = false;     //�u���[�`�[���̏���

    [SerializeField]
    private bool m_reset = false;

    [SerializeField]
    private float m_timer;

    [SerializeField]
    private float START_TIME;

    [SerializeField]
    private bool m_redTeamDown = false;
    [SerializeField]
    private bool m_blueTeamDown = false;
    [SerializeField]
    private bool m_startFlag = false;

    [SerializeField]
    private CountSystem m_countSystem;



    // Start is called before the first frame update
    void Start()
    {
        m_timer = START_TIME;
        m_startFlag = true;
        m_reset = true;
    }

    void FixedUpdate()
    {
        if (m_playerCounter.PlayerNum >= 2)
        {
            JudgmentOfWin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerCounter.PlayerNum >= 2)
        {
            if (m_startFlag)
                StartRound();

       
            if (m_reset)
            {
                ResetPosition();
                //m_startFlag = true;
            }
        }
     
    }

    //�L�����N�^�[�����X�g�ɓo�^����
    public void AddCharacter(GameObject obj)
    {
        m_listPlayerData.Add(obj);
    }

    //���s����
    public void JudgmentOfWin()
    {
        float red = 0;
        float blue = 0;

        foreach (GameObject player in m_listPlayerData)
        {
            if (player.layer == 13)
                red += player.GetComponent<ThirdPersonController>().HP;

            if (player.layer == 14)
                blue += player.GetComponent<ThirdPersonController>().HP;

        }


        //���b�h�`�[���̑�HP���[���ɂȂ�����t���O�𗧂Ă�
        if ((int)red <= 0)
        {
            m_blueTeamWin = true;

            m_redTeamDown = true;

            RoundUpdate();
        }
        //�u���[�`�[���̑�HP���[���ɂȂ�����t���O�𗧂Ă�
        else if ((int)blue <= 0)
        {
            m_redTeamWin = true;

            m_blueTeamDown = true;

            RoundUpdate();
        }

    }

    private void RoundUpdate()
    {
        m_roundNum--;

        if (m_redTeamWin)
        {
            m_redTeamCurrentRounds++;
        }
        if (m_blueTeamWin)
        {
            m_blueTeamCurrentRounds++;
        }

        if (m_roundNum == 0)
        {
            VictoryOrDefeatDirection();
        }

        //�t���O�̏�����
        m_redTeamWin = false;
        m_blueTeamWin = false;

        m_reset = true;

    }

    public void VictoryOrDefeatDirection()
    {

        if (m_redTeamCurrentRounds == m_blueTeamCurrentRounds)
        {
            //���b�h�`�[���Ɉ����������o


            //�u���[�`�[���Ɉ����������o


        }
        else if (m_redTeamCurrentRounds < m_blueTeamCurrentRounds)
        {
            //���b�h�`�[���ɕ������o


            //�u���[�`�[���ɏ������o


        }
        else if (m_redTeamCurrentRounds > m_blueTeamCurrentRounds)
        {
            //���b�h�`�[���ɏ������o


            //�u���[�`�[���ɕ������o


        }
    }

    //�ʒu�����Z�b�g����@�ΏہF �v���C���[ �� �{�[��
    public void ResetPosition()
    {
        bool[] check = { false, false };

        //�`�[���ʂňʒu������������
        foreach (GameObject player in m_listPlayerData)
        {
            int num = 0;

            foreach (GameObject spawnPoint in m_listSpawnPoints)
            {
                if (player.layer == m_listSpawnPoints[num].gameObject.layer && !check[num])
                {
                    player.transform.position = m_listSpawnPoints[num].transform.position;  //�ʒu�̏�����
                    player.transform.rotation = m_listSpawnPoints[num].transform.rotation;  //��]�̏�����
                    check[num] = true;

                    break;
                }
                num++;
            }
        }

        //�{�[���̈ʒu������������
        //GameObject.FindWithTag("Ball1").GetComponent<Ball>().ResetPosition();

        //�t���O�̃��Z�b�g
        m_reset = false;
    }

    public void StartRound()
    {
        m_timer -= Time.deltaTime;

        m_countSystem.CountDown();

        if (!m_reset)
            m_reset = true;
        else
            m_reset = false;

        //�`�[���ʂňʒu������������
        foreach (GameObject player in m_listPlayerData)
        {
            var tpc = player.GetComponent<ThirdPersonController>();

            tpc.IsOperation = false;

            if (m_timer <= 0.0f)
            {
                m_timer = 0.0f;

                tpc.IsOperation = true;
            }
            else if(m_countSystem.End)
            {
                m_startFlag = false;
            }
        }

        Debug.Log("���݂̃^�C�}�["+�@m_timer);

        
    }

    public void ResetTimer()
    {
        m_timer = START_TIME; 
    }

    public bool RedTeamDown
    {
        get { return m_redTeamDown; }
    }
    public bool BlueTeamDown
    {
        get { return m_blueTeamDown; }
    }

    public float Timer
    {
        get { return m_timer; }
    }

    public float StartTime
    {
        get { return START_TIME; }
    }

}
