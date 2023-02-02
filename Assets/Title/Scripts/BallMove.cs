using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    // �x��
    [SerializeField] float delay;
    // �^�C�}�[
    float timer;

    // �v�Z���ʕۑ��p���W�ϐ�
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        Initailize();       
    }

    void Initailize()
    {
        // �E�ォ��X�^�[�g
        transform.position = new Vector3(2100, 1200, 0);
        timer = 0f;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�}�[�X�V
        timer++;

        // ���o���J�n���邩�ǂ���
        if (timer <= delay) { return; }

        pos.x -= Mathf.Abs(pos.x - 990) / 20f;
        pos.y -= Mathf.Abs(pos.y - 740) / 20f;

        // �K��
        this.transform.position = pos;
    }
}
