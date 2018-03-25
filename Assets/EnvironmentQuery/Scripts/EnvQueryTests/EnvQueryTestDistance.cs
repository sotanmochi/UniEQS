using System.Collections.Generic;
using UnityEngine;

public class EnvQueryTestDistance : EnvQueryTest
{
    public Transform DistanceTo;

    public override void RunTest(int currentTest, List<EnvQueryItem> envQueryItems)
    {
        if(IsActive && DistanceTo != null && envQueryItems != null)
        {
            foreach(EnvQueryItem item in envQueryItems)
            {
                item.TestResults[currentTest] = Vector3.Distance(DistanceTo.position, item.GetWorldPosition());
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