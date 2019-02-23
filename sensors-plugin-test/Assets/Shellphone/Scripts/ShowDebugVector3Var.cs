using System.Collections;
using System.Collections.Generic;
using UnityAndroidSensors.Scripts.Utils.SmartVars;
using UnityEngine;
using UnityEngine.UI;

public class ShowDebugVector3Var : MonoBehaviour
{
    public bool showMagOnly;
    private Text _text;
    public Vector3Var var;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showMagOnly)
        {
            _text.text = var.value.sqrMagnitude.ToString("000.000");
        }
        else
        {
            _text.text = var.value.ToString();
        }

    }
}
