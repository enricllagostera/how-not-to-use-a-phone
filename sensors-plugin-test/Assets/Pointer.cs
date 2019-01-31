using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAndroidSensors.Scripts.Utils.SmartVars;

public class Pointer : MonoBehaviour
{
    public Vector3Var orientation;

    void Update()
    {
        transform.right = orientation.value;
    }
}
