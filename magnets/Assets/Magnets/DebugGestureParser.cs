using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.Events;

namespace Magnets
{
    public class DebugGestureParser : MonoBehaviour
    {
        public UnityEvent onStartRecognizing, onEndRecognizing;
        public KeyCode reconModeKey;

        public void ParseGestureChoice(Result gestureResult)
        {
            Debug.Log(string.Format("Gesture [{0}] {1}", gestureResult.GestureClass, gestureResult.Score));
        }

        private void Update()
        {
            if (Input.GetKeyDown(reconModeKey))
            {
                if (onStartRecognizing != null) onStartRecognizing.Invoke();
            }
            if (Input.GetKeyUp(reconModeKey))
            {
                if (onEndRecognizing != null) onEndRecognizing.Invoke();
            }
        }
    }

}