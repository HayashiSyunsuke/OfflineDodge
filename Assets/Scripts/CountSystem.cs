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
            }
                
        }
        else
        {
            int time = (int)m_startCount + 1;

            m_startImage.gameObject.SetActive(false);

            foreach (Image image in m_numbersImage)
            {
                image.gameObject.SetActive(false);
            }

            m_numbersImage[time - 1].gameObject.SetActive(true);

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
