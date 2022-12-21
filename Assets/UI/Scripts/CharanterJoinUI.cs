using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharanterJoinUI : MonoBehaviour
{
    [SerializeField] bool isJoin;
    [SerializeField] Image image;
    [SerializeField] float alpha;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnJoin();
    }

    private void OnJoin()
    {
        if(isJoin)
        {
            if (image.color.a < 1f)
                alpha += 0.03f;
            else
                isJoin = false;

            image.color = new Vector4(255f, 255f, 255f, alpha);
            this.transform.localPosition += new Vector3(0, -3f, 0);
        }
    }

    public bool JoinFlag
    {
        get { return isJoin; }
        set { isJoin = value; }
    }
}
