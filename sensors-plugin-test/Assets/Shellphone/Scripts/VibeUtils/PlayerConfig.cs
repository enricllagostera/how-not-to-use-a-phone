using UnityEngine;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

namespace VibeUtils
{

    [CreateAssetMenu(fileName = "VibrationFX", menuName = "VibeUtils/PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Slider(100f, 600f)]
        public float basePatternDuration = 600f; // in milliseconds

        [Slider(1f, 600f)] public float minSilence = 50f;
        [Slider(50f, 600f)] public float minVibration = 100f;

        public long[] GetPatternFromRatio(float ratio)
        {
            float onTime = Mathf.Max(minSilence, ratio * basePatternDuration);
            float offTime = Mathf.Max(minVibration, basePatternDuration - onTime);
            long[] pattern = new long[2] { (long)offTime, (long)onTime };
            return pattern;
        }

        public float DurationFromPattern(long[] pattern)
        {
            float duration = 0f;
            foreach (var band in pattern)
            {
                duration += band * 0.001f;
            }
            return duration;
        }
    }
}