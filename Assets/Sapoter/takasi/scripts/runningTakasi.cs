using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runningTakasi : MonoBehaviour
{
    private Animator anim;
    int animTimer;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        animTimer++;

        if (animTimer >= 600)
        {
            anim.SetBool("kokeru", true);

        }
        if (animTimer >= 650)
        {
            anim.SetBool("kokeru", false);
            animTimer = 0;

        }
    }
}
