using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
//using ExitGames.Client.Photon;

// IPunObservable�C���^�[�t�F�[�X���������āAPhotonView�̊Ď��ΏۃR���|�[�l���g�ɂ���
public class PhotonDataSend : MonoBehaviourPunCallbacks, IPunObservable
{
    //[SerializeField] Player player = null;
    [SerializeField] Text text;
    public ThirdPersonController thirdPersonController;

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            // Transform�̒l���X�g���[���ɏ�������ő��M����
            stream.SendNext(thirdPersonController.hp);
            
        }
        else
        {
            // ��M�����X�g���[����ǂݍ����Transform�̒l���X�V����
            //thirdPersonController.hp = (int)stream.ReceiveNext();
            text.text = (string)stream.ReceiveNext();
        }

    }

    void Update()
    {
        //thirdPersonController= GameObject.Find("FemaleDummy1").GetComponent<ThirdPersonController>();
        int stageId = (PhotonNetwork.CurrentRoom.CustomProperties["StageId"] is int value) ? value : 0;
    }
}