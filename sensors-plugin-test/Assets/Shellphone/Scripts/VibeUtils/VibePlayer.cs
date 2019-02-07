using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace VibeUtils
{
    public class VibePlayer : MonoBehaviour
    {
        public float value;
        public PlayerConfig config;
        public bool isPlaying;
        private float timer;
        private long[] lastPattern;

        void Update()
        {
            if (isPlaying)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Vibration.Cancel();
                    timer = config.DurationFromPattern(lastPattern);
                    Vibration.Vibrate(lastPattern, 0);
                }
            }
        }

        public void Stop()
        {
            isPlaying = false;
            Vibration.Cancel();
        }

        public void Play(float value)
        {
            Vibration.Cancel();
            if (value <= 0f)
            {
                isPlaying = false;
                return;
            }
            isPlaying = true;
            timer = 0f;
            lastPattern = config.GetPatternFromRatio(value);
        }

        public void CurvePlay(float value)
        {
            if (!isPlaying)
            {
                timer = 0f;
                isPlaying = true;
            }
            lastPattern = config.GetPatternFromRatio(value);
        }
    }
}