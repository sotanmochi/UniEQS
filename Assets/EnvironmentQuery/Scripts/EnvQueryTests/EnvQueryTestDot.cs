using System.Collections.Generic;
using UnityEngine;

public class EnvQueryTestDot : EnvQueryTest
{
    public enum DirModeForLineA
    {
        FromTargetToEachItem,
        FromEachItemToTarget,
    }

    public enum DirModeForLineB
    {
        DirectionVector_Forward,
        DirectionVector_Backward,
        DirectionVector_Right,
        DirectionVector_Left,
        TwoPoints,
    }

    public bool AbsoluteValue;

    public DirModeForLineA DirectionA;
    public Transform Target;

    public DirModeForLineB DirectionB; 
    public GameObject DirectionVectorObj;
    public Transform LineFrom;
    public Transform LineTo;

    public override void RunTest(int currentTest, List<EnvQueryItem> envQueryItems)
    {
        Vector3 a = Vector3.zero;
        Vector3 b = Vector3.zero;
        float dotValue = 0.0f;

        if(IsActive && Target != null && envQueryItems != null)
        {
            foreach(EnvQueryItem item in envQueryItems)
            {
                a.Set(0.0f,0.0f,0.0f);
                b.Set(0.0f,0.0f,0.0f);
                dotValue = 0.0f;

                if(DirectionA == DirModeForLineA.FromTargetToEachItem)
                {
                    a = (item.GetWorldPosition() - Target.position).normalized;
                }
                else if(DirectionA == DirModeForLineA.FromEachItemToTarget)
                {
                    a = (Target.position - item.GetWorldPosition()).normalized;
                }

                if(DirectionB == DirModeForLineB.TwoPoints && LineFrom != null && LineTo != null)
                {
                    b = (LineTo.position - LineFrom.position).normalized;
                }
                else if(DirectionB == DirModeForLineB.DirectionVector_Forward && DirectionVectorObj != null)
                {
                    b = DirectionVectorObj.transform.forward;
                }
                else if(DirectionB == DirModeForLineB.DirectionVector_Backward && DirectionVectorObj != null)
                {
                    b = -(DirectionVectorObj.transform.forward);
                }
                else if(DirectionB == DirModeForLineB.DirectionVector_Right && DirectionVectorObj != null)
                {
                    b = DirectionVectorObj.transform.right;
                }
                else if(DirectionB == DirModeForLineB.DirectionVector_Left && DirectionVectorObj != null)
                {
                    b = -(DirectionVectorObj.transform.right);
                }

                dotValue = Vector3.Dot(a, b);
                if(AbsoluteValue)
                {
                    dotValue = Mathf.Abs(dotValue);
                }

                item.TestResults[currentTest] = dotValue;
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
}