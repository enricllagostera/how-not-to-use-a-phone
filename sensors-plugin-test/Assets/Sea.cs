using System;
using System.Collections;
using System.Collections.Generic;
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

        void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
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
            _sprite.color = info.backgroundPulse.Evaluate(swayIndex);
        }

        public void CreateCoral(Coral parent = null)
        {
            Coral coral;
            if (parent == null)
            {
                Debug.Log("START NEW ROOT CORAL");
                coral = GameObject.Instantiate<Coral>(prefabCoral, Random.insideUnitCircle * Random.Range(1f, 4f), Quaternion.identity, this.transform);
                coral.isRoot = true;
            }
            else
            {
                Debug.Log("NEW CORAL BRANCH");
                coral = GameObject.Instantiate<Coral>(prefabCoral, parent.transform.position + (Vector3)Random.insideUnitCircle.normalized * parent.info.branchScaleDecay / 2f, Quaternion.identity, parent.transform);
                coral.isRoot = false;
                coral.parent = parent;
            }
            coral.sea = this;
            coral.isLeaf = false;
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