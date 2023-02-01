using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCrash : MonoBehaviour
{
    private Animator anim;
    int animTimer;

    bool crashFlag = false;
    public flagManager manager;

    public slid slid;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slid.wallCrashFlag == true)
        {
            crashFlag = true;
        }
        if (crashFlag == true)
        {
            anim.SetBool("wallCrash", true);
        }

        if (anim.GetBool("wallCrash") == true)
        {
            animTimer++;
        }

        if (animTimer >= 73)
        {
            anim.SetBool("wallCrash", false);
            animTimer = 0;
            crashFlag = false;
        }
    }
}
