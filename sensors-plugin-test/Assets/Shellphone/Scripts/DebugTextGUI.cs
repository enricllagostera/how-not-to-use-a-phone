using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityAndroidSensors.Scripts.Utils.SmartVars;
using TMPro;

namespace SensorTest
{
    public class DebugTextGUI : MonoBehaviour
    {
        private TextMeshProUGUI msgGUI;
        public FloatVar varValue;

        void Start()
        {
            msgGUI = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            msgGUI.text = varValue.value.ToString("000.000");
        }
    }
}


