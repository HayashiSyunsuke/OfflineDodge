using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageObject : MonoBehaviour
{
    const int NONE = 0b0000;
    const int ROTATE = 0b0001;
    const int EXPANSION = 0b0010;

    // 親オブジェクト
    [SerializeField] GameObject parent;

    // 演出するか否かのフラグ
    [Tooltip("回転するかどうか")]
    public bool isRotate;

    [Tooltip("拡大するかどうか")]
    public bool isExpansion;

    // 演出フラグ一括管理
    private int stagingInfo;

    // 演出開始時リセット済か判定するためのフラグ
    private bool isReset;
    public bool GetIsReset() { return isReset; }
    public void SetIsReset(bool flag) { isReset = flag; }

    private void Start()
    {
        // 親子関係の構築
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

    // 演出情報の取得
    public int GetStagingInfo()
    {
        return stagingInfo;
    }
}