using System.Collections.Generic;
using UnityEngine;

public class EnvQueryTestTrace : EnvQueryTest
{
    private enum TraceType
    {
        Visible,
        Invisible
    }

    // [SerializeField]
    // private GameObject querier;

    [SerializeField]
    private TraceType traceType;

    public Transform TraceFrom; // TraceFrom

    public float ItemHeightOffset;
    public float TargetHeightOffset;

    public EnvQueryTestTrace()
    {
        traceType = TraceType.Visible;
    }

    public override void RunTest(int currentTest, List<EnvQueryItem> envQueryItems)
    {
        if(IsActive && TraceFrom != null && envQueryItems != null)
        {
            foreach(EnvQueryItem item in envQueryItems)
            {
                Vector3 itemPosition = item.GetWorldPosition() + Vector3.up * ItemHeightOffset;
                // Vector3 direction =  itemPosition - TraceFrom.position;
                Vector3 direction = (TraceFrom.position + Vector3.up * TargetHeightOffset) - itemPosition;

                RaycastHit raycastHit;
                Physics.Raycast(itemPosition, direction, out raycastHit);
                // Physics.Raycast(TraceFrom.position, direction, out raycastHit);

                if(raycastHit.transform == TraceFrom)
                {
                    if(traceType == TraceType.Visible)
                    {
                        item.TestResults[currentTest] = 1.0f;
                    }
                    else if(traceType == TraceType.Invisible)
                    {
                        item.TestResults[currentTest] = -1.0f;
                    }
                }
                else{
                    item.TestResults[currentTest] = 0.0f;
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
}