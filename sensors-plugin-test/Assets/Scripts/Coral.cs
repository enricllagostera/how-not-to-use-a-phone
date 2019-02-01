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
        public int depthLevel;
        public bool isLeaf;
        public bool hasStopped;
        public float nextRollTime;
        public float baseScale;
        public float baseAngle;
        public float chanceToBranch;
        public float chanceToStop;
        private SpriteRenderer _sprite;
        public int seeds;

        void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();

            if (IsRoot())
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

        public bool IsRoot()
        {
            return depthLevel == 0;
        }

        private float CalculateTimeToNextRoll()
        {
            return Time.realtimeSinceStartup + Random.Range(info.branchCooldown * (1f - info.branchCooldownRange), info.branchCooldown * (1f + info.branchCooldownRange));
        }

        void Update()
        {
            _sprite.color = info.initialColors.Evaluate(1f - Utils.Remap(depthLevel, 0f, info.maxDepthLevel, 0f, 1f));
            _sprite.sortingOrder = depthLevel;
            if (hasStopped)
            {
                this.enabled = false;
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

        public void DropSeed()
        {
            if (!IsRoot())
            {
                parent.DropSeed();
            }
            else
            {
                seeds--;
                if (seeds <= 0)
                {
                    this.hasStopped = true;
                    this.gameObject.name += " IDLE-ROOT";
                    var allCorals = this.GetComponentsInChildren<Coral>();
                    foreach (var coral in allCorals)
                    {
                        coral.hasStopped = true;
                        coral.gameObject.name += " STOPPED";
                    }
                }
            }
        }

        private void UpdateChanceToBranch()
        {
            if (depthLevel < info.maxDepthLevel)
            {
                chanceToBranch = info.baseChanceToBranch;
            }
            else
            {
                chanceToBranch = Mathf.Infinity;
            }

        }

        private void UpdateChanceToStop()
        {
            if (depthLevel < info.maxDepthLevel)
            {
                chanceToStop = info.baseChanceToStop;
            }
            else
            {
                chanceToStop = 0f;
            }
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, sea.debugGravity);
        }

    }

}