using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAndroidSensors.Scripts.Utils.SmartVars;

namespace Shellphone
{
    public class MagneticRotate : MonoBehaviour
    {
        public Vector3Var magneticField;
        void Update()
        {
            transform.right = new Vector3(0, 0, magneticField.value.x);
        }
    }
}