using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.Events;
using PDollarGestureRecognizer;
using System;
using UnityEngine.UI;

namespace RedDirt
{
    public class RedDirtStory : MonoBehaviour
    {
        [Header("Story & Logic")]
        public TextAsset inkAsset;
        public bool playOnStart;
        [Range(0.2f, 0.99f)] public float gestureScoreThreshold;
        private bool isPlaying;
        bool waitingForChoice = false;
        Story inkStory;

        [Header("Story UI")]
        public Text msgLbl;

        [Header("Events")]
        public UnityEvent onStartDecision;
        public UnityEvent onEndDecision;
        public VoiceOverEvent onShowNewLine;
        private bool justFinishedShowingLine;
        public Magnets.MagneticGestureReader reader;

        #region [PublicAPI]
        public void ParseGestureChoice(Result gestureResult)
        {
            if (waitingForChoice)
            {
                for (int i = 0; i < inkStory.currentChoices.Count; i++)
                {
                    if (gestureResult.GestureClass.ToLower() == inkStory.currentChoices[i].text.ToLower() && gestureResult.Score > gestureScoreThreshold)
                    {
                        ChooseChoice(i);
                        return;
                    }
                }
                // did not find any choice, picks last as default
                ChooseChoice(inkStory.currentChoices.Count - 1);
            }
        }

        public void OnFinishedShowingLine()
        {
            justFinishedShowingLine = true;
        }

        #endregion

        #region [Messages]

        private void Awake()
        {
            inkStory = new Story(inkAsset.text);
            waitingForChoice = false;
            isPlaying = false;
            justFinishedShowingLine = true;
            //reader.trainingSetFile = GameObject.FindObjectOfType<Registry>().trainingSet;
        }

        private void Start()
        {
            if (playOnStart)
            {
                PlayStory();
            }
        }

        #endregion

        #region [Private Methods]

        private void PlayStory()
        {
            isPlaying = true;
            StartCoroutine(ShowKnot());
        }


        private void ChooseChoice(int choice)
        {
            inkStory.ChooseChoiceIndex(choice);
            StartCoroutine(ShowKnot());
        }

        private IEnumerator ShowKnot()
        {
            while (inkStory.canContinue)
            {
                waitingForChoice = false;
                if (justFinishedShowingLine)
                {
                    justFinishedShowingLine = false;
                    msgLbl.text = inkStory.Continue();
                    var tags = inkStory.currentTags;
                    if (onShowNewLine != null)
                    {
                        onShowNewLine.Invoke(tags);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            while (!justFinishedShowingLine)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(2f);
            if (inkStory.currentChoices.Count > 0)
            {
                msgLbl.text = "";
                for (int i = 0; i < inkStory.currentChoices.Count; ++i)
                {
                    Choice choice = inkStory.currentChoices[i];
                    msgLbl.text += "\nChoice " + (i + 1) + ". " + choice.text;
                }
                msgLbl.text += "\n\n";
                waitingForChoice = true;
                if (onStartDecision != null)
                {
                    onStartDecision.Invoke();
                }
                StartCoroutine(EndChoicePeriod());
            }
        }

        private IEnumerator EndChoicePeriod()
        {
            yield return new WaitForSeconds(5f);
            if (onEndDecision != null)
            {
                onEndDecision.Invoke();
            }
        }

        #endregion
    }
}
