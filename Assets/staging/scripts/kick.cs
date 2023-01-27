using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kick : MonoBehaviour
{
    private Animator anim;
    int animTimer;

    bool kickFlag = false;
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
        if (slid.kickFlag == true)
        {
            kickFlag = true;
        }
        if (kickFlag == true)
        {
            anim.SetBool("kick", true);
        }

        if (anim.GetBool("kick") == true)
        {
            animTimer++;
        }

        if (animTimer >= 44)
        {
            anim.SetBool("kick", false);
            animTimer = 0;
            kickFlag = false;
        }
    }
}
