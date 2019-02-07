using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace VibeUtils
{
    public class CurvePlayer : MonoBehaviour
    {
        public bool isPlaying;
        float timer;
        public CurveFX curve;
        public VibePlayer player;
        public float duration;

        void Update()
        {
            if (isPlaying)
            {
                timer += Time.deltaTime;
                player.CurvePlay(curve.GetIntensity(timer));
                if (duration >= 0)
                {
                    if (timer >= duration)
                    {
                        player.Stop();
                    }
                }
                isPlaying = player.isPlaying;
            }
        }

        public void Play()
        {
            timer = 0f;
            duration = curve.GetDuration();
            isPlaying = true;
            player.isPlaying = false;
        }

        internal void Stop()
        {
            timer = 0f;
            isPlaying = false;
            player.Stop();
        }
    }
}