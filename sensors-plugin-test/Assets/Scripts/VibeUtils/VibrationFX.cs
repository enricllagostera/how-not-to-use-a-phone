using UnityEngine;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

namespace VibeUtils
{

    [CreateAssetMenu(fileName = "VibrationFX", menuName = "VibeUtils/VibrationFX", order = 0)]
    public class VibrationFX : ScriptableObject
    {
        public long[] highPattern;
        [ReadOnly]
        public int highPatternDuration;
        public long[] midPattern;
        [ReadOnly]
        public int midPatternDuration;
        public long[] lowPattern;
        [ReadOnly]
        public int lowPatternDuration;
        public int maxPatternDuration; // in milliseconds

        [Slider(40, 100)]
        public int bandDuration;
        public AnimationCurve amplitudeOverTime;



        [Button("Auto-populate patterns")]
        void GeneratePatterns()
        {
            highPattern = GeneratePattern(6, ref highPatternDuration);
            midPattern = GeneratePattern(3, ref midPatternDuration);
            lowPattern = GeneratePattern(1, ref lowPatternDuration);
        }

        private long[] GeneratePattern(int ratio, ref int resultDuration)
        {
            List<long> pattern = new List<long>();
            int patternDuration = maxPatternDuration;
            while (patternDuration > 0)
            {
                int newBand = bandDuration;
                if (pattern.Count % 2 > 0)
                {
                    newBand *= ratio;
                }
                if (patternDuration - newBand < 0)
                {
                    newBand = patternDuration;
                }
                else
                {
                    pattern.Add(newBand);
                }
                patternDuration -= newBand;
            }
            resultDuration = maxPatternDuration - patternDuration;
            return pattern.ToArray();
        }

        [Button("Update durations")]
        void UpdateDurations()
        {
            lowPatternDuration = 0;
            foreach (var band in lowPattern)
            {
                lowPatternDuration += (int)band;
            }

            midPatternDuration = 0;
            foreach (var band in midPattern)
            {
                midPatternDuration += (int)band;
            }

            highPatternDuration = 0;
            foreach (var band in highPattern)
            {
                highPatternDuration += (int)band;
            }
        }
    }
}