using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Production : MonoBehaviour
{
    // ���o�J�n�܂ł̒x��
    [Tooltip("�N�����牉�o�J�n�܂ł̒x��  1�b = 60")]
    [SerializeField] float delay;

    // �{�����o�̃t���O
    [Tooltip("�g�剉�o�̐؂�ւ�")]
    [SerializeField] bool isExpansion;
    [Tooltip("��]���o�̐؂�ւ�")]
    [SerializeField] bool isRotate;
    [Tooltip("�ړ����o�̐؂�ւ�")]
    [SerializeField] bool isMovement;

    // ���o�J�n�p�^�C�}�[
    float timer;

    // �g�剉�o�Ɏg���X�P�[��
    Vector3 scale;
    // ��]���o�Ɏg���p�x
    float angle;
    float angle2;

    // �g�剉�o�ɂ��ǂ��܂Ŋg�傷�邩
    [Tooltip("�ǂ��܂Ŋg�傷�邩(isExpansion��true�̏ꍇ�̂ݓK��)")]
    [SerializeField] float scaleSize;

    // ��]���o�ɂ�艽�x(�p�x)��]���邩
    [Tooltip("����]���邩(isRotate��true�̏ꍇ�̂ݓK��)")]
    [SerializeField]�@int rotateNum;

    // ����
    private void Start()
    {
        // �������֐�
        this.Initialize();

        // �g�剉�oON�Ȃ珉���T�C�Y��0��
        if (isExpansion)
        {
            transform.localScale = new Vector3(0, 0, 0);
        }

        // ��]���oON�Ȃ��]����p�x���Z�o
        if (isRotate)
        {
            angle = rotateNum * 360f;
            angle2 = 0f;
        }
    }

    // ������
    private void Initialize()
    {
        // �^�C�}�[�̏�����
        timer = 0.0f;
        // ���o�Ɏg���ꎞ�I�ȃX�P�[���ۑ��ϐ�������
        scale = new Vector3(0, 0, 0);
    }

    // �X�V
    private void Update()
    {
        // �^�C�}�[�X�V
        timer++;

        // ���o���J�n���邩�ǂ���
        if (timer <= delay) { return; }

        // �g��t���OON�Ȃ�
        if (isExpansion) { this.Expansion(); }

        // ��]�t���OON�Ȃ�
        if (isRotate) { this.Rotate(); }
    }

    // �g�剉�o�֐�
    private void Expansion()
    {
        scale.x += Mathf.Abs(scaleSize - scale.x) / 20f;
        scale.y += Mathf.Abs(scaleSize - scale.y) / 20f;
        scale.z += Mathf.Abs(scaleSize - scale.z) / 20f;
        // �K��
        this.transform.localScale = scale;
    }

    // ��]���o�֐�
    private void Rotate()
    {
        //angle2 = Mathf.Abs(angle - angle2) / 20;

        //transform.localRotation = new Quaternion(0,0,angle2,0);
    }
}
