using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWallMaria : MonoBehaviour
{

    public static bool pickUpBallTakasi = false;
    //public bool pickUpBallTakasiFlag
    //{
    //    get{ return this.pickUpBallTakasi; }
    //    private set{ this.pickUpBallTakasi = bool takasi; }
    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //OnTriggerEnter�֐�
    //�ڐG�����I�u�W�F�N�g������other�Ƃ��ēn�����
    void OnTriggerEnter(Collider other)
    {
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("Ball1"))
        {
            Debug.Log("deta---");
            pickUpBallTakasi = true;
        }
    }
}
