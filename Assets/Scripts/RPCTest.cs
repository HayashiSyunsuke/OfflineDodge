using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCTest : MonoBehaviourPunCallbacks
{
    public bool isTalk = false;

    PhotonStream a;

    void Update()
    {
        if (isTalk)
        {
            RpcSendMessage("‚±‚ñ‚É‚¿‚Í");
            photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "‚¨‚Í‚æ‚¤");
            isTalk = false;
        }
    }

    [PunRPC]
    private void RpcSendMessage(string message)
    {
        Debug.Log(message);
    }
}
