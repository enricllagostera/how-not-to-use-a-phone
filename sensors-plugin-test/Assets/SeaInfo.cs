using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shellphone
{
    [CreateAssetMenu(fileName = "SeaInfo", menuName = "Shellphone/SeaInfo", order = 0)]
    public class SeaInfo : ScriptableObject
    {
        public float seaBPM = 60f;

        // all rates are per millisecond
        public float healthBaseRate = 10;
        public float newCoralCooldown = 500;

        // all chance variables need to have a lower-equal than roll
        // and be between 0-1f
        public float chanceToStartNewCoral;
        public Gradient backgroundPulse;

        public float CalculateAndSetChanceToStartNewCoral(float health)
        {
            chanceToStartNewCoral = health;
            return chanceToStartNewCoral;
        }

    }
}