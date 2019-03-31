using System;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.Events;

namespace Magnets
{
    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    [Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }

    [Serializable]
    public class GestureResultEvent : UnityEvent<Result> { }
}