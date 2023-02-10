using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;
public class StartButton : MonoBehaviour
{
    private bool isEndFade = false;

    [SerializeField] tekitou_fade fade;
    [SerializeField] Image image;
    [SerializeField] AudioSource audio;
    private bool isStart = false;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif

    private ChangeSampleScene input;

    private void Start()
    {
#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#else
        Debug.Log("Package is missing dependencies.")
#endif

        input = GetComponent<ChangeSampleScene>();
    }

    private void Update()
    {
        if(!isStart)
        isEndFade = fade.FadeOut(image, 1.0f);

        if (!isEndFade) return;

        if (input.start)
            isStart = true;

        if (!isStart) return;

        audio.volume -= 0.1f * Time.deltaTime;

        if(fade.FadeIn(image, 1.0f))
        {
            SceneManager.LoadScene("animationScene");
        }
    }
}
