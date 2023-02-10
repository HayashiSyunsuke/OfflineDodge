using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] tekitou_fade fade;
    [SerializeField] Image image;
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
        if (input.start)
            isStart = true;

        if (!isStart) return;

        if(fade.FadeIn(image, 0.5f) && isStart)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
