using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ChangeTitleScene : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] tekitou_fade fade;

    private bool isEndFade = false;
    private bool isStart = false;

    private ChangeSampleScene input;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif


    void Start()
    {
#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#else
        Debug.Log("Package is missing dependencies.")
#endif

        input = GetComponent<ChangeSampleScene>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStart)
        isEndFade = fade.FadeOut(fadeImage, 2.0f);

        if (!isEndFade) return;

        if (input.start)
            isStart = true;

        if (!isStart) return;

        if (fade.FadeIn(fadeImage, 0.5f) && isStart)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
