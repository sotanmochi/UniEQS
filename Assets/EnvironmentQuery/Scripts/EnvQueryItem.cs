using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnvQueryItem
{
    public float Score;
    public bool IsValid;
    public float[] TestResults;

    private Transform centerOfItems; // 基準位置
    private Vector3 location; // 相対位置
    private Vector3 navLocation; // 相対位置（NavMesh投影後）

    public EnvQueryItem(int numTests, Vector3 location, Transform centerOfItems)
    {
        Score = 0.0f;
        IsValid = true;
        TestResults = new float[numTests];
        this.centerOfItems = centerOfItems;
        this.location = location;
        this.navLocation = location;
    }

    public Vector3 GetWorldPosition()
    {
        return centerOfItems.position + navLocation;
    }

    public void UpdateNavMeshProjection()
    {
        IsValid = true;

        NavMeshHit result;
        Vector3 worldPosition = centerOfItems.position + location;
        NavMesh.SamplePosition(worldPosition, out result, 3.0f, NavMesh.AllAreas);

        float diff = (result.position.x - worldPosition.x)*(result.position.x - worldPosition.x)
                   + (result.position.z - worldPosition.z)*(result.position.z - worldPosition.z);

        if(result.hit && diff < 0.00000001f)
        {
            IsValid = true;
            navLocation = result.position - centerOfItems.position;
        }
        else
        {
            IsValid = false;
            navLocation = location;
        }
    }
}