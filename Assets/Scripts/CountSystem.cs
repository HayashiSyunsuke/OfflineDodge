using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSystem : MonoBehaviour
{
    [SerializeField]
    private List<Image> m_numbersImage;

    [SerializeField]
    private GameObject m_readyImage;

    [SerializeField]
    private GameObject m_timeUpImage;

    private float m_startCount;

    private float m_count = 0.6f;

    [SerializeField]
    private float CountTime = 2.0f;

    [SerializeField]
    private GameRule m_gameRule;

    private bool m_end = false;

    [SerializeField]
    Timer timerUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void CountDown()
    {
        m_startCount = m_gameRule.Timer;

        m_count -= Time.deltaTime;

        //m_timeUpImage.gameObject.SetActive(false);

        if (m_count < 0.0f)
            m_readyImage.gameObject.SetActive(true);


        if (m_startCount <= 0.0f)
        {


            if (m_count <= 0.0f)
            {
                //m_timeUpImage.gameObject.SetActive(false);
                m_end = true;
                timerUI.StartFlag = true;
            }

        }

    }

    public void TimeUp()
    {
        m_timeUpImage.gameObject.SetActive(true);
    }

    public void ResetCount()
    {
        m_count = 0.6f;
        m_end = false;
    }

    public bool End
    {
        get { return m_end; }
    }

}
