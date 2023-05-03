using UnityEngine;
using UniEQS.QueryTests;

namespace UniEQS.Behaviours.QueryTests
{
    public class EnvQueryDistanceTestBehaviour : EnvQueryTestBehaviour
    {
        [SerializeField] private EnvQueryDistanceTest _distanceTest;
        [SerializeField] private Transform _distanceToPosition;
        
        public override EnvQueryTestBase QueryTest => _distanceTest;
        
        private void OnEnable()
        {
            _distanceTest.Enabled = true;
        }

        private void OnDisable()
        {
            _distanceTest.Enabled = false;
        }
        
        private void Update()
        {
            _distanceTest.DistanceToPosition = _distanceToPosition.position;
        }
    }
}