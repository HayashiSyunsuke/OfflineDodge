using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
//using ExitGames.Client.Photon;

// IPunObservableインターフェースを実装して、PhotonViewの監視対象コンポーネントにする
public class PhotonDataSend : MonoBehaviourPunCallbacks, IPunObservable
{
    //[SerializeField] Player player = null;
    [SerializeField] Text text;
    public ThirdPersonController thirdPersonController;

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            // Transformの値をストリームに書き込んで送信する
            stream.SendNext(thirdPersonController.hp);
            
        }
        else
        {
            // 受信したストリームを読み込んでTransformの値を更新する
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