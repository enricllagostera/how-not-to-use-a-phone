using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrainingSetRecorder : MonoBehaviour
{
    public KeyCode recordKey;
    public UnityEvent onStartedRecording, onFinishedRecording;

    void Update()
    {
        if (Input.GetKeyDown(recordKey))
        {
            if (onStartedRecording != null) onStartedRecording.Invoke();
        }

        if (Input.GetKeyUp(recordKey))
        {
            if (onFinishedRecording != null) onFinishedRecording.Invoke();
        }
    }
}
