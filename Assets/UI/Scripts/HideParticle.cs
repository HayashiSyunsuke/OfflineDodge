using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideParticle : MonoBehaviour
{
    [SerializeField]
    private Timer timer;

    void Start()
    {
        if (timer == null)
        {
            timer = GameObject.Find("Timer").GetComponent<Timer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Minutes <= 0)
        {
            this.gameObject.SetActive(false);

        }
    }
}
