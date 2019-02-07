using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CoralInfo", menuName = "Shellphone/CoralInfo", order = 1)]
public class CoralInfo : ScriptableObject
{
    [MinMaxSlider(1f, 10f)]
    public Vector2 startScale;
    [MinMaxSlider(1f, 30f)]
    public Vector2 startSeeds;
    public float baseChanceToBranch = 0.5f;
    public float baseChanceToStop = 0.01f;

    [MinMaxSlider(0f, 30f)]
    public Vector2 branchCooldownRange;
    public float stopCooldown = 10000;
    public float branchScaleDecay = 0.75f;
    public int maxDepthLevel = 10;
    public AnimationCurve healthChangeRatePerSeaHealth;
    [MinMaxSlider(60f, 2400f)] public Vector2 deadCleanupInterval;
}