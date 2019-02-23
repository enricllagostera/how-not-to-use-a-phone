using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        Debug.Log("gyro " + SystemInfo.supportsGyroscope);
        Debug.Log("vibration " + SystemInfo.supportsVibration);
        Debug.Log("motionVectors " + SystemInfo.supportsMotionVectors);
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = -Input.gyro.gravity;
        Debug.Log(Input.gyro.attitude);
    }
}
