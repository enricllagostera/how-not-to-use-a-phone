using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shellphone
{
    public class Coral : MonoBehaviour
    {
        public Sea sea;
        public Coral prefabCoral;
        public CoralInfo info;
        public Coral parent;
        public bool isRoot;
        public bool isLeaf;
        public bool hasStopped;
        public float nextRollTime;
        public float baseScale;
        public float chanceToBranch;
        public float chanceToStop;
        private SpriteRenderer _sprite;

        void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.color = info.initialColors.Evaluate(sea.health);
            if (isRoot)
            {
                baseScale = Random.Range(info.baseStartScale * (1f - info.baseScaleRange), info.baseStartScale * (1f + info.baseScaleRange));
            }
            else
            {
                baseScale = 1f * info.branchScaleDecay;
            }
            transform.localScale = Vector3.one * baseScale;
            chanceToBranch = 0f;
            chanceToStop = 0f;
            hasStopped = false;
            nextRollTime = CalculateTimeToNextRoll();
        }

        private float CalculateTimeToNextRoll()
        {
            return Time.realtimeSinceStartup + Random.Range(info.branchCooldown * (1f - info.branchCooldownRange), info.branchCooldown * (1f + info.branchCooldownRange));
        }

        void Update()
        {
            if (hasStopped)
            {
                return;
            }

            if (nextRollTime <= Time.realtimeSinceStartup)
            {
                nextRollTime = CalculateTimeToNextRoll();
                UpdateChanceToStop();
                UpdateChanceToBranch();
                if (WillBranchOut())
                {
                    Debug.Log(name + " BRANCH OUT");
                    sea.CreateCoral(this);
                }
                else if (WillStopBranching())
                {
                    Debug.Log(name + " STOPPED BRANCHING");
                    hasStopped = true;
                }
            }
        }

        private void UpdateChanceToBranch()
        {
            chanceToBranch = info.baseChanceToBranch;
        }

        private void UpdateChanceToStop()
        {
            chanceToStop = info.baseChanceToStop;
        }

        private bool WillStopBranching()
        {
            return (chanceToStop <= Random.value);
        }

        private bool WillBranchOut()
        {
            return (chanceToBranch <= Random.value);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, baseScale);
        }
    }

}