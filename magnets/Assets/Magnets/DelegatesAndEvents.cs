using System;
using UnityEngine;
using UnityEngine.Events;

namespace Magnets
{
    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    [Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }
}