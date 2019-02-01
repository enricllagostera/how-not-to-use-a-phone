using System;
using System.Collections;
using System.Collections.Generic;
using UnityAndroidSensors.Scripts.Utils.SmartVars;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shellphone
{
    public class Sea : MonoBehaviour
    {
        public SeaInfo info;
        public Coral prefabCoral;
        public float health = 1f;
        public float chanceToStartNewCoral = 1f;
        public float swayIndex;
        private SpriteRenderer _sprite;
        public Vector3Var magneticData;

        public Vector2 debugGravity;

        void Start()
        {
            Input.gyro.enabled = true;
            _sprite = GetComponent<SpriteRenderer>();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        void Update()
        {
            // game logic
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

        public void CreateCoral(Coral parent = null)
        {
            Coral coral;
            if (parent == null)
            {
                Debug.Log("START NEW ROOT CORAL");
                coral = GameObject.Instantiate<Coral>(prefabCoral, LocateNewCoral(), Quaternion.identity);
                coral.depthLevel = 0;
                coral.seeds = 20;
                coral.transform.SetParent(this.transform);
                coral.baseAngle = Random.Range(0, 360f);
            }
            else
            {
                // Debug.Log("NEW CORAL BRANCH");
                coral = GameObject.Instantiate<Coral>(prefabCoral, Vector2.zero, Quaternion.identity);
                LocateCoralPart(coral, parent);
                coral.depthLevel = parent.depthLevel + 1;
                coral.parent = parent;
                coral.transform.SetParent(parent.transform, false);
                parent.DropSeed();
            }
            coral.sea = this;
        }

        private void LocateCoralPart(Coral coral, Coral parent)
        {
            Vector2 basePos = Vector2.up * 0.5f;
            Vector2 direction = -((Vector2)coral.transform.localPosition - basePos);
            float angle = Vector2.Angle(Vector2.up, direction);
            var rotation = Quaternion.AngleAxis(Random.Range(-100f, 100f), Vector3.forward);




#if UNITY_EDITOR
            Vector2 gravityOffset = debugGravity.normalized;
#else
            Vector2 gravityOffset = new Vector2(Input.gyro.gravity.x, Input.gyro.gravity.y).normalized;
            float gravity = Vector2.Angle(Vector2.up, gravityOffset);
#endif

            print(rotation.eulerAngles);
            coral.transform.position = (gravityOffset.normalized + (Vector2)rotation.eulerAngles.normalized) * 0.25f;

            /* 
                        
            #if UNITY_EDITOR
                        float gravity = Vector2.Angle(Vector2.up, debugGravity);
            #else
                        float gravity = Vector2.Angle(Vector2.up, gravityOffset);
            #endif
                        print(gravity);
                        float randomOffset = Random.Range(-15f, 15f);

                        coral.baseAngle = gravity;
                        coral.transform.Rotate(0, 0, coral.baseAngle);
                        */
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