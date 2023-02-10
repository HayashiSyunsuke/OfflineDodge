using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tekitou_fade : MonoBehaviour
{
    private bool isFirst = false;
    [SerializeField] private float alpha = 0.0f;

    public bool FadeIn(Image image, float fadeSpeed)
    {
        if(!isFirst)
        {
            alpha = 0.0f;
            isFirst = true;
        }

        if (alpha >= 1) return true;

        alpha += fadeSpeed * Time.deltaTime;
        image.color = new Vector4(0f, 0f, 0f, alpha);

        if (alpha >= 1) return true;
        else return false;
    }
    
    public bool FadeOut(Image image, float fadeSpeed)
    {
        if(!isFirst)
        {
            alpha = 1.0f;
            isFirst = true;
        }

        if (alpha <= 0) return true;

        alpha -= fadeSpeed * Time.deltaTime;
        image.color = new Vector4(0f, 0f, 0f, alpha);

        if (alpha <= 0) return true;
        else return false;
    }

    public void ResetAlpha()
    {
        alpha = 0;
    }

    public float Alpha
    {
        get { return alpha; }
        set { alpha = value; }
    }
}
