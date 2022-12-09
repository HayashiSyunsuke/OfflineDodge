using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Cinemachine;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class PhotonConnect : MonoBehaviourPunCallbacks
{
    private CinemachineTargetGroup targetGroup;

    private void Start()
    {
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
        targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();

    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom("RoomTakasi", new RoomOptions(), TypedLobby.Default);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
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

        //RpcSendMessage("����ɂ���");

        //if(RpcTarget.All != 0)
        //photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "���͂悤");
    }

    [PunRPC]
    private void RpcSendMessage(string message)
    {
        Debug.Log(message);
    }

}