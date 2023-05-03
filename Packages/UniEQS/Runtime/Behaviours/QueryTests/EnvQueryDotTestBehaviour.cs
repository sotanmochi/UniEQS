using UnityEngine;
using UniEQS.QueryTests;

namespace UniEQS.Behaviours.QueryTests
{
    public class EnvQueryDotTestBehaviour : EnvQueryTestBehaviour
    {
        [SerializeField] private EnvQueryDotTest _dotTest;
        
        [SerializeField] private bool _absoluteValue;
        
        [SerializeField] private EnvQueryDotTest.DirModeForLineA _directionA;
        [SerializeField] private Transform _targetPosition;
        
        [SerializeField] private EnvQueryDotTest.DirModeForLineB _directionB;
        [SerializeField] private Transform _forwardDirection;
        [SerializeField] private Transform _rightDirection;
        [SerializeField] private Transform _lineFromPosition;
        [SerializeField] private Transform _lineToPosition;
        
        public override EnvQueryTestBase QueryTest => _dotTest;
        
        private void OnEnable()
        {
            _dotTest.Enabled = true;
        }
        
        private void OnDisable()
        {
            _dotTest.Enabled = false;
        }
        
        private void Update()
        {
            _dotTest.AbsoluteValue = _absoluteValue;
            
            _dotTest.DirectionA = _directionA;
            _dotTest.TargetPosition = _targetPosition.position;
            
            _dotTest.DirectionB = _directionB;
            
            if (_directionB is EnvQueryDotTest.DirModeForLineB.DirectionVector_Forward
                || _directionB is EnvQueryDotTest.DirModeForLineB.DirectionVector_Backward)
            {
                _dotTest.ForwardDirection = _forwardDirection.forward;
            }
            else if (_directionB is EnvQueryDotTest.DirModeForLineB.DirectionVector_Right 
                    || _directionB is EnvQueryDotTest.DirModeForLineB.DirectionVector_Left)
            {
                _dotTest.RightDirection = _rightDirection.right;
            }
            else if (_directionB is EnvQueryDotTest.DirModeForLineB.TwoPoints)
            {
                _dotTest.LineFromPosition = _lineFromPosition.position;
                _dotTest.LineToPostision = _lineToPosition.position;
            }
        }
    }
}