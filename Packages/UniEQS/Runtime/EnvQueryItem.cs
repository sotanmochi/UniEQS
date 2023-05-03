using UnityEngine;
using UnityEngine.AI;

namespace UniEQS
{
    public class EnvQueryItem
    {
        public bool Enabled;
        public float Score;
        public float[] TestResults;
        public Vector3 RelativePosition;
        public Transform ReferencePosition;

        private Vector3 NavPosition; // Relative position (projected onto NavMesh)
        
        public EnvQueryItem(int numberOfTests, Vector3 relativePosition = default, Transform referencePosition = null)
        {
            Enabled = true;
            Score = 0.0f;
            TestResults = new float[numberOfTests];
            RelativePosition = relativePosition;
            ReferencePosition = referencePosition;
        }

        public Vector3 GetNavPosition()
        {
            return ReferencePosition.position + NavPosition;
        }

        public Vector3 GetPosition()
        {
            return ReferencePosition.position + RelativePosition;
        }

        public void UpdateNavMeshProjection()
        {
            NavMeshHit result;
            Vector3 worldPosition = ReferencePosition.position + RelativePosition;
            NavMesh.SamplePosition(worldPosition, out result, 3.0f, NavMesh.AllAreas);

            float diff = (result.position.x - worldPosition.x) * (result.position.x - worldPosition.x)
                       + (result.position.z - worldPosition.z) * (result.position.z - worldPosition.z);

            if (result.hit && diff < 0.00000001f)
            {
                Enabled = true;
                NavPosition = result.position - ReferencePosition.position;
            }
            else
            {
                Enabled = false;
                NavPosition = RelativePosition;
            }
        }
    }
}