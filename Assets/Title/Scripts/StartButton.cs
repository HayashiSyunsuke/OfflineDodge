using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartButton : MonoBehaviour
{
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
        if(input.start)
        SceneManager.LoadScene("SampleScene");
    }
}
