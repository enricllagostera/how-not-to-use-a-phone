using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedDirt
{
    public class VoiceOver : MonoBehaviour
    {
        public AudioSource audioPlayer;
        public VoiceOverDictionary voiceOverLines;

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