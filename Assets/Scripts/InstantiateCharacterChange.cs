using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstantiateCharacterChange : MonoBehaviour
{
    [SerializeField] PlayerInputManager playerInputManager;
    [SerializeField] PlayerCounter pc;

    [SerializeField] GameObject[] players;

    private void Update()
    {
        if (pc.PlayerNum >= 4)
            return;

        playerInputManager.playerPrefab = players[pc.PlayerNum];
    }
}
