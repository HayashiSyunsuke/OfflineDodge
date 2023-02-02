using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageObject : MonoBehaviour
{
    const int NONE = 0b0000;
    const int ROTATE = 0b0001;
    const int EXPANSION = 0b0010;

    // �e�I�u�W�F�N�g
    [SerializeField] GameObject parent;

    // ���o���邩�ۂ��̃t���O
    [Tooltip("��]���邩�ǂ���")]
    public bool isRotate;

    [Tooltip("�g�傷�邩�ǂ���")]
    public bool isExpansion;

    // ���o�t���O�ꊇ�Ǘ�
    private int stagingInfo;

    // ���o�J�n�����Z�b�g�ς����肷�邽�߂̃t���O
    private bool isReset;
    public bool GetIsReset() { return isReset; }
    public void SetIsReset(bool flag) { isReset = flag; }

    private void Start()
    {
        // �e�q�֌W�̍\�z
        this.transform.parent = parent.transform;

        this.CreateStagingInfo();
    }

    private void CreateStagingInfo()
    {
        int bit = NONE;

        if (isRotate) { stagingInfo = bit | ROTATE;  }
        if (isExpansion) { stagingInfo = bit | EXPANSION; }

        Debug.Log(stagingInfo.ToString());
    }

    // ���o���̎擾
    public int GetStagingInfo()
    {
        return stagingInfo;
    }
}