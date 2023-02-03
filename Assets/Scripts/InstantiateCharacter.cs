using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCharacter : MonoBehaviour
{

    [SerializeField] GameObject[] player;

    bool flag = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flag) return;

        if(GameObject.Find("FemaleDummy1"))
        {
            for(int i = 0;i < 3;i++)
            {
                Instantiate(player[i]);
                
            }

            flag = true;
        }
    }
}
