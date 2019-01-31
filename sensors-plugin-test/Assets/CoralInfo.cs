using UnityEngine;

[CreateAssetMenu(fileName = "CoralInfo", menuName = "Shellphone/CoralInfo", order = 1)]
public class CoralInfo : ScriptableObject
{
    public float baseStartScale = 3f;
    public float baseScaleRange = 0.2f;
    public float baseChanceToBranch = 0.5f;
    public float baseChanceToStop = 0.01f;
    public float branchCooldown = 1000;
    public float branchCooldownRange = 0.2f;
    public float stopCooldown = 10000;
    public float baseChanceToLeaf = 0.1f;
    public Gradient initialColors;
    public float branchScaleDecay = 0.75f;
}