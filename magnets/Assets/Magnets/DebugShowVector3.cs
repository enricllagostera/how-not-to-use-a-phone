using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Magnets
{
    public class DebugShowVector3 : MonoBehaviour
    {
        public bool showAsScale;
        [EnableIf("showAsScale")]
        public RectTransform imgX, imgY, imgZ;
        [EnableIf("showAsScale")]
        public float maxAxisValue;

        public bool showAs3DVector;
        [EnableIf("showAs3DVector")]
        public Transform directionIndicator;
        private Vector3 previousValue, inputValue, baseValue1, baseValue2;
        private bool testLerp;

        public void ShowVector3AsScale(Vector3 value)
        {
            imgX.sizeDelta = ComputeNewSize(imgX.sizeDelta, value.x.Remap(-300f, 300f, 0f, maxAxisValue));
            imgY.sizeDelta = ComputeNewSize(imgY.sizeDelta, value.y.Remap(-300f, 300f, 0f, maxAxisValue));
            imgZ.sizeDelta = ComputeNewSize(imgZ.sizeDelta, value.z.Remap(-300f, 300f, 0f, maxAxisValue));
        }

        public void ShowVector3AsDirection(Vector3 value)
        {
            previousValue = inputValue;
            directionIndicator.rotation = Quaternion.Euler(value);
            inputValue = value;
        }

        private Vector2 ComputeNewSize(Vector2 size, float v)
        {
            var result = size;
            result.y = v;
            return result;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                baseValue1 = inputValue;
                print("base1: " + baseValue1);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                baseValue2 = inputValue;
                print("base2: " + baseValue2);
            }

            testLerp = Input.GetKey(KeyCode.I);

            if (testLerp)
            {
                float progress = InverseLerp(baseValue1, baseValue2, inputValue);
                print("lerp progress: " + progress);
            }

        }

        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(directionIndicator.position, previousValue);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(directionIndicator.position, inputValue);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(directionIndicator.position, baseValue1 - baseValue2);
        }
    }

}