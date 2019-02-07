using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shellphone
{
    [CreateAssetMenu(fileName = "MoodInfo", menuName = "Shellphone/MoodInfo", order = 0)]
    public class MoodInfo : ScriptableObject
    {
        public Gradient colorRange;

    }
}