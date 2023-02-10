using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUI : MonoBehaviour
{
    [SerializeField]
    private GameObject text;

    [SerializeField]
    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        if(text==null)
        {
            text = transform.GetChild(0).gameObject;
        }
        if(timer==null)
        {
            timer = GameObject.Find("Timer").GetComponent<Timer>();
        }
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Minutes <= 0)
        {
            text.SetActive(true);
        }
    }
}
