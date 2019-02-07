using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VibeUtils;

namespace Shellphone
{
    public class VibrationTest : MonoBehaviour
    {
        public long[] pattern;
        bool hasTouched = false;

        void Update()
        {
            if (Input.touchCount == 1 && !hasTouched)
            {
                //Debug.Log(Vibration.HasVibrator());
                Vibration.Vibrate(pattern, 0);
                hasTouched = true;
            }

            if (Input.touchCount > 2 && hasTouched)
            {
                //Debug.Log(Vibration.HasVibrator());
                Vibration.Cancel();
                hasTouched = false;
            }
        }
    }
}