using UnityEngine;

namespace UniEQS.Behaviours
{
    public abstract class EnvQueryGeneratorBehaviour : MonoBehaviour
    {
        public abstract IEnvQueryGenerator Generator { get; }
    }
}