using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UniEQS.QueryTests;

namespace UniEQS.System
{
    public struct EnvQueryDistanceTestJob : IJobParallelFor
    {
        [ReadOnly] public bool Enabled;
        [ReadOnly] public NativeArray<EnvQueryTestItem> Items;
        [WriteOnly] public NativeArray<EnvQueryTestResult> Results;
        
        [ReadOnly] public Vector3 DistanceToPosition;
        
        public void Execute(int index)
        {
            if (!Enabled)
            {
                Results[index] = new EnvQueryTestResult(EnvQueryTestType.Distance, 0);
            }
            else
            {
                var distance = Vector3.Distance(DistanceToPosition, Items[index].WorldPosition);
                Results[index] = new EnvQueryTestResult(EnvQueryTestType.Distance, distance);
            }
        }
    }
    
    public struct EnvQueryDotTestJob : IJobParallelFor
    {
        [ReadOnly] public bool Enabled;
        [ReadOnly] public NativeArray<EnvQueryTestItem> Items;
        [WriteOnly] public NativeArray<EnvQueryTestResult> Results;
        
        [ReadOnly] public bool AbsoluteValue;
        
        [ReadOnly] public EnvQueryDotTest.DirModeForLineA DirectionA;
        [ReadOnly] public Vector3 TargetPosition;
        
        [ReadOnly] public EnvQueryDotTest.DirModeForLineB DirectionB;
        [ReadOnly] public Vector3 ForwardDirection;
        [ReadOnly] public Vector3 RightDirection;
        [ReadOnly] public Vector3 LineFromPosition;
        [ReadOnly] public Vector3 LineToPosition;
        
        public void Execute(int index)
        {
            if (!Enabled)
            {
                Results[index] = new EnvQueryTestResult(EnvQueryTestType.Dot, 0);
                return;
            }
            
            var a = Vector3.zero;
            var b = Vector3.zero;
            
            if (DirectionA == EnvQueryDotTest.DirModeForLineA.FromTargetToEachItem)
            {
                a = (Items[index].WorldPosition - TargetPosition).normalized;
            }
            else if (DirectionA == EnvQueryDotTest.DirModeForLineA.FromEachItemToTarget)
            {
                a = (TargetPosition - Items[index].WorldPosition).normalized;
            }
            
            if (DirectionB == EnvQueryDotTest.DirModeForLineB.TwoPoints)
            {
                b = (LineToPosition - LineFromPosition).normalized;
            }
            else if (DirectionB == EnvQueryDotTest.DirModeForLineB.DirectionVector_Forward)
            {
                b = ForwardDirection;
            }
            else if (DirectionB == EnvQueryDotTest.DirModeForLineB.DirectionVector_Backward)
            {
                b = -ForwardDirection;
            }
            else if (DirectionB == EnvQueryDotTest.DirModeForLineB.DirectionVector_Right)
            {
                b = RightDirection;
            }
            else if (DirectionB == EnvQueryDotTest.DirModeForLineB.DirectionVector_Left)
            {
                b = -RightDirection;
            }
            
            var dot = AbsoluteValue ? Mathf.Abs(Vector3.Dot(a, b)) : Vector3.Dot(a, b);
            Results[index] = new EnvQueryTestResult(EnvQueryTestType.Dot, dot);
        }
    }
}