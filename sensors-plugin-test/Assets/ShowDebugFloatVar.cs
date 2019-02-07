using System.Collections;
using System.Collections.Generic;
using UnityAndroidSensors.Scripts.Utils.SmartVars;
using UnityEngine;
using UnityEngine.UI;

public class ShowDebugFloatVar : MonoBehaviour
{
    private Text _text;
    public FloatVar floatVar;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = floatVar.value.ToString("000.000");
    }
}
