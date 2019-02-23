using UnityEngine;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

namespace VibeUtils
{

    [CreateAssetMenu(fileName = "VibrationFX", menuName = "VibeUtils/CurveFX", order = 1)]
    public class CurveFX : ScriptableObject
    {
        public bool looping = false;
        public AnimationCurve intensityCurve;

        public float GetIntensity(float time)
        {
            return intensityCurve.Evaluate(time);
        }

        public float GetDuration()
        {
            if (looping)
            {
                return -1f;
            }
            return intensityCurve.keys[intensityCurve.length - 1].time;
        }
    }
}