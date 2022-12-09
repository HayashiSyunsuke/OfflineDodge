using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringAct : MonoBehaviour
{
    private int posX = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posX++;
        this.transform.position = new Vector3(posX, 0, 0);
    }
}
