using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffectsManager : MonoBehaviour
{
    private TrailRenderer m_trail;

    [SerializeField]
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        m_trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //ボールの速度でのトレイルの有無
        if(rb.velocity.magnitude >= 10.0f)
        {
            m_trail.emitting = true;
        }
        else if(rb.velocity.magnitude < 10.0f)
        {
            m_trail.emitting = false;
        }

        //速度でのトレイルの色の変化
        if(rb.velocity.magnitude >= 25.0f)
        {
            m_trail.startColor = Color.red;
        }
        else if(rb.velocity.magnitude >= 20.0f)
        {
            m_trail.startColor = Color.yellow;
        }
        else if(rb.velocity.magnitude >= 15.0f)
        {
            m_trail.startColor =new Vector4(0.0f,191.0f,255.0f,1.0f);
        }
        else if (rb.velocity.magnitude < 15.0f)
        {
            m_trail.startColor = Color.white;
        }

    }
}
