using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : MonoBehaviour
{
    [SerializeField] private float coolTime = 10.0f;
    [SerializeField] private float remaining = 0.0f;

    [SerializeField] private Text countdownTimer;

    private bool isUsed = false;

    void Start()
    {
        remaining = coolTime + 1;
    }

    void Update()
    {
        if (isUsed)
        {
            remaining = remaining - Time.deltaTime;

            if (remaining <= 1)
            {
                isUsed = false;
                remaining = coolTime + 1;
            }

            int timer = (int)remaining;
            countdownTimer.text = timer.ToString();
        }
        else
        {
            countdownTimer.text = "";
        }
    }

    /// <summary>
    /// Get::無し
    /// Set::クールタイム開始
    /// </summary>
    public bool CoolTimeFlag
    {
        get { return isUsed; }
        set { isUsed = value; }
    }
}
