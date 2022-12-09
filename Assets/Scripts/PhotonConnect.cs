using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Cinemachine;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class PhotonConnect : MonoBehaviourPunCallbacks
{
    private CinemachineTargetGroup targetGroup;

    private void Start()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
        targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();

    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("RoomTakasi", new RoomOptions(), TypedLobby.Default);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        var position = new Vector3(0,7,-5);
        GameObject clone = PhotonNetwork.Instantiate("FemaleDummy", position, Quaternion.identity);
        clone.name = "FemaleDummy" + PhotonNetwork.CountOfPlayers;
        GameObject ball = null;

        if (PhotonNetwork.CountOfPlayers == 1)
        {
            ball = PhotonNetwork.Instantiate("Ball", new Vector3(0, 3, 0), Quaternion.identity);
            ball.SetActive(true);
        }

        targetGroup.AddMember(clone.transform, 1f, 1f);

        //targetGroup.AddMember(ball.transform, 1f, 1f);
        

        //int a = PhotonNetwork.PlayerList[0].Get();

        //RpcSendMessage("こんにちは");

        //if(RpcTarget.All != 0)
        //photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "おはよう");
    }

    [PunRPC]
    private void RpcSendMessage(string message)
    {
        Debug.Log(message);
    }

}