using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomAnimTakasi2 : MonoBehaviour
{
    private Animator anim;
    int animTimer;
    int rand;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rand = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        // ‰ñ“]‚µ‚È‚¢‚æ‚¤‚É‚·‚é
        this.transform.rotation = Quaternion.Euler(0, 180, 0);

        animTimer++;

        if (animTimer >= 600)
        {
            rand = Random.Range(1, 4);

            animTimer = 0;
        }

        if (rand == 1)
        {
            anim.SetBool("yanki-", true);
            anim.SetBool("sit_clap", false); ;
            anim.SetBool("kyodouhusin", false);
        }

        if (rand == 2)
        {
            anim.SetBool("sit_clap", true);
            anim.SetBool("yanki-", false);
            anim.SetBool("kyodouhusin", false);
        }

        if (rand == 3)
        {
            anim.SetBool("kyodouhusin", true);
            anim.SetBool("yanki-", false);
            anim.SetBool("sit_clap", false); ;
        }
    }
}
