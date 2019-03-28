using System.Collections;
using System.Collections.Generic;
using OscJack;
using UnityEngine;
using System.Linq;

namespace Magnets
{
    public class SensorFilter : MonoBehaviour
    {
        public List<Vector3> readings;
        private int portNumber = 9000;
        public string oscEndpoint = "/magneticfield";
        private OscServer server;

        public DebugShowVector3 debugV3;
        public int frameWindow = 5;
        public Vector3 avgV3;

        public Vector3Event OnUpdatedSensors;

        void OnEnable()
        {
            readings = new List<Vector3>();
            server = new OscServer(portNumber); // Port number
            server.MessageDispatcher.AddCallback(
                oscEndpoint,
                (string address, OscDataHandle data) =>
                    {
                        readings.Add(new Vector3(data.GetElementAsFloat(0), data.GetElementAsFloat(1), data.GetElementAsFloat(2)));
                        if (readings.Count > frameWindow)
                        {
                            readings.Remove(readings.Last());
                        }
                        avgV3 = new Vector3();
                        avgV3.x = readings.Average(a => a.x);
                        avgV3.y = readings.Average(a => a.y);
                        avgV3.z = readings.Average(a => a.z);
                    }
                );
        }


        void Update()
        {
            if (debugV3 != null)
            {
                debugV3.ShowVector3AsScale(avgV3);
            }
            if (OnUpdatedSensors != null)
            {
                OnUpdatedSensors.Invoke(avgV3);
            }
        }

        void OnDisable()
        {
            server.Dispose();
        }
    }

}