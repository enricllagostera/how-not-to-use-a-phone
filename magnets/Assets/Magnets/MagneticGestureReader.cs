using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System;
using TMPro;

namespace Magnets
{
    public class MagneticGestureReader : MonoBehaviour
    {
        public bool useTrainingSetFile = false;
        public TrainingSet trainingSetFile;
        List<Point> points;
        private Vector3 inputValue;
        private int strokeId;
        public List<Gesture> trainingSet = new List<Gesture>();
        public TextMeshProUGUI lbl;
        private bool isRecordingForTrainingSet;
        private bool isRecordingForRecognizing;
        public GestureResultEvent onRecognizedGesture;


        #region [Public API]

        public void UpdateInputValue(Vector3 value)
        {
            inputValue = value;
        }

        public void SetRecordingMode(bool recordMode)
        {
            // just started recording
            if (recordMode)
            {
                strokeId = 0;
                points.Clear();
            }
            // just stopped recording
            else
            {
                // has a recorded training set
                if (points.Count > 0)
                {
                    if (useTrainingSetFile)
                    {
                        trainingSetFile.allGestures.Add(new Gesture(points.ToArray(), "gesture_" + (trainingSetFile.allGestures.Count + 1)));
                    }
                    else
                    {
                        trainingSet.Add(new Gesture(points.ToArray(), "gesture_" + (trainingSet.Count + 1)));
                    }
                }
            }
            isRecordingForTrainingSet = recordMode;
        }

        public void SetRecognizingMode(bool recognizingMode)
        {
            if (recognizingMode)
            {
                strokeId = 0;
                points.Clear();
            }
            else
            {
                // runs the classification algorithm
                if (points.Count > 0)
                {
                    if (useTrainingSetFile)
                    {
                        trainingSet = trainingSetFile.allGestures;
                    }
                    Gesture candidate = new Gesture(points.ToArray());
                    Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                    if (onRecognizedGesture != null) onRecognizedGesture.Invoke(gestureResult);
                    print(gestureResult.GestureClass + " " + gestureResult.Score);
                }
            }
            isRecordingForRecognizing = recognizingMode;
        }

        #endregion


        #region [Messages]

        void Start()
        {
            points = new List<Point>();
            if (useTrainingSetFile)
            {
                trainingSet = trainingSetFile.allGestures;
            }
            isRecordingForRecognizing = false;
            isRecordingForTrainingSet = false;
        }

        void Update()
        {
            if (isRecordingForTrainingSet || isRecordingForRecognizing)
            {
                points.Add(new Point(inputValue.x, inputValue.y, strokeId));
            }
        }

        #endregion
    }

}