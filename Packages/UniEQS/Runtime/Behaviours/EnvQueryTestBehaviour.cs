using UnityEngine;

namespace UniEQS.Behaviours
{
    public abstract class EnvQueryTestBehaviour : MonoBehaviour
    {
        public abstract EnvQueryTestBase QueryTest { get; }
    }
}