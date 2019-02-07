using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;

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
        public float hueIndex;
        public int seeds;
        [MinMaxSlider(0f, 1f)] public Vector2 saturation;

        void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
            hueIndex = 0f;

            if (IsRoot())
            {
                baseScale = Random.Range(info.startScale.x, info.startScale.y);
                seeds = Mathf.CeilToInt(Random.Range(info.startSeeds.x, info.startSeeds.y));
#if UNITY_EDITOR
                hueIndex = Utils.Remap(sea.debugLight, 0f, 1000f, 0f, 1f); ;
#else
            hueIndex = Utils.Remap(sea.lightSensorData.value, 0f, 1000f, 0f, 1f);
#endif
            }
            else
            {
                baseScale = 1f * info.branchScaleDecay;
                hueIndex = parent.hueIndex;
            }
            transform.localScale = Vector3.one * baseScale;
            chanceToBranch = 0f;
            chanceToStop = 0f;
            hasStopped = false;
            nextRollTime = CalculateTimeToNextRoll();



            float value = 1f - Utils.Remap(depthLevel, 0f, info.maxDepthLevel, 0.2f, 0.8f);
            _sprite.color = Random.ColorHSV(hueIndex, hueIndex, saturation.x, saturation.y, value, value);
        }

        public bool IsRoot()
        {
            return depthLevel == 0;
        }

        private float CalculateTimeToNextRoll()
        {
            return Time.realtimeSinceStartup + Random.Range(info.branchCooldownRange.x, info.branchCooldownRange.y);
        }

        void Update()
        {

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