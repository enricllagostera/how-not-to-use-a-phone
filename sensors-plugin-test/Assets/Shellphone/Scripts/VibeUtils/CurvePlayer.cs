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
        [ReadOnly] public float duration;
        public bool playAudio;
        private AudioSource _audio;

        void Start()
        {
            if (playAudio)
            {
                _audio = GetComponent<AudioSource>();
            }
        }

        void Update()
        {
            if (isPlaying)
            {
                timer += Time.deltaTime;
                player.CurvePlay(curve.GetIntensity(timer));
                if (playAudio)
                {
                    _audio.volume = curve.GetIntensity(timer);
                }
                if (duration >= 0)
                {
                    if (timer >= duration)
                    {
                        if (playAudio) _audio.Stop();
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
            _audio.Play();
            _audio.volume = curve.GetIntensity(timer);
        }

        internal void Stop()
        {
            timer = 0f;
            isPlaying = false;
            player.Stop();
            if (playAudio) _audio.Stop();
        }
    }
}