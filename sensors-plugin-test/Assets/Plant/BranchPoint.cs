using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.ZestKit;

public class BranchPoint : MonoBehaviour
{
    public float changeTargetInterval;
    public float timer;
    public float distance;

    void Start()
    {
        timer = changeTargetInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeTargetInterval)
        {
            timer = 0f;
            ChangeTarget();
        }
    }

    void ChangeTarget()
    {
        var newPos = Random.insideUnitCircle.normalized * distance;
        newPos.y = Mathf.Max(newPos.y, transform.position.y);
        transform.ZKpositionTo(newPos, changeTargetInterval)
         .setCompletionHandler(tw => Debug.Log("Tween complete"))
         .setIsRelative()
         .setEaseType(EaseType.CubicInOut)
         .start();
    }
}
