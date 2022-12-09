using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarker : MonoBehaviour
{
    //EnemyMarkerPosition
    Vector3 marker = new Vector3();

    //ThirdPersonController
    public GameObject player;

    void Start()
    {
        if(player.GetComponent<ThirdPersonController>().CurrentTarget != null)
        marker = player.GetComponent<ThirdPersonController>().CurrentTargetPosition;
    }

    void Update()
    {
        if (player.GetComponent<ThirdPersonController>().CurrentTarget != null)
        {
            marker = player.GetComponent<ThirdPersonController>().CurrentTargetPosition;

            marker.y += 1.5f;

            transform.position = marker;
        }
    }
}
