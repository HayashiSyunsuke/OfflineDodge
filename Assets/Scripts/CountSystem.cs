using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSystem : MonoBehaviour
{
    [SerializeField]
    private List<Image> m_numbersImage;
    [SerializeField]
    private Image m_startImage;

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

        if (m_startCount <= 0.0f)
        {
            m_count -= Time.deltaTime;

            foreach (Image image in m_numbersImage)
            {
                image.gameObject.SetActive(false);
            }

            m_startImage.gameObject.SetActive(true);

            if (m_count <= 0.0f)
            {
                m_startImage.gameObject.SetActive(false);
                m_end = true;
                timerUI.StartFlag = true;
            }

        }
        else
        {
            int time = (int)m_startCount + 1;

            m_startImage.gameObject.SetActive(false);

            if (time <= 3)
            {
                if (time <= 1)
                {
                    m_numbersImage[0].gameObject.SetActive(true);
                    m_numbersImage[1].gameObject.SetActive(false);
                    m_numbersImage[2].gameObject.SetActive(false);
                }
                else if (time <= 2)
                {
                    m_numbersImage[1].gameObject.SetActive(true);
                    m_numbersImage[2].gameObject.SetActive(false);
                    m_numbersImage[0].gameObject.SetActive(false);
                }
                else if (time <= 3)
                {
                    m_numbersImage[2].gameObject.SetActive(true);
                    m_numbersImage[1].gameObject.SetActive(false);
                    m_numbersImage[0].gameObject.SetActive(false);
                }

            }
        }

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
