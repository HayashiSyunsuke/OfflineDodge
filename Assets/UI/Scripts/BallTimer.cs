using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallTimer : MonoBehaviour
{
    [SerializeField]
    Text text;

    [SerializeField]
    ThirdPersonController player;

    private bool ones = true;

    private float totalTime;
    private float oldSeconds;

    float maxCount=11.0f;

    int timer = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        totalTime = Time.time;

        if (player.IsBallHaving)
        {
            if(ones)
            {
                oldSeconds = Time.time;
                ones = false;
            }

            if(timer>0)
            {
                timer = (int)(maxCount - (totalTime - oldSeconds));
            }
        }
        else
        {
            ones = true;
            timer = 10;
        }
        text.text = timer.ToString();
    }
}
