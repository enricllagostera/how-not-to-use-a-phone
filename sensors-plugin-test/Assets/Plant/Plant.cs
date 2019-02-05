using System.Collections.Generic;
using Prime31.ZestKit;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public List<Transform> branchPoints;
    public Transform branchPrefab;
    public float changeTargetInterval;
    public float timer;
    public float distance;
    public float verticalGrowth;
    private Vector2 newPos;

    void Start()
    {
        timer = changeTargetInterval;
        branchPoints.Add(branchPrefab);
        CreatePoint(transform);
        ChangeTarget(branchPoints[branchPoints.Count - 1]);
    }

    void Update()
    {
        /* timer += Time.deltaTime;
        if (timer >= changeTargetInterval)
        {
            timer = 0f;
            ChangeTarget(branchPoints[branchPoints.Count - 1]);
        } */
    }

    void CreatePoint(Transform lastPoint)
    {
        var newPoint = GameObject.Instantiate(branchPrefab, lastPoint.position, Quaternion.identity);
        newPoint.SetParent(transform, true);
        newPoint.SetAsLastSibling();
        branchPoints.Add(newPoint);
    }

    void ChangeTarget(Transform point)
    {
        newPos = (Vector2)point.position;
        newPos += (new Vector2(Random.Range(-distance, distance), verticalGrowth)).normalized * distance;
        CreatePoint(branchPoints[branchPoints.Count - 1]);
        branchPoints[branchPoints.Count - 1].ZKpositionTo(newPos, changeTargetInterval)
         .setCompletionHandler(tw =>
         {
             Debug.Log("Tween complete");
             ChangeTarget(branchPoints[branchPoints.Count - 1]);
         })
         .setEaseType(EaseType.CubicInOut)
         .start();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(newPos, 0.5f);
    }

}