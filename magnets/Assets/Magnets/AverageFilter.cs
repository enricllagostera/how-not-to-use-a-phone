using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Magnets
{
    public class AverageFilter : MonoBehaviour
    {
        public List<Vector3> readings;
        public Vector3 avgV3;
        public int frameWindow;
        public DebugShowVector3 debugV3;

        public Vector3Event OnNewAverage;



        void OnEnable()
        {
            readings = new List<Vector3>();
            avgV3 = new Vector3();
        }

        public void AddNewReading(Vector3 value)
        {
            readings.Add(value);
            if (readings.Count > frameWindow)
            {
                readings.RemoveAt(0);
            }
            avgV3 = new Vector3();
            avgV3.x = readings.Average(a => a.x);
            avgV3.y = readings.Average(a => a.y);
            avgV3.z = readings.Average(a => a.z);
            debugV3.ShowVector3AsScale(avgV3);
            if (OnNewAverage != null) OnNewAverage.Invoke(avgV3);
        }
    }

}