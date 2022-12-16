using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeControll: MonoBehaviour
{
    Fade fade;

    bool ones = true;

    public bool IsFadeOut = false;
    public bool IsFadeIn = false;

    private float fadeRange;

    [SerializeField]
    float fadeTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<Fade>();
        IsFadeOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        fadeRange = fade.GetRange();

        if (IsFadeIn)
        {
            StartFadeIn();
        }
        if (IsFadeOut)
        {
            StartFadeOut();
        }
    }

    //フェードインする
    void StartFadeIn()
    {
        if (ones) 
        {
            fade.FadeIn(fadeTime);
            ones = false;
        }
        if (fadeRange >= 1)
        {
            ones = true;
            IsFadeIn = false;
        }
    }

    //フェードアウトする
    void StartFadeOut()
    {
        if (ones)
        {
            fade.FadeOut(fadeTime);
            ones = false;
        }
        if (fadeRange <= 0)
        {
            ones = true;
            IsFadeOut = false;
        }
    }

    //フェードの強さ
    public float FadeRange
    {
        get
        {
            return fadeRange;
        }
        set
        {
            fadeRange = value;
        }
    }
}
