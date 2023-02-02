using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slid : MonoBehaviour
{
    private Animator anim;
    float animTimer;
    bool slidFlag = false;
    public flagManager manager;
    bool a = false;
    public bool kickFlag = false;
    public bool wallCrashFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.flag == true)
        {
            slidFlag = true;
        }
        if (slidFlag == true)
        {
            anim.SetBool("slid", true);
        }

        if (anim.GetBool("slid") == true)
        {
            a = true;
            animTimer += Time.deltaTime;
        }
        if (!a)
        {
            return;
        }
        if (animTimer >= 5.0f)
        {
            kickFlag = true;
            wallCrashFlag = true;
            anim.SetBool("slid", false);
            animTimer = 0;
            slidFlag = false;
        }
    }
}
