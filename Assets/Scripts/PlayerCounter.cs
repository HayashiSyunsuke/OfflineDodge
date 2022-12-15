using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounter : MonoBehaviour
{
    [SerializeField] private int PlayerCount;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int PlayerNum
    {
        get { return PlayerCount; }
        set { PlayerCount = value; }
    }

}
