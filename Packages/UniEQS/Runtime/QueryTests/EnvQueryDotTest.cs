using System;
using UnityEngine;

namespace UniEQS.QueryTests
{
    [Serializable]
    public class EnvQueryDotTest : EnvQueryTestBase
    {
        public bool AbsoluteValue;
        
        public DirModeForLineA DirectionA;
        public Vector3 TargetPosition;
        
        public DirModeForLineB DirectionB; 
        public Vector3 ForwardDirection;
        public Vector3 RightDirection;
        public Vector3 LineFromPosition;
        public Vector3 LineToPostision;
        
        public enum DirModeForLineA
        {
            FromTargetToEachItem = 0,
            FromEachItemToTarget = 1,
        }
        
        public enum DirModeForLineB
        {
            DirectionVector_Forward = 0,
            DirectionVector_Backward = 1,
            DirectionVector_Right = 2,
            DirectionVector_Left = 3,
            TwoPoints = 4,
        }
    }
}