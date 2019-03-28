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
        List<Point> points;
        private Vector3 inputValue;
        private int strokeId;
        public List<Gesture> trainingSet = new List<Gesture>();
        public TextMeshProUGUI lbl;

        // Start is called before the first frame update
        void Start()
        {
            points = new List<Point>();
        }

        void Update()
        {
            // recording for recognizing
            if (Input.GetKey(KeyCode.Space))
            {
                lbl.text = "hmmm...";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    strokeId = 0;
                    points.Clear();
                }
                points.Add(new Point(inputValue.x, inputValue.y, strokeId));
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Gesture candidate = new Gesture(points.ToArray());
                Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

                print(gestureResult.GestureClass + " " + gestureResult.Score);
                lbl.text = "this is a...\n" + gestureResult.GestureClass + "?\n(" + (gestureResult.Score * 100f).ToString("00.0") + "% sure)";
            }


            // recording gesture
            if (Input.GetKey(KeyCode.R))
            {
                // this will be a one stroke gesture for now
                if (Input.GetKeyDown(KeyCode.R))
                {
                    strokeId = 0;
                    points.Clear();
                }
                points.Add(new Point(inputValue.x, inputValue.y, strokeId));
            }
            // finish recording and add to training set
            if (Input.GetKeyUp(KeyCode.R))
            {
                trainingSet.Add(new Gesture(points.ToArray(), "gesture_" + DateTime.Now.ToFileTime()));
            }
        }


        /*

            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

            message = gestureResult.GestureClass + " " + gestureResult.Score;


         */
        public void UpdateInputValue(Vector3 value)
        {
            inputValue = value;
        }
    }

}