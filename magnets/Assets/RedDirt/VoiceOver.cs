using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedDirt
{
    public class VoiceOver : MonoBehaviour
    {
        public AudioSource audioPlayer;
        public UnityEvent onFinishPlayingLine;
        public VoiceOverDictionary voiceOverLines;
        private bool wasPlaying;

        #region [PublicAPI]
        public void PlayNextLine(List<string> lineTags)
        {
            AudioClip clip = null;
            foreach (var tag in lineTags)
            {
                clip = FindAudioForLine(tag);
                if (clip != null)
                {
                    break;
                }
            }
            audioPlayer.Stop();
            audioPlayer.PlayOneShot(clip, 1f);
        }
        #endregion

        #region [Messages]
        private void Awake()
        {
            wasPlaying = false;
        }

        private void Update()
        {
            if (wasPlaying && !audioPlayer.isPlaying)
            {
                if (onFinishPlayingLine != null)
                {
                    onFinishPlayingLine.Invoke();
                }
            }
            wasPlaying = audioPlayer.isPlaying;
        }
        #endregion


        #region [PrivateMethods]
        private IDictionary<string, AudioClip> VOLines
        {
            get { return voiceOverLines as IDictionary<string, AudioClip>; }
            set { voiceOverLines.CopyFrom(value); }
        }

        private AudioClip FindAudioForLine(string tag)
        {
            AudioClip result = null;
            if (VOLines.TryGetValue(tag, out result))
            {
                return result;
            }
            return result;
        }
        #endregion
    }
}