using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityAndroidSensors.Scripts.Utils.SmartVars;
using UnityEngine;
using VibeUtils;
using Random = UnityEngine.Random;

namespace Shellphone
{
    public class Sea : MonoBehaviour
    {
        public SeaInfo info;
        public Coral prefabCoral;
        [ProgressBar("Health Change Rate", 1f, ProgressBarColor.Red)] public float healthChangeRate;
        public bool isCharging;
        public float healthPerSecondModifier;
        public AnimationCurve healthRateCurve;
        [ProgressBar("Health", 1f, ProgressBarColor.Green)] public float health = 1f;
        public float chanceToStartNewCoral = 1f;
        public float swayIndex;
        private SpriteRenderer _sprite;
        public Vector3Var magneticData;
        public Vector3Var accelerationData;
        public FloatVar lightSensorData;
        public FloatVar proximityData;

        [Slider(0f, 1f)] public float mood;
        [Slider(0f, 1f)] public float moodLimit;
        [Slider(0.1f, 5f)] public float moodChangeFactor;


        private float targetMood;
        public MoodInfo sleepyMoodInfo, chirpyMoodInfo, moodInfo;
        public CurvePlayer warningVFX;
        [Header("Damage by movement logic")]
        public float movementThreshold;
        [ReadOnly] public float damageCooldown;
        public float damageInterval;
        public float damageAmount;
        [ReadOnly] public bool damageMode;
        public bool damageDurationFromFXCurve;
        public CurvePlayer damageVFX;

        [Header("Debug controls")]
        public Vector2 debugGravity;
        public float debugLight;
        [Slider(0f, 8f)] public float debugProximity;
        [Slider(0f, 1f)] public float debugBatteryLevel;
        public bool debugCharging;

        void Start()
        {
            Input.gyro.enabled = true;
            _sprite = GetComponent<SpriteRenderer>();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            if (damageDurationFromFXCurve) damageInterval = damageVFX.duration + 0.2f;
        }

        void Update()
        {
            // game logic
            HandleProximity();
            UpdateHealth();
            // shake detection changing health
            DetectMovement();
            UpdateMood();
            UpdateChanceOfNewCoral();
            if (WillStartNewCoral())
            {
                chanceToStartNewCoral = 0f;
                CreateCoral();
            }
            // visual update
            swayIndex = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * info.seaBPM / 60f));
            var healthColor = info.healthGradient.Evaluate(health);
            healthColor.a = swayIndex;
            _sprite.color = healthColor;
        }

        private void DetectMovement()
        {
            float accelerationMag = 0f;
#if UNITY_EDITOR
            accelerationMag = Input.acceleration.sqrMagnitude;
#else
            accelerationMag = accelerationData.value.sqrMagnitude;
#endif

            if (accelerationMag > movementThreshold)
            {
                if (damageCooldown <= 0f)
                {
                    damageMode = true;
                    health -= damageAmount;
                    damageCooldown = damageInterval;
                    damageVFX.Play();
                }
            }
            else
            {
                // only releases vibration after finishing cooldown
                if (damageCooldown <= 0f)
                {
                    damageMode = false;
                    if (damageVFX.isPlaying) damageVFX.Stop();
                }
            }
            damageCooldown -= Time.deltaTime;
        }

        private void UpdateHealth()
        {
            float batteryLevel = 0f;
#if UNITY_EDITOR
            batteryLevel = debugBatteryLevel;
            isCharging = debugCharging;
#else
            batteryLevel = SystemInfo.batteryLevel;
            isCharging = (SystemInfo.batteryStatus == BatteryStatus.Charging);
#endif
            healthChangeRate = 1f - batteryLevel;
            float applyRate = (isCharging) ? 1f : healthRateCurve.Evaluate(batteryLevel);
            health = Mathf.Lerp(health, health + applyRate, Time.deltaTime * healthPerSecondModifier);
            health = Mathf.Clamp01(health);
        }

        private void HandleProximity()
        {
            // skip if is giving feedback on damage, which is higher priority
            if (damageMode)
            {
                return;
            }
            float proximity = 0f;
#if UNITY_EDITOR
            proximity = debugProximity;
#else
            proximity = proximityData.value;
#endif

            if (proximity >= 0.1f)
            {
                // it's ok
                warningVFX.Stop();
            }
            else if (!warningVFX.isPlaying)
            {
                warningVFX.Play();
                print("LET ME GO");
            }
        }

        private void UpdateMood()
        {
#if UNITY_EDITOR
            targetMood = Utils.Remap(debugLight, 0f, 1000f, 0f, 1f); ;
#else
            targetMood = Utils.Remap(lightSensorData.value, 0f, 1000f, 0f, 1f);
#endif
            mood = Mathf.Lerp(mood, targetMood, Time.deltaTime * moodChangeFactor);
            if (mood <= moodLimit)
            {
                // set sleepy
                moodInfo = sleepyMoodInfo;
            }
            else
            {
                // set chirpy
                moodInfo = chirpyMoodInfo;
            }
        }

        public void CreateCoral(Coral parent = null)
        {
            Coral coral;
            if (parent == null)
            {
                Debug.Log("START NEW ROOT CORAL");
                coral = GameObject.Instantiate<Coral>(prefabCoral, LocateNewCoral(), Quaternion.identity);
                coral.depthLevel = 0;
                var random = (Vector2)Random.insideUnitCircle.normalized;
                Vector2 gravity = new Vector2();
#if UNITY_EDITOR
                gravity = debugGravity.normalized;
#elif UNITY_ANDROID
            gravity = new Vector2(Input.gyro.gravity.x, Input.gyro.gravity.y).normalized;
#endif
                var up = random + gravity * 2f;
                coral.transform.up = up.normalized;
            }
            else
            {
                // Debug.Log("NEW CORAL BRANCH");
                coral = GameObject.Instantiate<Coral>(prefabCoral, Vector2.zero, Quaternion.identity);
                LocateCoralPart(coral, parent);
                coral.depthLevel = parent.depthLevel + 1;
                coral.parent = parent;
                parent.DropSeed();
            }
            coral.sea = this;
        }

        private void LocateCoralPart(Coral coral, Coral parent)
        {
            var parentDirection = Vector2.up;
            var randomVector = (Vector2)Random.insideUnitCircle.normalized;
            var pos = parentDirection * 3f + randomVector;
            coral.transform.position = pos.normalized * 0.5f;
            coral.transform.SetParent(parent.transform, false);
        }

        private Vector3 LocateNewCoral()
        {
            return Random.insideUnitCircle * Random.Range(2f, 10f);
        }

        bool WillStartNewCoral()
        {
            return (chanceToStartNewCoral >= Random.value);
        }

        void UpdateChanceOfNewCoral()
        {
            info.CalculateAndSetChanceToStartNewCoral(health);
            // this effects a cooldown
            chanceToStartNewCoral = Mathf.Lerp(chanceToStartNewCoral, info.chanceToStartNewCoral, Time.deltaTime / info.newCoralCooldown);
        }
    }

}