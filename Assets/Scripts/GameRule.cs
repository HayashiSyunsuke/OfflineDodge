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
    private int m_roundNum;                 //ラウンド数
    [SerializeField]
    private int m_redTeamCurrentRounds;     //レッドチームの現在のラウンド勝利数
    [SerializeField]
    private int m_blueTeamCurrentRounds;    //ブルーチームの現在のラウンド勝利数

    private bool m_redTeamWin = false;      //レッドチームの勝利
    private bool m_blueTeamWin = false;     //ブルーチームの勝利

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

    //キャラクターをリストに登録する
    public void AddCharacter(GameObject obj)
    {
        m_listPlayerData.Add(obj);
    }

    //勝敗判定
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


        //レッドチームの総HPがゼロになったらフラグを立てる
        if ((int)red <= 0)
        {
            m_blueTeamWin = true;

            m_redTeamDown = true;

            RoundUpdate();
        }
        //ブルーチームの総HPがゼロになったらフラグを立てる
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

        //フラグの初期化
        m_redTeamWin = false;
        m_blueTeamWin = false;

        m_reset = true;

    }

    public void VictoryOrDefeatDirection()
    {

        if (m_redTeamCurrentRounds == m_blueTeamCurrentRounds)
        {
            //レッドチームに引き分け演出


            //ブルーチームに引き分け演出


        }
        else if (m_redTeamCurrentRounds < m_blueTeamCurrentRounds)
        {
            //レッドチームに負け演出


            //ブルーチームに勝ち演出


        }
        else if (m_redTeamCurrentRounds > m_blueTeamCurrentRounds)
        {
            //レッドチームに勝ち演出


            //ブルーチームに負け演出


        }
    }

    //位置をリセットする　対象： プレイヤー ＆ ボール
    public void ResetPosition()
    {
        bool[] check = { false, false };

        //チーム別で位置を初期化する
        foreach (GameObject player in m_listPlayerData)
        {
            int num = 0;

            foreach (GameObject spawnPoint in m_listSpawnPoints)
            {
                if (player.layer == m_listSpawnPoints[num].gameObject.layer && !check[num])
                {
                    player.transform.position = m_listSpawnPoints[num].transform.position;  //位置の初期化
                    player.transform.rotation = m_listSpawnPoints[num].transform.rotation;  //回転の初期化
                    check[num] = true;

                    break;
                }
                num++;
            }
        }

        //ボールの位置を初期化する
        //GameObject.FindWithTag("Ball1").GetComponent<Ball>().ResetPosition();

        //フラグのリセット
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

        //チーム別で位置を初期化する
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

        Debug.Log("現在のタイマー"+　m_timer);

        
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
