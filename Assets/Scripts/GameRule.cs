using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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
    private GameObject m_stanbyScene;
    [SerializeField]
    private GameObject m_stanbyCamera;
    [SerializeField]
    private GameObject m_playerUICanvas;
    [SerializeField]
    private float m_sceneTimer;
    [SerializeField]
    private float ChangeSceneTime = 2;
    [SerializeField]
    private Ball m_ball;

    [SerializeField]
    private int m_roundNum;                 //ラウンド数
    [SerializeField]
    private int m_redTeamCurrentRounds;     //レッドチームの現在のラウンド勝利数
    [SerializeField]
    private int m_blueTeamCurrentRounds;    //ブルーチームの現在のラウンド勝利数

    private bool m_redTeamWin = false;      //レッドチームの勝利
    private bool m_blueTeamWin = false;     //ブルーチームの勝利

    [SerializeField]
    private bool m_resetFlag = false;

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
    private bool m_resultFlag = false;

    [SerializeField]
    private CountSystem m_countSystem;

    [SerializeField]
    private float m_redTotalDamage = 0;
    [SerializeField]
    private float m_blueTotalDamage = 0;
    [SerializeField]
    private Timer m_timeLimit;


    [SerializeField]
    private GameObject m_rawImage;
    [SerializeField]
    private float VIDEO_TIMER;

    private float m_videoTimer;
    private bool m_flagOnce;


    //赤チーム
    List<GameObject> redTeam = new List<GameObject>();
    //青チーム
    List<GameObject> blueTeam = new List<GameObject>();
    //全体
    List<ThirdPersonController> tpc = new List<ThirdPersonController>();
    //一回だけ
    bool flagOnce;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = START_TIME;
        m_sceneTimer = ChangeSceneTime;
        m_startFlag = false;
        m_resetFlag = true;
        m_playerUICanvas.SetActive(false);
        m_stanbyScene.SetActive(true);
        m_stanbyCamera.SetActive(true);
        m_ball.UseGravity = false;
        m_rawImage.SetActive(false);
        m_videoTimer = VIDEO_TIMER;
        m_flagOnce = false;


    }

    void FixedUpdate()
    {
        //プレイヤーが２人より少なければ return する
        if (m_playerCounter.PlayerNum < 2)
            return;

        //勝敗を判定する
        //JudgmentOfWin();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが４人より少なければ return する
        if (m_playerCounter.PlayerNum < 4)
            return;

        if (m_sceneTimer >= 0.0f)
            m_sceneTimer -= Time.deltaTime;

        if (m_sceneTimer > 0.0f)
            return;

        //アクティブ状態なら非アクティブにする
        if (!m_playerUICanvas.activeSelf)
            m_playerUICanvas.SetActive(true);

        //アクティブ状態なら非アクティブにする
        if (m_stanbyScene.activeSelf)
            m_stanbyScene.SetActive(false);

        //アクティブ状態なら非アクティブにする
        if (m_stanbyCamera.activeSelf)
            m_stanbyCamera.SetActive(false);

        if (!m_rawImage.activeSelf && !m_flagOnce)
            m_rawImage.SetActive(true);

        m_videoTimer -= Time.deltaTime;

        if (m_videoTimer <= 0.0f && !m_flagOnce)
        {
            m_flagOnce = true;
            m_startFlag = true;

            m_rawImage.SetActive(false);
        }
            

        //スタートフラグがTrueの時にラウンドをスタートする
        if (m_startFlag)
            StartRound();

        if (m_timeLimit.TotalTime <= 0.0f && !m_resultFlag)
        {
            JudgmentOfWin();
            m_resultFlag = true;
            m_countSystem.TimeUp();
        }
            


        if (!flagOnce)
        {
            SettingCharacterInfo();
            flagOnce = true;
        }

        //リセットフラグがTrueの時にポジションをリセットする
        if (m_resetFlag)
        {
            //ボールに重力を加える
            if (!m_ball.UseGravity)
                m_ball.UseGravity = true;

            //ボールの位置をリセットする
            m_ball.ResetPosition();
            //プレイヤーの位置をリセットする
            ResetPosition();
        }
            

    }

    //キャラクターをリストに登録する
    public void AddCharacter(GameObject obj)
    {
        m_listPlayerData.Add(obj);
    }

    public void TeamTotalDamage(float damage , LayerMask layer)
    {
        if(layer == 19)         //Redチーム
        {
            m_redTotalDamage += damage;
        }
        else if(layer == 20)    //Blueチーム
        {
            m_blueTotalDamage += damage;
        }


    }

    //勝敗判定
    public void JudgmentOfWin()
    {



        //与えたダメージの合計が高いほうの勝利

        //Blueチームの勝利
        if (m_redTotalDamage < m_blueTotalDamage)
        {
            //Blueチームの勝利演出

            Debug.Log("青チームの勝ち");

        }
        //Redチームの勝利
        else if(m_redTotalDamage > m_blueTotalDamage)
        {
            //Redチームの勝利演出

            Debug.Log("赤チームの勝ち");

        }
        //引き分け
        else if (m_redTotalDamage == m_blueTotalDamage)        
        {
            //引き分けの演出

            Debug.Log("引き分けでやんす");
        }

    }

    private void RoundUpdate()
    {
        //ラウンドの数を減らす
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

        //m_reset = true;

    }

    public void VictoryOrDefeatDirection()
    {
        
        
    }

    //位置をリセットする　対象： プレイヤー ＆ ボール
    public void ResetPosition()
    {

        bool[] check = { false, false, false, false };

        //チーム別で位置を初期化する
        foreach (GameObject player in m_listPlayerData)
        {
            int num = 0;

            foreach (GameObject spawnPoint in m_listSpawnPoints)
            {
                if (player.layer == spawnPoint.gameObject.layer && !check[num])
                {
                    player.transform.position = spawnPoint.transform.position;  //位置の初期化
                    player.transform.rotation = spawnPoint.transform.rotation;  //回転の初期化
                    //player.GetComponent<ThirdPersonController>().CinemachineTargetYaw;
                    check[num] = true;

                    break;
                }
                num++;

                
            }
        }

        //ボールの位置を初期化する
        //GameObject.FindWithTag("Ball1").GetComponent<Ball>().ResetPosition();

        //フラグのリセット
        m_resetFlag = false;
    }

    public void StartRound()
    {
        m_timer -= Time.deltaTime;

        m_countSystem.CountDown();

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

        //Debug.Log("現在のタイマー"+　m_timer);

        
    }

    private void SettingCharacterInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            //奇数チーム
            if (i % 2 == 1)
            {
                blueTeam.Add(m_listPlayerData[i]);
                m_listPlayerData[i].GetComponent<ThirdPersonController>().CinemachineTargetYaw = 180;
            }
            //偶数チーム
            if (i % 2 == 0)
            {
                redTeam.Add(m_listPlayerData[i]);
            }

            tpc.Add(m_listPlayerData[i].GetComponent<ThirdPersonController>());
        }

        tpc[0].SetEnemy = blueTeam;
        tpc[0].SetAlly = redTeam[1];

        tpc[1].SetEnemy = redTeam;
        tpc[1].SetAlly = blueTeam[1];

        tpc[2].SetEnemy = blueTeam;
        tpc[2].SetAlly = redTeam[0];

        tpc[3].SetEnemy = redTeam;
        tpc[3].SetAlly = blueTeam[0];
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

    public float RedTeamTotal
    {
        get { return m_redTotalDamage; }
        set { m_redTotalDamage = value; }
    }
    public float BlueTeamTotal
    {
        get { return m_blueTotalDamage; }
        set { m_blueTotalDamage = value; }
    }

}
