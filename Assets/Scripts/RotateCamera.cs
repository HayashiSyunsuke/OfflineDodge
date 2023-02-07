using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    float rotate = 1f;
    void Update()
    {
        rotate = 1f * Time.deltaTime;
        transform.Rotate(new Vector3(0.0f, rotate, 0.0f));
    }
}
