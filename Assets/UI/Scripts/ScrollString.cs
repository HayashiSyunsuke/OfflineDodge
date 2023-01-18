using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollString : MonoBehaviour
{
    private RectTransform m_rectTransform;

    [SerializeField]
    private Transform m_returnTransform;

    [SerializeField]
    private Transform nitializeTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_rectTransform.position.x >= m_returnTransform)
        //{m_returnTransform
        //    m_rectTransform.position = m_initializeTransform.position;
        //}

        m_rectTransform.transform.position += new Vector3(1.0f, 1.0f * (Mathf.Deg2Rad * 24.0f), 0.0f);

        if (transform.position.x >= m_returnTransform.position.x)
        {
            transform.position = nitializeTransform.position;
        }

        transform.position += new Vector3(1.0f, 1.0f * (Mathf.Deg2Rad * 24.0f), 0.0f);
    }
}
