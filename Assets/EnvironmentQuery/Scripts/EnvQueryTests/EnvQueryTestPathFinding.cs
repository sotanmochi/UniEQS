using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnvQueryTestPathFinding : EnvQueryTest
{
    public enum PathFindingTestType
    {
        PathExist,
        PathLength
    }

    public PathFindingTestType PathFindingType;
    public Transform Target;

    public override void RunTest(int currentTest, List<EnvQueryItem> envQueryItems)
    {
        if(IsActive && Target != null && envQueryItems != null)
        {
            NavMeshPath path = new NavMeshPath();
            if(PathFindingType == PathFindingTestType.PathExist)
            {
                foreach(EnvQueryItem item in envQueryItems)
                {
                    NavMesh.CalculatePath(item.GetWorldPosition(), Target.position, NavMesh.AllAreas, path);
                    item.TestResults[currentTest] = (path.status == NavMeshPathStatus.PathComplete) ? 1.0f : 0.0f;
                }
            }
            else if(PathFindingType == PathFindingTestType.PathLength)
            {
                foreach(EnvQueryItem item in envQueryItems)
                {
                    NavMesh.CalculatePath(item.GetWorldPosition(), Target.position, NavMesh.AllAreas, path);
                    if(path.status == NavMeshPathStatus.PathComplete)
                    {
                        // 距離が近い場合にスコアが高くなるようにする
                        item.TestResults[currentTest] = -CalculatePathLength(item.GetWorldPosition(), path);
                    }
                    else
                    {
                        // パスが到達しない場合は最もスコアが低くなるようにする
                        item.TestResults[currentTest] = -10000;
                    }
                }
            }
        }
        else
        {
            foreach(EnvQueryItem item in envQueryItems)
            {
                item.TestResults[currentTest] = 0.0f;
            }
        }
    }

    private float CalculatePathLength(Vector3 startPosition, NavMeshPath path)
    {
        if(path.corners.Length < 1)
        {
            return 0.0f;
        }

        float lengthSoFar = Vector3.Distance(startPosition, path.corners[0]);
        Vector3 previousCorner = path.corners[0];
        for(int i = 1; i < path.corners.Length; i++)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
        }

        return lengthSoFar;
    }
}